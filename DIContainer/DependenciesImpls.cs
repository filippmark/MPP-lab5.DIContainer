using System;
using System.Collections.Generic;

namespace DIContainer
{
    public class DependenciesImpls
    {
        public List<DependencyDetails> ImplTypes { get; }

        public DependenciesImpls(bool isSingleton, Type implType)
        {
            ImplTypes = new List<DependencyDetails>();
            AddTypeToList(isSingleton, implType);
        }

        public void AddTypeToList(bool isSingleton, Type implType)
        {
            var dependency = new DependencyDetails(isSingleton, implType, implType.IsGenericType);
            ImplTypes.Add(dependency);
        }
    }
}
