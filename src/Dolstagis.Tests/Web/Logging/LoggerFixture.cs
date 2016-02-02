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
            var provider = new Mock<ILoggingProvider>();
            provider.Setup(x => x.CreateLogger(It.IsAny<Type>())).Returns<Logger>(null);
            var tmp = Logger.Provider;
            Logger.Provider = provider.Object;
            var log = Logger.ForThisClass();
            Logger.Provider = tmp;
            provider.Verify(x => x.CreateLogger(this.GetType()), Times.Once);
        }
    }
}
