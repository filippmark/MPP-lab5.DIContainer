using System;

namespace DIContainer
{
    public class DependencyDetails
    {
        public bool IsSingleton { get; }
        public object Instance { get; set; }

        public Type ImplType;

        public bool IsGeneric { get; }


        public DependencyDetails(bool isSingleton, Type implType, bool isGeneric)
        {
            IsSingleton = isSingleton;
            Instance = null;
            ImplType = implType;
            IsGeneric = isGeneric;
        }
    }
}
