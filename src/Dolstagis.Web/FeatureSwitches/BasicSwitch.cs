namespace Dolstagis.Web.FeatureSwitches
{
    public class BasicSwitch : IFeatureSwitch
    {
        private bool _enabled;

        public bool IsEnabledForRequest(Http.IRequest request)
        {
            return _enabled;
        }

        public BasicSwitch(bool enabled)
        {
            _enabled = enabled;
        }
    }
}
