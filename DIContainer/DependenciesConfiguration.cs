using System;
using System.Collections.Generic;

namespace DIContainer
{
    public class DependenciesConfiguration
    {
        public Dictionary<Type, DependenciesImpls> Dependencies { get; }

        public bool Register<InterfaceType, ImplType>(bool isSingleton)
        {
            Type typeInterface = typeof(InterfaceType);
            Type typeImpl = typeof(ImplType);

            if (((typeInterface.IsInterface) && (typeInterface.IsAssignableFrom(typeImpl))) || (typeInterface.IsAssignableFrom(typeImpl)))
            {
                if (!Dependencies.ContainsKey(typeInterface))
                {
                    var dependencies = new DependenciesImpls(isSingleton, typeImpl);
                    Dependencies.Add(typeInterface, dependencies);
                    return true;
                }
                else
                {
                    Dependencies.TryGetValue(typeInterface, out DependenciesImpls dependencies);
                    dependencies.AddTypeToList(isSingleton, typeImpl);

                }
            }
            return false;
        }
    }
}
