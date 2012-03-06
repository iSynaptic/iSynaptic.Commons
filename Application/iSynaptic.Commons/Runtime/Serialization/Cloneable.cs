// The MIT License
// 
// Copyright (c) 2012 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Security.Permissions;
using iSynaptic.Commons.Reflection;
using iSynaptic.Commons.Reflection.Emit;

namespace iSynaptic.Commons.Runtime.Serialization
{
    public static class Cloneable
    {
        public static T Clone<T>(this T source)
        {
            return Cloneable<T>.Clone(source);
        }

        public static T ShallowClone<T>(this T source)
        {
            return Cloneable<T>.ShallowClone(source);
        }

        public static T CloneTo<T>(this T source, T destination)
        {
            return Cloneable<T>.CloneTo(source, destination);
        }

        public static T ShallowCloneTo<T>(this T source, T destination)
        {
            return Cloneable<T>.ShallowCloneTo(source, destination);
        }
    }

    public static class Cloneable<T>
    {
        private static readonly Type[] CloneablePrimitives =
        {
            typeof(IntPtr),
            typeof(UIntPtr),
            typeof(string),
            typeof(decimal),
            typeof(DateTime),
            typeof(TimeSpan),
            typeof(Guid)
        };

        private static readonly Type TargetType = null;
        private static readonly MethodInfo CloneContextGetShouldUseExistingObjectsMethod = null;
        private static readonly MethodInfo CloneContextGetIsShallowCloneMethod = null;
        private static readonly MethodInfo CloneContextGetCloneMapMethod = null;
        private static readonly MethodInfo DictionaryContainsKeyMethod = null;
        private static readonly MethodInfo DictionaryGetItemMethod = null;

        private static readonly object SyncLock = new object();

        private static bool? _CanClone = null;
        private static bool? _CanShallowClone = null;

        private static Func<T, T, CloneContext, T> _Strategy = null;
        private static Func<T, T, CloneContext, T> _DynamicStrategy = null;

        private static readonly Func<FieldInfo, bool> FieldIncludeFilter = f =>
            (f.IsDefined(typeof(NonSerializedAttribute), true) != true);

        static Cloneable()
        {
            TargetType = typeof(T);
            CloneContextGetShouldUseExistingObjectsMethod = GetMethod(typeof(CloneContext), "get_ShouldUseExistingObjects");
            CloneContextGetIsShallowCloneMethod = GetMethod(typeof(CloneContext), "get_IsShallowClone");
            CloneContextGetCloneMapMethod = GetMethod(typeof (CloneContext), "get_CloneMap");

            Type dictionaryType = typeof(IDictionary<,>).MakeGenericType(typeof(object), typeof(object));

            DictionaryContainsKeyMethod = GetMethod(dictionaryType, "ContainsKey", typeof(object));
            DictionaryGetItemMethod = GetMethod(dictionaryType, "get_Item", typeof(object));
        }

        private class ReferenceTypeStrategy<TConcrete>
        {
            private Func<T, T, CloneContext, T> _NextStrategy = null;
            private Func<TConcrete, TConcrete, CloneContext, TConcrete> _ReferenceTypeDynamicStrategy = null;

            public T Strategy(T source, T destination, CloneContext cloneContext)
            {
                Type sourceType = source.GetType();

                if (sourceType == typeof(TConcrete))
                {
                    var s = (TConcrete)(object)source;
                    var d = destination is TConcrete ? (TConcrete) (object) destination : default(TConcrete);

                    if (_ReferenceTypeDynamicStrategy == null)
                        _ReferenceTypeDynamicStrategy = Cloneable<TConcrete>.BuildDynamicStrategy();

                    return (T)(object)_ReferenceTypeDynamicStrategy(s, d, cloneContext);
                }

                if(_NextStrategy == null)
                {
                    var interfaceStrategyType = typeof(ReferenceTypeStrategy<>).MakeGenericType(TargetType, sourceType);
                    var interfaceStrategy = Activator.CreateInstance(interfaceStrategyType);

                    _NextStrategy = interfaceStrategy.GetDelegate<Func<T, T, CloneContext, T>>("Strategy");
                }

                return _NextStrategy(source, destination, cloneContext);
            }
        }

        #region Build Methods

        private static Func<T, T, CloneContext, T> BuildStrategy()
        {
            bool canShallowClone = CanShallowClone();
            bool canClone = CanClone();

            bool isSealedType = TargetType.IsSealed;
            bool isTargetTypeArray = TargetType.IsArray;
            bool isReferenceType = !TargetType.IsValueType;
            bool isNullableType = TargetType.IsGenericType &&
                                  TargetType.GetGenericTypeDefinition() == typeof (Nullable<>);

            bool canReturnSourceAsClone = IsRootTypeCloneablePrimitive(TargetType) && isTargetTypeArray != true;

            Func<T, T, CloneContext, T> completionStrategy = null;
            Func<Array, Array, CloneContext, T> arrayCloneStrategy = null;

            return (s, d, c) =>
            {
                if (canShallowClone != true || (c.IsShallowClone != true && canClone != true))
                    throw new InvalidOperationException("This type cannot be cloned.");

                if (canReturnSourceAsClone)
                    return s;

                if (s == null)
                    return default(T);

                var actualType = s.GetType();

                if (isReferenceType && c.CloneMap.ContainsKey(s))
                    return (T) c.CloneMap[s];

                if (actualType.IsArray)
                {
                    Array destArray = (Array) (object) d;
                    Array sourceArray = (Array) (object) s;

                    if (destArray == null || destArray.LongLength != sourceArray.LongLength)
                    {
                        d = (T) sourceArray.Clone();
                        destArray = (Array)(object)d;
                    }
                    else if (c.IsShallowClone && sourceArray.LongLength > 0)
                        Array.Copy(sourceArray, destArray, sourceArray.LongLength);

                    if (c.IsShallowClone || sourceArray.LongLength <= 0)
                    {
                        c.CloneMap.Add(s, d);
                        return d;
                    }

                    if(arrayCloneStrategy == null)
                    {
                        var arrayCloneMethod = GetMethod(typeof(Cloneable<T>),
                                                         "ArrayClone",
                                                         typeof (Array),
                                                         typeof (Array),
                                                         typeof (CloneContext));

                        arrayCloneMethod = arrayCloneMethod.MakeGenericMethod(actualType.GetElementType());
                        arrayCloneStrategy = arrayCloneMethod.ToDelegate<Func<Array, Array, CloneContext, T>>();
                    }

                    d = arrayCloneStrategy(sourceArray, destArray, c);
                    c.CloneMap.Add(s, d);

                    return d;
                }

                if(d == null && (isReferenceType || isNullableType))
                    d = (T)FormatterServices.GetSafeUninitializedObject(s.GetType());

                if (completionStrategy == null)
                {
                    if (isReferenceType && isSealedType != true)
                    {
                        var referenceTypeCloneableType = typeof(ReferenceTypeStrategy<>).MakeGenericType(TargetType, s.GetType());
                        var referenceTypeCloneable = Activator.CreateInstance(referenceTypeCloneableType);

                        completionStrategy = referenceTypeCloneable.GetDelegate<Func<T, T, CloneContext, T>>("Strategy");
                    }
                    else
                        completionStrategy = BuildDynamicStrategy();
                }

                if(isReferenceType)
                    c.CloneMap.Add(s, d);

                return completionStrategy(s, d, c);
            };
        }

        private static Func<T, T, CloneContext, T> BuildDynamicStrategy()
        {
            if (_DynamicStrategy == null)
            {
                string dynamicMethodName = string.Format("Cloneable<{0}>_CloneDynamicStrategy", TargetType.Name);
                var dynamicStrategyMethod = new DynamicMethod(dynamicMethodName,
                                                              TargetType,
                                                              new[]
                                                              {
                                                                  TargetType,
                                                                  TargetType,
                                                                  typeof (CloneContext)
                                                              },
                                                              TargetType,
                                                              true);


                var il = dynamicStrategyMethod.GetILGenerator();

                il.DeclareLocal(TargetType);

                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Stloc_0);

                foreach (FieldInfo field in TargetType.GetFieldsDeeply(FieldIncludeFilter))
                {
                    if (field.IsDefined(typeof (CloneReferenceOnlyAttribute), true) ||
                        field.FieldType.IsDefined(typeof (CloneReferenceOnlyAttribute), true) ||
                        IsTypeCloneablePrimative(field.FieldType))
                    {
                        EmitCopyField(il, field);
                    }
                    else
                    {
                        EmitCloneFieldWithShallowCheck(il, field);
                    }
                }

                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ret);

                _DynamicStrategy = dynamicStrategyMethod.ToFunc<T, T, CloneContext, T>();
            }

            return _DynamicStrategy;
        }

        #endregion

        #region Emit Methods

        private static void EmitCopyField(ILGenerator gen, FieldInfo field)
        {
            var copyFieldLabel = gen.DefineLabel();
            var storeFieldLabel = gen.DefineLabel();

            // handles self-referencing fields
            if (field.FieldType.IsValueType != true && field.FieldType.IsAssignableFrom(TargetType))
            {
                // check clone map
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldfld, field);
                gen.Emit(OpCodes.Brfalse_S, copyFieldLabel);

                gen.Emit(OpCodes.Ldarg_2);
                gen.Emit(OpCodes.Call, CloneContextGetCloneMapMethod);

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldfld, field);

                gen.Emit(OpCodes.Callvirt, DictionaryContainsKeyMethod);
                gen.Emit(OpCodes.Brfalse_S, copyFieldLabel);

                gen.Emit(OpCodes.Ldarg_2);
                gen.Emit(OpCodes.Call, CloneContextGetCloneMapMethod);

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldfld, field);

                gen.Emit(OpCodes.Call, DictionaryGetItemMethod);

                gen.Emit(OpCodes.Ldloc_0);
                gen.Emit(OpCodes.Br_S, storeFieldLabel);
            }

            // copy field
            gen.MarkLabel(copyFieldLabel);
            if (field.DeclaringType.IsValueType)
            {
                gen.Emit(OpCodes.Ldloca_S, (byte)0);
                gen.Emit(OpCodes.Ldarga_S, (byte)0);
            }
            else
            {
                gen.Emit(OpCodes.Ldloc_0);
                gen.Emit(OpCodes.Ldarg_0);
            }
            
            gen.Emit(OpCodes.Ldfld, field);

            gen.MarkLabel(storeFieldLabel);
            gen.Emit(OpCodes.Stfld, field);
        }

        private static void EmitCloneFieldWithShallowCheck(ILGenerator gen, FieldInfo field)
        {
            var ifNotShallowCloneLabel = gen.DefineLabel();
            var afterDestinationCopied = gen.DefineLabel();

            gen.Emit(OpCodes.Ldarg_2);
            gen.Emit(OpCodes.Call, CloneContextGetIsShallowCloneMethod);
            gen.Emit(OpCodes.Brfalse_S, ifNotShallowCloneLabel);

            EmitCopyField(gen, field);
            gen.Emit(OpCodes.Br_S, afterDestinationCopied);

            gen.MarkLabel(ifNotShallowCloneLabel);

            // used by final store field (Stfld) instruction
            if (field.DeclaringType.IsValueType)
                gen.Emit(OpCodes.Ldloca_S, (byte)0);
            else
                gen.Emit(OpCodes.Ldloc_0);

            EmitCloneField(gen, field);
            gen.Emit(OpCodes.Stfld, field);

            gen.MarkLabel(afterDestinationCopied);
        }

        private static void EmitCloneField(ILGenerator gen, FieldInfo field)
        {
            Type fieldClonableType = typeof(Cloneable<>).MakeGenericType(field.FieldType);
            MethodInfo getStrategyMethod = GetMethod(fieldClonableType, "get_Strategy");
            
            Type strategyMethodType = typeof(Func<,,,>).MakeGenericType(field.FieldType, field.FieldType, typeof(CloneContext), field.FieldType);
            MethodInfo getFuncMethod = GetMethod(strategyMethodType, "Invoke", field.FieldType, field.FieldType, typeof(CloneContext));

            gen.Emit(OpCodes.Call, getStrategyMethod);
            
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldfld, field);

            EmitLoadDestination(gen, field);

            gen.Emit(OpCodes.Ldarg_2);

            gen.Emit(OpCodes.Callvirt, getFuncMethod);
        }

        private static void EmitLoadDestination(ILGenerator gen, FieldInfo field)
        {
            if(field.FieldType.IsValueType)
            {
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldfld, field);
            }
            else
            {
                var ifShouldUseExistingLabel = gen.DefineLabel();
                var afterLoadingDestinationLabel = gen.DefineLabel();

                gen.Emit(OpCodes.Ldarg_2);
                gen.Emit(OpCodes.Call, CloneContextGetShouldUseExistingObjectsMethod);
                gen.Emit(OpCodes.Brtrue_S, ifShouldUseExistingLabel);

                gen.Emit(OpCodes.Ldnull);
                gen.Emit(OpCodes.Br_S, afterLoadingDestinationLabel);

                gen.MarkLabel(ifShouldUseExistingLabel);
                gen.Emit(OpCodes.Ldloc_0);
                gen.Emit(OpCodes.Ldfld, field);

                gen.MarkLabel(afterLoadingDestinationLabel);
            }
        }

        #endregion

        #region Helper Methods

        private static bool IsNotCloneable(Type inputType)
        {
            if (typeof(Delegate).IsAssignableFrom(inputType))
                return true;

            return false;
        }

        private static bool IsRootTypeCloneablePrimitive(Type inputType)
        {
            Type rootType = GetRootType(inputType);
            return IsTypeCloneablePrimative(rootType);
        }

        private static bool IsTypeCloneablePrimative(Type inputType)
        {
            if (Array.Exists(CloneablePrimitives, x => x == inputType))
                return true;

            if (inputType.IsPrimitive)
                return true;

            return false;
        }

        private static Type GetRootType(Type type)
        {
            while (type.IsArray)
                type = type.GetElementType();

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return type.GetGenericArguments()[0];

            return type;
        }

        private static MethodInfo GetMethod(Type type, string methodName, params Type[] argumentTypes)
        {
            const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            return type.GetMethod
            (
                methodName,
                bindingFlags,
                null,
                argumentTypes,
                null
            );
        }

        #endregion

        #region Can Methods

        private static bool CanClone(Type type, bool isShallow)
        {
            Type typeToCheck = GetRootType(type);

            if (typeToCheck.IsInterface)
                return true;

            if (IsNotCloneable(typeToCheck))
                return false;

            if (IsRootTypeCloneablePrimitive(typeToCheck))
                return true;

            Func<FieldInfo, bool> includeFilter = f => FieldIncludeFilter(f) && f.FieldType != typeToCheck;

            foreach (FieldInfo field in typeToCheck.GetFieldsDeeply(includeFilter))
            {
                Type fieldType = GetRootType(field.FieldType);

                if (IsNotCloneable(fieldType))
                    return false;

                if (IsRootTypeCloneablePrimitive(fieldType))
                    continue;

                if (fieldType.IsInterface)
                         continue;

                if (isShallow != true)
                {
                    Type fieldClonableType = typeof(Cloneable<>).MakeGenericType(fieldType);
                    MethodInfo canCloneMethod = GetMethod(fieldClonableType, "CanClone");
                    var canClone = canCloneMethod.ToDelegate<Func<bool>>();

                    if (canClone() != true)
                        return false;
                }
            }

            return true;
        }

        public static bool CanClone()
        {
            if (_CanClone.HasValue != true)
                _CanClone = CanClone(TargetType, false);

            return _CanClone.Value;
        }

        public static bool CanShallowClone()
        {
            if (_CanShallowClone.HasValue != true)
                _CanShallowClone = CanClone(TargetType, true);

            return _CanShallowClone.Value;
        }

        #endregion

        #region Clone Methods

        public static T Clone(T source)
        {
            var context = new CloneContext(false, false);
            return Strategy(source, default(T), context);
        }

        public static T ShallowClone(T source)
        {
            var context = new CloneContext(true, false);
            return Strategy(source, default(T), context);
        }

        public static T CloneTo(T source, T destination)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(destination, "destination");

            if (TargetType.IsValueType != true && ReferenceEquals(source, destination))
                throw new InvalidOperationException("The destination object cannot be the same as the source.");

            var sourceArray = source as Array;
            var destArray = destination as Array;

            if(sourceArray != null && destArray != null && destArray.LongLength != sourceArray.LongLength)
                throw new InvalidOperationException("The destination array must be the same size (length) as the source array.");

            var context = new CloneContext(false, true);
            return Strategy(source, destination, context);
        }

        public static T ShallowCloneTo(T source, T destination)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(destination, "destination");

            if(TargetType.IsValueType != true && ReferenceEquals(source, destination))
                throw new InvalidOperationException("The destination object cannot be the same as the source.");

            var sourceArray = source as Array;
            var destArray = destination as Array;

            if (sourceArray != null && destArray != null && destArray.LongLength != sourceArray.LongLength)
                throw new InvalidOperationException("The destination array must be the same size (length) as the source array.");

            var context = new CloneContext(true, true);
            return Strategy(source, destination, context);
        }

        private static T ArrayClone<TItem>(Array sourceArray, Array destArray, CloneContext context)
        {
            var i = new ArrayIndex(sourceArray);
            while(true)
            {
                TItem sourceValue = (TItem)sourceArray.GetValue(i);
                TItem currentValue = context.ShouldUseExistingObjects ? (TItem)destArray.GetValue(i) : default(TItem);

                TItem value = Cloneable<TItem>.Strategy(sourceValue, currentValue, context);
                destArray.SetValue(value, i);

                if(i.CanIncrement())
                    i.Increment();
                else
                    break;
            }

            return (T)(object)destArray;
        }


        #endregion

        private static Func<T, T, CloneContext, T> Strategy
        {
            get
            {
                if (_Strategy == null)
                {
                    lock (SyncLock)
                    {
                        if (_Strategy == null)
                            _Strategy = BuildStrategy();
                    }
                }

                return _Strategy;
            }
        }
    }
}
