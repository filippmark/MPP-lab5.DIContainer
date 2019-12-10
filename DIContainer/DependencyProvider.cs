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
            if (type.IsGenericType)
                type = type.GetGenericTypeDefinition();

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
                        dependency.Instance = GenerateDependencyByConstructor(dependency.ImplType);
                        return dependency.Instance;
                    }
                }
                    return GenerateDependencyByConstructor(dependency.ImplType);
            }
            return null;
        }

        private object GenerateGenericDependency(DependencyDetails genDeps, Type nestedType)
        {
            var interfaces = nestedType.GetInterfaces();
            if ((interfaces.Length != 0) &&
                    (((!nestedType.IsGenericType)) && (_configuration.Dependencies.TryGetValue(interfaces[0], out DependenciesImpls nestedDep))))
            {
                Type implType = genDeps.ImplType;
                return GenerateDependencyByConstructor(implType);
            }
            return null;
        }

        private object GenerateDependencyByConstructor(Type type)
        {
            if (type.IsGenericType)
            {
                var args = GetArgForGenericType(type);
                if(args == null)
                {
                    return null;
                }
                type = type.MakeGenericType(args);
            }
            ConstructorInfo[] constructorInfos = type.GetConstructors();
            ConstructorInfo constructor = constructorInfos[0];
            ParameterInfo[] parameters = constructor.GetParameters();
            List<object> values = new List<object>();
            if (parameters.Length > 0)
            {
                foreach (var param in parameters)
                {
                    object parameter = null;
                    if (type.IsGenericType)
                    {
                        Type typeInterface = param.ParameterType.GetInterfaces()[0];
                        parameter = CreateInstance(typeInterface);
                        if (parameter == null)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        parameter = CreateInstance(param.ParameterType);
                        if (parameter == null)
                        {
                            return null;
                        }
                    }
                    values.Add(parameter);
                }
            }
            return constructor.Invoke(values.ToArray());

        }

        private Type[] GetArgForGenericType(Type genericType)
        {
            List<Type> types = new List<Type>();
            var genTypes = genericType.GetGenericArguments();
            foreach (var type in genTypes)
            {
                var interfaces = type.GetInterfaces();
                if ((interfaces.Length != 0) &&
                    (((!type.IsGenericType)) && (_configuration.Dependencies.TryGetValue(interfaces[0], out DependenciesImpls impls))))
                {
                    var dependency = impls.ImplTypes[impls.ImplTypes.Count - 1];
                    types.Add(dependency.ImplType);
                }
                else 
                {
                    return null;
                }
            }
            return types.ToArray();
        }

    }
}