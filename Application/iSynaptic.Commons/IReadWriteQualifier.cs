namespace iSynaptic.Commons
{
    public interface IReadWriteQualifier<TItem, TQualifier>
    {
        TItem this[TQualifier qualifier] { get; set; }
    }
}
