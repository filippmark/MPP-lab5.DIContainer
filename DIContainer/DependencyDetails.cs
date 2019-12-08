using System;

namespace DIContainer
{
    public class DependencyDetails
    {
        public bool IsSingleton { get; }
        public object Instance { get; set; }

        public Type ImplType;

        public DependencyDetails(bool isSingleton, Type implType)
        {
            IsSingleton = isSingleton;
            Instance = null;
            ImplType = implType;
        }
    }
}
