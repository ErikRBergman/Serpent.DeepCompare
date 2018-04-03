namespace Serpent.DeepCompare.NetFramework
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;

    public static class Compare
    {
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
            return InternalAreEqual(first, second, CompareContext.Create());
        }

        private static bool InternalAreEqual(object first, object second, CompareContext context)
        {
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

            if (ReferenceEquals(first, second) == false)
            {
                return false;
            }

            // Prevent circular references
            if (context.TraversedTypes.Contains(firstType))
            {
                return true;
            }

            context.TraversedTypes.Add(firstType);

            // Get a comparer
            //var comparer = GetComparer(firstType);
            var comparer = InternalGetComparer(firstType);


            // Funkar
            // return comparer();
            //return comparer(first, second);

            return comparer(first, second, context);
        }

        ////private static Dynamic


        private class TestClass
        {
            public int IntValue { get; set; }
        }

        private static bool crap(object x)
        {
            //var first = (TestClass)x;
            //var value = first.IntValue;


            //Compare.InternalAreEqual("123", "123, ")

            return false;
        }

        //private static ConcurrentDictionary<Type, Func<object, object, CompareContext, bool>> comparers = new ConcurrentDictionary<Type, Func<object, object, CompareContext, bool>>();

        //private static Func<object, object, CompareContext, bool> GetComparer(Type firstType)
        //{
        //    return comparers.GetOrAdd(firstType, InternalGetComparer);
        //}

        //private static Func<object, object, CompareContext, bool> InternalGetComparer(Type firstType)

        // funkar
        //private static Func<bool> InternalGetComparer(Type firstType)
        //private static Func<object, object, bool> InternalGetComparer(Type firstType)
        
           
        private static Func<object, object, CompareContext, bool> InternalGetComparer(Type firstType)
        {
            //var builder = new DynamicMethod("Serpent.DeepCompare.AreEqual_" + firstType.FullName, typeof(bool), new[] { typeof(object), typeof(object), typeof(CompareContext) });

            var builder = new DynamicMethod("CompareIt", typeof(bool), new[] { typeof(object), typeof(object), typeof(CompareContext) });
            
            // Funkar
            //var builder = new DynamicMethod("CompareIt", typeof(bool), new[] { typeof(object), typeof(object) });
            //var builder = new DynamicMethod("CompareIt", typeof(bool), Array.Empty<Type>());

            var areEqualMethod = typeof(Compare).GetMethod(nameof(InternalAreEqual));

            var generator = builder.GetILGenerator();

            ////var first = generator.DeclareLocal(firstType);
            ////var second = generator.DeclareLocal(firstType);

            ////generator.Emit(OpCodes.Ldarg_1);
            ////generator.Emit(OpCodes.Castclass, firstType);
            ////generator.Emit(OpCodes.Stloc, first);

            ////generator.Emit(OpCodes.Ldarg_2);
            ////generator.Emit(OpCodes.Castclass, firstType);
            ////generator.Emit(OpCodes.Stloc, second);

            ////var notEqualLabel = generator.DefineLabel();

            ////foreach (var property in firstType.GetProperties())
            ////{
            ////    var getter = property.GetGetMethod();

            ////    if (getter.IsPublic == false)
            ////    {
            ////        // Ignore non public properties
            ////        continue;
            ////    }

            ////    var areEqualLabel = generator.DefineLabel();

            ////    // load arguments
            ////    generator.Emit(OpCodes.Ldloc_S, first);
            ////    generator.Emit(OpCodes.Call, getter);

            ////    generator.Emit(OpCodes.Ldloc_S, second);
            ////    generator.Emit(OpCodes.Call, getter);

            ////    if (!property.PropertyType.IsValueType)
            ////    {
            ////        generator.Emit(OpCodes.Ldarg_3); // context
            ////        generator.Emit(OpCodes.Call, areEqualMethod);

            ////        generator.Emit(OpCodes.Brfalse, notEqualLabel);
            ////        //generator.EmitCall();
            ////    }
            ////    else
            ////    {
            ////        generator.Emit(OpCodes.Beq, areEqualLabel);
            ////    }

            ////    generator.Emit(OpCodes.Br, notEqualLabel);

            ////    generator.MarkLabel(areEqualLabel);
            ////}

            ////// return true
            ////generator.Emit(OpCodes.Ldc_I4_1);
            ////generator.Emit(OpCodes.Ret);

            ////generator.MarkLabel(notEqualLabel);

            // return false
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Ret);

            // funkar
            //return (Func<object, object, bool>)builder.CreateDelegate(typeof(Func<object, object, bool>));

            // funkar
            //return (Func<bool>)builder.CreateDelegate(typeof(Func<bool>));
            
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