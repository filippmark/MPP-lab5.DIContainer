using System;
using System.Collections.Generic;

namespace DIContainer
{
    public class DependenciesImpls
    {
        public List<DependencyDetails> ImplTypes { get; }

        public DependenciesImpls(bool isSingleton, Type implType)
        {
            var dependency = new DependencyDetails(isSingleton, implType);
            ImplTypes = new List<DependencyDetails>();
            ImplTypes.Add(dependency);
        }

        public void AddTypeToList(bool isSingleton, Type implType)
        {
            var dependency = new DependencyDetails(isSingleton, implType);
            ImplTypes.Add(dependency);
        }

    }
}
