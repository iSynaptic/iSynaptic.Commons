using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace iSynaptic.Commons.Threading
{
    public struct SpinLock
    {
        [DllImport("kernel32", ExactSpelling = true)]
        private static extern void SwitchToThread();

        private static readonly Boolean IsSingleCpuMachine = (Environment.ProcessorCount == 1);

        private const Int32 FREE = 0;
        private const Int32 OWNED = 1;
        private Int32 _LockState;

        public void Enter()
        {
            Thread.BeginCriticalRegion();
            while (true)
            {
                if (Interlocked.Exchange(ref _LockState, OWNED) == FREE)
                    return;

                while (Thread.VolatileRead(ref _LockState) == OWNED)
                {
                    StallThread();
                }
            }
        }

        public void Exit()
        {
            Interlocked.Exchange(ref _LockState, FREE);
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
