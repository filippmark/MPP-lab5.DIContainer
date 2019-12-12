using DIContainer;
using DIContainerUnitTests.ClassesForTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DIContainerUnitTests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void ShouldGenerateWithRecursion()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<IService, ServiceImpl>(false);
            configuration.Register<IRepository, RepositoryImpl>(false);

            var provider = new DependencyProvider(configuration);
            ServiceImpl obj = (ServiceImpl) provider.Resolve<IService>();
            Assert.IsNotNull(obj.rep);
        }

        [TestMethod]
        public void ShouldGenerateWithOpenGeneric()
        {
            var configuration = new DependenciesConfiguration();
          
            configuration.Register(typeof(IServiceGen<>), typeof(ServiceGenImpl<>), false);
            configuration.Register<IRepository, RepositoryImpl>(false);

            var provider = new DependencyProvider(configuration);
            var obj = provider.Resolve<IServiceGen<IRepository>>();
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void ShouldGenerateWithIEnumerable()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<IRepository, RepositoryImpl>(false);

            configuration.Register<EnumInterface, ClassWithEnum>(false);

            var provider = new DependencyProvider(configuration);
            ClassWithEnum obj = (ClassWithEnum) provider.Resolve<EnumInterface>();
            Assert.IsNotNull(obj.dEnum);
            Assert.AreEqual(1, obj.dEnum.Count());
        }

    }
}
