using System;
using System.Collections.Generic;
using System.Reflection;

namespace DIContainer
{
    public class DependencyProvider
    {
        private DependenciesConfiguration _configuration;

        public DependencyProvider(DependenciesConfiguration configuration)
        {
            _configuration = configuration;
        }

        public object Resolve<T>()
        {
            return CreateInstance(typeof(T));
        }

        private object CreateInstance(Type type)
        {
            if (_configuration.Dependencies.TryGetValue(type, out DependenciesImpls details))
            {
                var dependency = details.ImplTypes[details.ImplTypes.Count - 1];
                if (dependency.IsSingleton)
                {
                    if (dependency.Instance != null)
                    {
                        return dependency.Instance;
                    }
                    else
                    {
                        dependency.Instance = GenerateDependecyByConstructor(type);
                        return dependency.Instance;
                    }
                }
                return GenerateDependecyByConstructor(type);
            }
            return null;
        }

        private object GenerateDependecyByConstructor(Type type)
        {
            ConstructorInfo[] constructorInfos = type.GetConstructors();
            Console.WriteLine(constructorInfos.Length);
            if (constructorInfos.Length == 0)
            {
                return null;
            }
            else
            {
                ConstructorInfo constructor = constructorInfos[0];
                ParameterInfo[] parameters = constructor.GetParameters();
                List<object> values = new List<object>();
                if (parameters.Length > 0)
                {
                    foreach (var param in parameters)
                    {
                        var parameter = CreateInstance(param.ParameterType);
                        if (parameter == null)
                        {
                            return null;
                        }
                        values.Add(parameter);
                    }
                }
                return constructor.Invoke(values.ToArray());
            }
        }
    }
}