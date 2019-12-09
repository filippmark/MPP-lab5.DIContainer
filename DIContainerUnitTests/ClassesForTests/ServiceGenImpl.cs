namespace DIContainerUnitTests.ClassesForTests
{
    public class ServiceGenImpl<TRepository> : IServiceGen<TRepository>
    where TRepository : IRepository
    {
        public ServiceGenImpl(TRepository repository)
        {

        }

    }
}
