using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public interface IResult<out T, out TObservation> : IResult, IMaybe<T>, IOutcome<TObservation>
    {
    }

    public interface IResult : IMaybe, IOutcome
    {
    }
}
