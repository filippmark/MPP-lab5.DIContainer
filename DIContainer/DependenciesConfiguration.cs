using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DIContainer
{
    public class DependenciesConfiguration
    {
        public ConcurrentDictionary<Type, DependenciesImpls> Dependencies { get; }
        
        public DependenciesConfiguration()
        {
            Dependencies = new ConcurrentDictionary<Type, DependenciesImpls>();
        }

        public bool Register<InterfaceType, ImplType>(bool isSingleton)
        {
            return TryToAddToConfig(typeof(InterfaceType), typeof(ImplType), isSingleton);

        }

        public bool Register(Type interfaceType, Type implType, bool isSingleton)
        {
            return TryToAddToConfig(interfaceType, implType, isSingleton);
        }


        private bool TryToAddToConfig(Type typeInterface, Type typeImpl, bool isSingleton)
        {
            if ((!typeImpl.IsInterface && !typeImpl.IsAbstract))
            //    && ((typeInterface.IsInterface && typeInterface.IsAssignableFrom(typeImpl)) || typeInterface.IsAssignableFrom(typeImpl)))
            {
                if (typeInterface.IsGenericType)
                    typeInterface = typeInterface.GetGenericTypeDefinition();

                if (!Dependencies.ContainsKey(typeInterface))
                {
                    var dependencies = new DependenciesImpls(isSingleton, typeImpl);
                    Dependencies.TryAdd(typeInterface, dependencies);
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
