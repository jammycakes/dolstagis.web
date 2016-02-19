using System;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Static;
using Dolstagis.Web.Views;

namespace Dolstagis.Web.Features.Impl
{
    public class ViewExpression : IStaticFilesExpression
    {
        private ViewTable _viewTable;
        private VirtualPath _root;

        public ViewExpression(ViewTable viewTable, VirtualPath root)
        {
            _viewTable = viewTable;
            _root = root;
        }

        public void FromResource(Func<VirtualPath, IServiceLocator, IResource> locator)
        {
            _viewTable.Add(_root, locator);
        }
    }
}
