using System;
using System.Collections.Concurrent;

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
            Console.WriteLine(CanBeGeneratedByConstructor(typeImpl));
            if (!typeImpl.IsInterface && !typeImpl.IsAbstract 
                && (typeInterface.IsClass || typeInterface.IsInterface) && CanBeGeneratedByConstructor(typeImpl)) 
            {
                Type nestedType = null;

                if (typeInterface.IsGenericType)
                {
                    if(typeImpl.GenericTypeArguments.Length > 0) 
                        nestedType = typeImpl.GenericTypeArguments[0];
                }
 
                if (!Dependencies.ContainsKey(typeInterface))
                {
                    var dependencies = new DependenciesImpls(isSingleton, typeImpl, nestedType);
                    Dependencies.TryAdd(typeInterface, dependencies);
                    return true;
                }
                else
                {
                    Dependencies.TryGetValue(typeInterface, out DependenciesImpls dependencies);
                    dependencies.AddTypeToList(isSingleton, typeImpl, nestedType);
                }
            }
            return false;
        }


        private bool CanBeGeneratedByConstructor(Type type)
        {
            return type.GetConstructors().Length != 0; 
        }
    }
}
