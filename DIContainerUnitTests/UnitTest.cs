using DIContainer;
using DIContainerUnitTests.ClassesForTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DIContainerUnitTests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<IService, ServiceImpl>(false);
            configuration.Register<IRepository, RepositoryImpl>(false);

            var provider = new DependencyProvider(configuration);
            var obj = provider.Resolve<IService>();
            Assert.IsNotNull(obj);
        }
    }
}
