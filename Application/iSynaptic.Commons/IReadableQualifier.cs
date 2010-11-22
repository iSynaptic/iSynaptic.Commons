namespace iSynaptic.Commons
{
    public interface IReadableQualifier<TItem, TQualifier>
    {
        TItem this[TQualifier qualifier] { get; }
    }
}
