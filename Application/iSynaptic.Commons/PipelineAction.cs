using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public delegate void PipelineAction<T>(ref T obj);
}
