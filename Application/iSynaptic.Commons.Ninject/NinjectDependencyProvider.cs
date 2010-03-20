using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Ninject
{
    public class NinjectDependencyProvider : IDependencyResolver
    {
        public object Resolve(string name, Type dependencyType, Type requestingType)
        {
            return null;
        }
    }
}
