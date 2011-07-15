namespace iSynaptic.Commons
{
    public static class UnsafeCast<TSource, TDestination>
    {
        public static TDestination With(TSource source)
        {
            return IL.UnsafeCast<TSource, TDestination>.With(source);
        }
    }
}
