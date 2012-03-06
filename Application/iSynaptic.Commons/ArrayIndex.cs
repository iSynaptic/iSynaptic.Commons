// The MIT License
// 
// Copyright (c) 2012 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

namespace iSynaptic.Commons
{
    public class ArrayIndex
    {
        private readonly Array _Target = null;

        public ArrayIndex(Array target)
        {
            _Target = Guard.NotNull(target, "target");
            Index = new int[_Target.Rank];
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
