using Dolstagis.Web;
using Dolstagis.Web.Lifecycle.ResultProcessors;
using Dolstagis.Web.Testing;
using Moq;
using NUnit.Framework;

using lc = Dolstagis.Web.Lifecycle;

namespace Dolstagis.Tests.Web.Lifecycle.ResultProcessors
{
    [TestFixture]
    public class TextResultProcessorFixture
    {
        [TestCase(lc.Match.Exact, 0.9, "application/json,text/plain;q=0.9,application/xhtml+xml;q=0.8")]
        [TestCase(lc.Match.Inexact, 0.5, "text/html;q=0.9,application/xhtml+xml;q=0.8,*/*;q=0.5")]
        [TestCase(lc.Match.None, 0, "text/html;q=0.9,application/xhtml+xml;q=0.8")]
        public void AcceptHeadersReturnCorrectMatches
            (lc.Match expectedMatch, double expectedQ, string acceptHeader)
        {
            var context = new Mock<IRequestContext>();
            var request = Request.Get("http://localhost/")
                .Header("Accept", acceptHeader);
            context.Setup(x => x.Request).Returns(request);

            var result = new TextResultProcessor().Match(null, context.Object);

            Assert.AreEqual(expectedMatch, result.Match);
            Assert.AreEqual(expectedQ, result.Q, 0.001);
        }
    }
}
