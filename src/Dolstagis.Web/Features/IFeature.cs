namespace Dolstagis.Web.Features
{
    public interface IFeature : ILegacyFeature
    {
        IFeatureSwitch Switch { get; }
        string Description { get; }
    }
}
