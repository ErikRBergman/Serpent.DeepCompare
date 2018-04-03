﻿namespace Serpent.DeepCompare.NetFramework
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Reflection.Emit;

    public static class Compare
    {
        private static readonly ConcurrentDictionary<Type, Func<object, object, CompareContext, bool>> comparers =
            new ConcurrentDictionary<Type, Func<object, object, CompareContext, bool>>();

        ////public static bool AreEqual<T>(T first, T second)
        ////{
        ////    if (first == null)
        ////    {
        ////        return second == null;
        ////    }

        ////    if (second == null)
        ////    {
        ////        return false;
        ////    }

        ////    if (object.ReferenceEquals(first, second) == false)
        ////    {
        ////        return false;
        ////    }

        ////}
        public static bool AreEqual(object first, object second)
        {
            // return InternalAreEqual(first, second);
            return InternalAreEqual(first, second, CompareContext.Create());
        }

        public static bool InternalAreEqual(object first, object second, CompareContext context)
        {
            // public static bool InternalAreEqual(object first, object second)
            if (first == null)
            {
                return second == null;
            }

            if (second == null)
            {
                return false;
            }

            var firstType = first.GetType();
            var secondType = second.GetType();

            if (firstType != secondType)
            {
                return false;
            }

            if (firstType.IsValueType)
            {
                // TODO: Make this optional. There may be an implementation that we are not interested in
                return first.Equals(second);
            }

            if (firstType == typeof(string))
            {
                return string.CompareOrdinal((string)first, (string)second) == 0;
            }

            // Prevent circular references
            if (context.TraversedTypes.Contains(first))
            {
                return true;
            }

            context.TraversedTypes.Add(first);

            // Get a comparer
            var comparer = GetComparer(firstType);
            return comparer(first, second, context);
        }

        private static Func<object, object, CompareContext, bool> GetComparer(Type firstType)
        {
            return comparers.GetOrAdd(firstType, InternalGetComparer);
        }

        private static Func<object, object, CompareContext, bool> InternalGetComparer(Type compareType)
        {
            var builder = new DynamicMethod("Serpent.DeepCompare.AreEqual_" + compareType.FullName, typeof(bool), new[] { typeof(object), typeof(object), typeof(CompareContext) });

            ////var debugWriteLine = typeof(Debug).GetMethod("WriteLine", new[] { typeof(string) });

            var ordinalStringComparer = typeof(string).GetMethod(nameof(string.CompareOrdinal), new[] { typeof(string), typeof(string) });

            var areEqualMethod = typeof(Compare).GetMethod(nameof(InternalAreEqual), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

            var generator = builder.GetILGenerator();

            var first = generator.DeclareLocal(compareType);
            var second = generator.DeclareLocal(compareType);

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, compareType);
            generator.Emit(OpCodes.Stloc, first);

            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Castclass, compareType);
            generator.Emit(OpCodes.Stloc, second);

            // generator.Emit(OpCodes.Call, debugWriteLine);
            var notEqualLabel = generator.DefineLabel();

            foreach (var property in compareType.GetProperties())
            {
                var getter = property.GetGetMethod();

                if (getter.IsPublic == false)
                {
                    // Ignore non public properties
                    continue;
                }

                var areEqualLabel = generator.DefineLabel();

                // load arguments
                generator.Emit(OpCodes.Ldloc_S, first);
                generator.Emit(OpCodes.Callvirt, getter);

                generator.Emit(OpCodes.Ldloc_S, second);
                generator.Emit(OpCodes.Callvirt, getter);

                ////generator.EmitWriteLine(property.Name);

                ////generator.Emit(OpCodes.Pop);
                ////generator.Emit(OpCodes.Pop);
                if (property.PropertyType == typeof(string))
                {
                    generator.Emit(OpCodes.Call, ordinalStringComparer);

                    // if result == 0, jump to areEqualLabel
                    generator.Emit(OpCodes.Brfalse, areEqualLabel);
                }
                else if (!property.PropertyType.IsValueType)
                {
                    generator.Emit(OpCodes.Ldarg_2); // context

                    generator.Emit(OpCodes.Call, areEqualMethod);
                    generator.Emit(OpCodes.Brtrue, areEqualLabel);
                }
                else
                {
                    generator.Emit(OpCodes.Beq, areEqualLabel);
                }

                generator.Emit(OpCodes.Br, notEqualLabel);

                generator.MarkLabel(areEqualLabel);
            }

            // return true
            generator.Emit(OpCodes.Ldc_I4_1);
            generator.Emit(OpCodes.Ret);

            generator.MarkLabel(notEqualLabel);

            // return false
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Ret);

            // funkar
            // return (Func<object, object, bool>)builder.CreateDelegate(typeof(Func<object, object, bool>));

            // funkar
            // return (Func<bool>)builder.CreateDelegate(typeof(Func<bool>));
            return (Func<object, object, CompareContext, bool>)builder.CreateDelegate(typeof(Func<object, object, CompareContext, bool>));
        }

        public struct CompareContext
        {
            public HashSet<object> TraversedTypes { get; set; }

            public static CompareContext Create()
            {
                return new CompareContext
                           {
                               TraversedTypes = new HashSet<object>()
                           };
            }
        }
    }
}