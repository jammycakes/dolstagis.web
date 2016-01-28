using System;
using System.Linq;
using Dolstagis.Tests.Web.TestFeatures;
using Dolstagis.Web.Features;
using Dolstagis.Web.Features.Impl;
using Dolstagis.Web.Http;
using Moq;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Features
{
    [TestFixture]
    public class FeatureSetFixture
    {
        private IFeature alwaysEnabled = new EmptyNamedFeature("alwaysEnabled");
        private IFeature alwaysDisabled = new EmptyNamedFeature("alwaysDisabled", req => false);
        private IFeature localhostOnly = new EmptyNamedFeature("localhostOnly", req => req.Url.IsLoopback);

        private IRequest localRequest;
        private IRequest nonLocalRequest;

        private FeatureSwitchboard switchboard;

        [OneTimeSetUp]
        public void CreateFeatures()
        {
            var mockLocalRequest = new Mock<IRequest>();
            mockLocalRequest.SetupGet(x => x.Url).Returns(new Uri("http://localhost/"));
            localRequest = mockLocalRequest.Object;

            var mockNonLocalRequest = new Mock<IRequest>();
            mockNonLocalRequest.SetupGet(x => x.Url).Returns(new Uri("http://example.com"));
            nonLocalRequest = mockNonLocalRequest.Object;

            switchboard = new FeatureSwitchboard(null)
                .Add(alwaysEnabled, alwaysDisabled, localhostOnly);
        }

        [Test]
        public void VerifyMocksHaveBeenSetUpCorrectly()
        {
            Assert.IsTrue(localRequest.Url.IsLoopback);
            Assert.IsFalse(nonLocalRequest.Url.IsLoopback);
        }

        [Test]
        public void LocalRequestShouldHaveTwoFeatures()
        {
            var localFeatureSet = switchboard.GetFeatureSet(localRequest);
            Assert.AreEqual(2, localFeatureSet.Features.Count);
            Assert.AreSame(this.alwaysEnabled, localFeatureSet.Features.First(), "First feature is wrong");
            Assert.AreSame(this.localhostOnly, localFeatureSet.Features.Last(), "Second feature is wrong");
        }

        [Test]
        public void NonlocalRequestShouldHaveOneFeature()
        {
            var nonLocalFeatureSet = switchboard.GetFeatureSet(nonLocalRequest);
            Assert.AreSame(this.alwaysEnabled, nonLocalFeatureSet.Features.Single());
        }

        [Test]
        public void DuplicateRequestsShouldReturnSameFeatureSet()
        {
            var set1 = switchboard.GetFeatureSet(localRequest);
            var set2 = switchboard.GetFeatureSet(localRequest);
            Assert.AreSame(set1, set2);
        }
    }
}
