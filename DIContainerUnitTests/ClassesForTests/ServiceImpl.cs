namespace DIContainerUnitTests.ClassesForTests
{
    public class ServiceImpl : IService
    {
        public IRepository rep { get; }
        public ServiceImpl(IRepository repository)
        {
            rep = repository;
        }
    }

}
