namespace Dolstagis.Web.IoC.DSL
{
    public interface IBindingExpression
    {
        IFromExpression<TSource> From<TSource>();
    }
}
