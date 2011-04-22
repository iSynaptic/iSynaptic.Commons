using System;

namespace iSynaptic.Commons
{
    public interface IDependencyResolver
    {
        Maybe<object> TryResolve(IDependencyDeclaration declaration);
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
            DependencyType = Guard.NotNull(dependencyType, "dependencyType");
            Name = name;
        }

        public Type DependencyType { get; private set; }
        public string Name { get; private set; }
    }
}
