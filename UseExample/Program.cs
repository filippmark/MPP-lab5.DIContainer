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
            /*var t1 = typeof(IServiceGen<>);

            configuration.Register<IServiceGen<IRepository>, ServiceGenImpl<IRepository>>(false);
            configuration.Register<IRepository, RepositoryImpl>(false);

            var provider = new DependencyProvider(configuration);
            var obj = provider.Resolve<IServiceGen<IRepository>>();
            Console.WriteLine(null == obj);*/

            var t1 = typeof(IServiceGen<IRepository>);
            var t2 = typeof(IServiceGen<>);

            Console.WriteLine(t1.GetGenericTypeDefinition().GetGenericArguments()[0]);
            Console.WriteLine(t2.GetGenericTypeDefinition().GetGenericArguments()[0]);
            Console.WriteLine(t2.GetGenericArguments().Length);
            configuration.Register(typeof(IServiceGen<>), typeof(ServiceGenImpl<>), false);
            configuration.Register<IRepository, RepositoryImpl>(false);


            var provider = new DependencyProvider(configuration);
            var obj = provider.Resolve<IServiceGen<IRepository>>();



            Console.WriteLine(obj == null);
        }
    }
}