namespace Dolstagis.Web.Features
{
    public interface IControllerExpression
    {
        IControllerExpression WithModelBinder(IModelBinder modelBinderFunc);
    }
}