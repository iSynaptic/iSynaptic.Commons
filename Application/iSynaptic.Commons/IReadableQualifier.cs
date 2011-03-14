namespace iSynaptic.Commons
{
    public interface IReadableQualifier<in TQualifier, out TItem>
    {
        TItem this[TQualifier qualifier] { get; }
    }
}
