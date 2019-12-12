using System;
using System.Collections.Generic;
using System.Text;

namespace DIContainerUnitTests.ClassesForTests
{
    public interface EnumInterface
    {
        IEnumerable<IRepository> dEnum { get; }
    }
}
