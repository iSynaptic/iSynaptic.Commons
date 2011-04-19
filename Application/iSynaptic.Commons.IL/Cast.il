.namespace iSynaptic.Commons.IL
{
	.class public abstract auto ansi sealed Cast<TSource, TDestination>
		extends [mscorlib]System.Object
	{
		.method private hidebysig specialname rtspecialname static void .cctor() cil managed
		{
			.maxstack 3

			ldtoken !TDestination
			call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
			ldtoken !TSource
			call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
			callvirt instance bool [mscorlib]System.Type::IsAssignableFrom(class [mscorlib]System.Type)

			brtrue.s LRET

			ldstr "Object of type {0} cannot be casted to {1}."
			ldtoken !TSource
			call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
			callvirt instance string [mscorlib]System.Reflection.MemberInfo::get_Name()
			ldtoken !TDestination
			call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
			callvirt instance string [mscorlib]System.Reflection.MemberInfo::get_Name()
			call string [mscorlib]System.String::Format(string, object, object)
			newobj instance void [mscorlib]System.InvalidOperationException::.ctor(string)
			throw 

			LRET: ret 
		}

		.method public hidebysig static !TDestination With(!TSource source) cil managed
		{
			.maxstack 1
			ldarg.0 
			ret 
		}
	}
}
