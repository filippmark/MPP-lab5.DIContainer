using DIContainer;
using DIContainerUnitTests.ClassesForTests;
using System;

namespace UseExample
{
    public class Program
    {
        static void Main(string[] args)
        {
            var configuration = new DependenciesConfiguration();

            configuration.Register<EnumInterface, ClassWithEnum>(false);

            var provider = new DependencyProvider(configuration);
            ClassWithEnum obj = (ClassWithEnum)provider.Resolve<EnumInterface>();
        }
    }
}