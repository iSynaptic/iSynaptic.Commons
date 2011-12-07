namespace iSynaptic.Commons.Linq
{
    public struct Neighbors<T>
    {
        public Neighbors(T value, Maybe<T> previous, Maybe<T> next)
            : this()
        {
            Value = value;
            Previous = previous;
            Next = next;
        }

        public T Value { get; private set; }

        public Maybe<T> Previous { get; private set; }
        public Maybe<T> Next { get; private set; }
    }
}