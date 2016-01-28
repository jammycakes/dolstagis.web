namespace Dolstagis.Web.Features
{
    public interface IHandlerExpression
    {
        void WithModelBinder<TModelBinder>() where TModelBinder : IModelBinder;

        void WithModelBinder(IModelBinder binder);
    }
}