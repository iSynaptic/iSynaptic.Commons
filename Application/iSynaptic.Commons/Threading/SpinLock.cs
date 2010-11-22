using System;
using System.Threading;
using System.Runtime.InteropServices;

namespace iSynaptic.Commons.Threading
{
    internal struct SpinLock
    {
        [DllImport("kernel32", ExactSpelling = true)]
        private static extern void SwitchToThread();

        private static readonly Boolean IsSingleCpuMachine = (Environment.ProcessorCount == 1);

        private const Int32 Free = 0;
        private const Int32 Owned = 1;
        private Int32 _LockState;

        public void Enter()
        {
            Thread.BeginCriticalRegion();
            while (true)
            {
                if (Interlocked.Exchange(ref _LockState, Owned) == Free)
                    return;

                while (Thread.VolatileRead(ref _LockState) == Owned)
                {
                    StallThread();
                }
            }
        }

        public void Exit()
        {
            Interlocked.Exchange(ref _LockState, Free);
            Thread.EndCriticalRegion();
        }

        private static void StallThread()
        {
            if (IsSingleCpuMachine)
                SwitchToThread();
            else
                Thread.SpinWait(1);
        }
    }
}
