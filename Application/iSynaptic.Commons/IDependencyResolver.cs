using System;

namespace iSynaptic.Commons
{
    public interface IDependencyResolver
    {
        object Resolve(IDependencyDeclaration declaration);
    }

    public interface IDependencyDeclaration
    {
        Type DependencyType { get; }
    }

    public interface INamedDependencyDeclaration : IDependencyDeclaration
    {
        string Name { get; }
    }

    public class DepencencyDeclaration : INamedDependencyDeclaration
    {

        public DepencencyDeclaration(Type dependencyType, string name)
        {
            Guard.NotNull(dependencyType, "dependencyType");

            DependencyType = dependencyType;
            Name = name;
        }

        public Type DependencyType { get; private set; }
        public string Name { get; private set; }
    }
}
