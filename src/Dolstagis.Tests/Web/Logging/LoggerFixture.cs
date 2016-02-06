using System;
using Dolstagis.Web.Logging;
using Moq;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Logging
{
    [TestFixture]
    public class LoggerFixture
    {
        [Test]
        public void CanCreateLogger()
        {
            var factory = new Mock<ILoggerFactory>();
            factory.Setup(x => x.CreateLogger(It.IsAny<Type>())).Returns<Logger>(null);
            var tmp = Logger.Factory;
            Logger.Factory = factory.Object;
            var log = Logger.ForThisClass();
            Logger.Factory = tmp;
            factory.Verify(x => x.CreateLogger(this.GetType()), Times.Once);
        }
    }
}
