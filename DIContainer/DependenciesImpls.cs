using System;
using System.Collections.Generic;

namespace DIContainer
{
    public class DependenciesImpls
    {
        public List<DependencyDetails> ImplTypes { get; }

        public DependenciesImpls(bool isSingleton, Type implType, Type nestedType)
        {
            ImplTypes = new List<DependencyDetails>();
            AddTypeToList(isSingleton, implType, nestedType);
        }

        public void AddTypeToList(bool isSingleton, Type implType, Type nestedType)
        {
            var dependency = new DependencyDetails(isSingleton, implType, implType.IsGenericType, nestedType);
            ImplTypes.Add(dependency);
        }
    }
}
