namespace iSynaptic.Commons.Collections.Generic
{
    public class LookAheadableValue<T>
    {
        private readonly T _Value = default(T);
        private readonly LookAheadEnumerator<T> _Enumerator = null;

        internal LookAheadableValue(T value, LookAheadEnumerator<T> enumerator)
        {
            _Value = value;
            _Enumerator = enumerator;
        }

        public T LookAhead(int index)
        {
            return _Enumerator.LookAhead(index);
        }

        public T Value
        {
            get { return _Value; }
        }
    }
}
