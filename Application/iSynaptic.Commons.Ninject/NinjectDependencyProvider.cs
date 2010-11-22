using System;
using Ninject;

namespace iSynaptic.Commons.Ninject
{
    public class NinjectDependencyProvider : IDependencyResolver
    {
        public NinjectDependencyProvider(IKernel kernel)
        {
            if (kernel == null)
                throw new ArgumentNullException("kernel");

            Kernel = kernel;
        }

        public object Resolve(string name, Type dependencyType, Type requestingType)
        {
            return Kernel.Get(dependencyType, name);
        }

        private IKernel Kernel { get; set; }
    }
}
