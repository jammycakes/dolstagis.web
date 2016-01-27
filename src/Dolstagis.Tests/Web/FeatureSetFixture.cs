using System;
using System.Linq;
using System.Threading.Tasks;
using Dolstagis.Tests.Web.TestFeatures;
using Dolstagis.Web;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;
using Moq;
using NUnit.Framework;

namespace Dolstagis.Tests.Web
{
    [TestFixture]
    public class FeatureSetFixture
    {
        private Feature alwaysEnabled = new EmptyNamedFeature("alwaysEnabled");
        private Feature alwaysDisabled = new EmptyNamedFeature("alwaysDisabled");
        private Feature localhostOnly = new EmptyNamedFeature("localhostOnly");
        private IFeatureSwitch alwaysEnabledSwitch;
        private IFeatureSwitch alwaysDisabledSwitch;
        private IFeatureSwitch localhostOnlySwitch;

        private IRequest localRequest;
        private IRequest nonLocalRequest;

        private FeatureSwitchboard switchboard;

        [OneTimeSetUp]
        public void CreateFeatures()
        {
            var mockAlwaysEnabled = new Mock<IFeatureSwitch>();
            mockAlwaysEnabled.Setup(x => x.IsEnabledForRequest(It.IsAny<IRequest>()))
                .Returns(Task.FromResult(true));
            alwaysEnabledSwitch = mockAlwaysEnabled.Object;

            var mockAlwaysDisabled = new Mock<IFeatureSwitch>();
            mockAlwaysDisabled.Setup(x => x.IsEnabledForRequest(It.IsAny<IRequest>()))
                .Returns(Task.FromResult(false));
            alwaysDisabledSwitch = mockAlwaysDisabled.Object;

            var mockLocalhostOnly = new Mock<IFeatureSwitch>();
            mockLocalhostOnly.Setup(x => x.IsEnabledForRequest(It.IsAny<IRequest>()))
                .Returns(Task.FromResult(false));
            mockLocalhostOnly.Setup(x => x.IsEnabledForRequest(It.Is<IRequest>(req => req.Url.IsLoopback)))
                .Returns(Task.FromResult(true));
            localhostOnlySwitch = mockLocalhostOnly.Object;

            var mockLocalRequest = new Mock<IRequest>();
            mockLocalRequest.SetupGet(x => x.Url).Returns(new Uri("http://localhost/"));
            localRequest = mockLocalRequest.Object;

            var mockNonLocalRequest = new Mock<IRequest>();
            mockNonLocalRequest.SetupGet(x => x.Url).Returns(new Uri("http://example.com"));
            nonLocalRequest = mockNonLocalRequest.Object;


            switchboard = new FeatureSwitchboard(null)
                .Add(this.alwaysEnabledSwitch, this.alwaysEnabled)
                .Add(this.alwaysDisabledSwitch, this.alwaysDisabled)
                .Add(this.localhostOnlySwitch, this.localhostOnly);
        }

        [Test]
        public void VerifyMocksHaveBeenSetUpCorrectly()
        {
            Assert.IsTrue(localRequest.Url.IsLoopback);
            Assert.IsFalse(nonLocalRequest.Url.IsLoopback);
        }

        [Test]
        public async Task LocalRequestShouldHaveTwoFeatures()
        {
            var localFeatureSet = await switchboard.GetFeatureSet(localRequest);
            Assert.AreEqual(2, localFeatureSet.Features.Count);
            Assert.AreSame(this.alwaysEnabled, localFeatureSet.Features.First(), "First feature is wrong");
            Assert.AreSame(this.localhostOnly, localFeatureSet.Features.Last(), "Second feature is wrong");
        }

        [Test]
        public async Task NonlocalRequestShouldHaveOneFeature()
        {
            var nonLocalFeatureSet = await switchboard.GetFeatureSet(nonLocalRequest);
            Assert.AreSame(this.alwaysEnabled, nonLocalFeatureSet.Features.Single());
        }

        [Test]
        public async Task DuplicateRequestsShouldReturnSameFeatureSet()
        {
            var set1 = await switchboard.GetFeatureSet(localRequest);
            var set2 = await switchboard.GetFeatureSet(localRequest);
            Assert.AreSame(set1, set2);
        }
    }
}
