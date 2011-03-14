namespace iSynaptic.Commons
{
    public interface IReadWriteQualifier<in TQualifier, TItem>
    {
        TItem this[TQualifier qualifier] { get; set; }
    }
}
