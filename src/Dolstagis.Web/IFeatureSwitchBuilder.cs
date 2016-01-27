namespace Dolstagis.Web
{
    public interface IFeatureSwitchBuilder
    {
        IFeatureSwitch CreateSwitch(Feature feature, Application application);
    }
}
