using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public delegate TRet Func<TRet>();
    public delegate TRet Func<TRet, A0>(A0 a0);
    public delegate TRet Func<TRet, A0, A1>(A0 a0, A1 a1);
    public delegate TRet Func<TRet, A0, A1, A2>(A0 a0, A1 a1, A2 a2);
    public delegate TRet Func<TRet, A0, A1, A2, A3>(A0 a0, A1 a1, A2 a2, A3 a3);
    public delegate TRet Func<TRet, A0, A1, A2, A3, A4>(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4);
    public delegate TRet Func<TRet, A0, A1, A2, A3, A4, A5>(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5);
    public delegate TRet Func<TRet, A0, A1, A2, A3, A4, A5, A6>(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6);
    public delegate TRet Func<TRet, A0, A1, A2, A3, A4, A5, A6, A7>(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7);
    public delegate TRet Func<TRet, A0, A1, A2, A3, A4, A5, A6, A7, A8>(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8);
}
