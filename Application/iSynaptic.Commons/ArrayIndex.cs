using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public class ArrayIndex
    {
        private int[] _Index = null;
        private Array _Target = null;

        public ArrayIndex(Array target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            _Index = new int[target.Rank];
            _Target = target;
        }

        public void Increment()
        {
            int currentRank = _Target.Rank - 1;
            while (currentRank >= 0)
            {
                int upperBound = _Target.GetUpperBound(currentRank);
                int currentRankIndex = _Index[currentRank];

                if (currentRankIndex < upperBound)
                {
                    _Index[currentRank] = currentRankIndex + 1;
                    break;
                }
                else
                {
                    if(currentRank == 0)
                        throw new IndexOutOfRangeException();

                    _Index[currentRank] = 0;
                    currentRank--;
                }
            }
        }

        public void Increment(int number)
        {
            int[] currentIndex = _Index.Clone() as int[];

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
                _Index = currentIndex;
                throw;
            }
        }

        public void Reset()
        {
            _Index = new int[_Target.Rank];
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
                int currentRankIndex = _Index[currentRank];

                if (currentRankIndex < upperBound)
                    return true;
                else
                {
                    currentRank--;
                }
            }

            return false;
        }

        public int[] Index
        {
            get
            {
                return _Index;
            }
        }
    }
}
