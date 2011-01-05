using System;

namespace iSynaptic.Commons
{
    public class ArrayIndex
    {
        private readonly Array _Target = null;

        public ArrayIndex(Array target)
        {
            Guard.NotNull(target, "target");

            Index = new int[target.Rank];
            _Target = target;
        }

        public void Increment()
        {
            int currentRank = _Target.Rank - 1;
            int currentRankIndex = Index[currentRank];

            while (currentRank >= 0)
            {
                int upperBound = _Target.GetUpperBound(currentRank);
                currentRankIndex = Index[currentRank];

                if (currentRankIndex >= upperBound)
                {
                    if(currentRank == 0)
                        throw new IndexOutOfRangeException();

                    Index[currentRank] = 0;
                    currentRank--;
                }
                else
                    break;
            }

            Index[currentRank] = currentRankIndex + 1;
        }

        public void Increment(int number)
        {
            var currentIndex = Index.Clone() as int[];

            try
            {
                int count = 0;
                while (count < number)
                {
                    Increment();
                    count++;
                }
            }
            catch
            {
                Index = currentIndex;
                throw;
            }
        }

        public void Reset()
        {
            Index = new int[_Target.Rank];
        }

        public static implicit operator int[](ArrayIndex index)
        {
            return index.Index;
        }

        public bool CanIncrement()
        {
            int currentRank = _Target.Rank - 1;
            while (currentRank >= 0)
            {
                int upperBound = _Target.GetUpperBound(currentRank);
                int currentRankIndex = Index[currentRank];

                if (currentRankIndex < upperBound)
                    return true;

                currentRank--;
            }

            return false;
        }

        public int[] Index { get; private set; }
    }
}
