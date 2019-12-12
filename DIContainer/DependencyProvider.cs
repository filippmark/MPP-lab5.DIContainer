using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DIContainer
{
    public class DependencyProvider
    {
        private DependenciesConfiguration _configuration;
        private object _locker;
        private List<Type> _typesInGeneration;

        public DependencyProvider(DependenciesConfiguration configuration)
        {
            _configuration = configuration;
            _locker = new object();
            _typesInGeneration = new List<Type>();
        }

        public object Resolve<T>()
        {
            return CreateInstance(typeof(T));
        }

        private object CreateInstance(Type type)
        {
            object result = null;
            if (!_typesInGeneration.Contains(type))
            {
                _typesInGeneration.Add(type);
                if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                {
                    result = CreateIEnumerable(type);
                    _typesInGeneration.Remove(type);
                }
                else
                {
                    if (type.IsGenericType && (type.GenericTypeArguments.Length > 0))
                    {
                        Type nestedType = type.GenericTypeArguments[0];
                        DependenciesImpls impls = null;
                        DependenciesImpls implsOfNestedType = null;
                        var typeOpenGeneric = type.GetGenericTypeDefinition();
                        DependencyDetails generic = null;
                        DependencyDetails nested = null;
                        if ((_configuration.Dependencies.TryGetValue(type, out impls) || (_configuration.Dependencies.TryGetValue(type.GetGenericTypeDefinition(), out impls)))
                            && _configuration.Dependencies.TryGetValue(nestedType, out implsOfNestedType))
                        {
                            generic = impls.ImplTypes[impls.ImplTypes.Count - 1];
                            nested = implsOfNestedType.ImplTypes[implsOfNestedType.ImplTypes.Count - 1];
                            result = GenerateGenericDependency(generic, nestedType);
                            _typesInGeneration.Remove(type);
                        }
                    }
                    else
                    {
                        if (_configuration.Dependencies.TryGetValue(type, out DependenciesImpls details))
                        {
                            var dependency = details.ImplTypes[details.ImplTypes.Count - 1];
                            result = GenerateDependency(dependency);
                            _typesInGeneration.Remove(type);
                        }
                    }
                }
            }
            return result;
        }

        private object GenerateGenericDependency(DependencyDetails dependency, Type nestedType)
        {
            if (dependency.IsSingleton)
            {
                lock (_locker)
                {

                    if ((dependency.Instance != null) && (dependency.NestedType == nestedType))
                    {
                        return dependency.Instance;
                    }
                    else if(dependency.NestedType != nestedType)
                    {
                        dependency.NestedType = nestedType;
                    }
                    dependency.Instance = GenerateDependencyByConstructor(MakeGenericTypeWithNestedType(dependency.ImplType, nestedType));
                    return dependency.Instance;
                }
            }
            else
            {
                return GenerateDependencyByConstructor(MakeGenericTypeWithNestedType(dependency.ImplType, nestedType));
            }
        }

        private Type MakeGenericTypeWithNestedType(Type genericType, Type nestedType)
        {
            return genericType.MakeGenericType(new Type[] { nestedType});
        }


        private object GenerateDependency(DependencyDetails dependency)
        {
            if (dependency.IsSingleton)
            {
                lock (_locker)
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
            }
            return GenerateDependencyByConstructor(dependency.ImplType);
        }

        private object GenerateDependencyByConstructor(Type type)
        {
            ConstructorInfo[] constructorInfos = type.GetConstructors();
            ConstructorInfo constructor = constructorInfos[0];
            ParameterInfo[] parameters = constructor.GetParameters();
            List<object> values = new List<object>();
            if (parameters.Length > 0)
            {
                foreach (var param in parameters)
                {
                    object parameter = null;
                    parameter = CreateInstance(param.ParameterType);
                    if (parameter == null)
                    {
                        return null;
                    }
                    values.Add(parameter);
                }
            }
            return constructor.Invoke(values.ToArray());

        }


        private object CreateIEnumerable(Type type)
        {
            if ((type.GetGenericArguments().Length > 0))
            {
                Type interfaceType = type.GetGenericArguments()[0];
                if(type.IsGenericTypeDefinition)
                {
                    Type nestedType = interfaceType.GetGenericArguments()[0];

                    DependenciesImpls impls = null;
                    DependenciesImpls implsOfNestedType = null;
                    if ((_configuration.Dependencies.TryGetValue(type, out impls) || (_configuration.Dependencies.TryGetValue(type.GetGenericTypeDefinition(), out impls)))
                        && _configuration.Dependencies.TryGetValue(nestedType, out implsOfNestedType))
                    {
                        var result =  Array.CreateInstance(type, impls.ImplTypes.Count);
                        var nestedImpl = implsOfNestedType.ImplTypes[implsOfNestedType.ImplTypes.Count - 1];
                        int index = 0;
                        foreach(var impl in impls.ImplTypes)
                        {
                            result.SetValue(GenerateGenericDependency(impl, nestedType), index);
                            index++;
                        }
                        return result;
                    }
                }
                else if (_configuration.Dependencies.TryGetValue(type.GetGenericArguments()[0], out DependenciesImpls impls))
                {
                    var result = Array.CreateInstance(type.GetGenericArguments()[0], impls.ImplTypes.Count);
                    
                    int index = 0;
                    foreach (var impl in impls.ImplTypes)
                    {
                        result.SetValue(GenerateDependency(impl), index);
                        index++;
                    }
                    return result;
                }
            }
            return null;
        }
    }
}