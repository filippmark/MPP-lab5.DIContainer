using System;
using System.Collections.Generic;
using System.Text;

namespace DIContainerUnitTests.ClassesForTests
{
    public class ClassWithEnum : EnumInterface
    {
        public IEnumerable<IRepository> dEnum { get; }

        public ClassWithEnum(IEnumerable<IRepository> d)
        {
            dEnum = d;
        }
    }
}
