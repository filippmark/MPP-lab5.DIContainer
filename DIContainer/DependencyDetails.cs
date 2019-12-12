using System;

namespace DIContainer
{
    public class DependencyDetails
    {
        public bool IsSingleton { get; }
        public object Instance { get; set; }

        public Type ImplType;

        public bool IsGeneric { get; }

        public Type NestedType { get; set; }


        public DependencyDetails(bool isSingleton, Type implType, bool isGeneric, Type nestedType)
        {
            IsSingleton = isSingleton;
            Instance = null;
            ImplType = implType;
            IsGeneric = isGeneric;
            NestedType = nestedType;
        }
    }
}
