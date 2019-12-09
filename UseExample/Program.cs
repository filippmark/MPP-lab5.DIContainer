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
            
            var t1 = typeof(IServiceGen<IRepository>);
            var t2 = typeof(IServiceGen<>);
            
            configuration.Register(typeof(IServiceGen<>), typeof(ServiceGenImpl<>), false);
            configuration.Register<IRepository, RepositoryImpl>(false);


            var provider = new DependencyProvider(configuration);
            var obj = provider.Resolve<IServiceGen<IRepository>>();



            Console.WriteLine(obj == null);
        }
    }
}