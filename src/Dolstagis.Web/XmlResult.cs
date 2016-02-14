using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using Dolstagis.Web.Http;

namespace Dolstagis.Web
{
    public class XmlResult : ResultBase
    {
        public object Model { get; set; }

        public XmlSerializer Serializer { get; set; }

        public XslCompiledTransform Xslt { get; set; }

        public XsltArgumentList XsltArgs { get; set; }

        public XmlResult(object model)
        {
            Model = model;
            MimeType = "application/xml";
            Encoding = System.Text.Encoding.UTF8;
        }

        public override Task RenderAsync(IRequestContext context)
        {
            Encoding = Encoding ?? Encoding.UTF8;
            return base.RenderAsync(context);
        }

        private async Task TransformAsync(IRequestContext context, IXPathNavigable xml)
        {
            using (var writer = context.Response.GetStreamWriter())
            using (var xWriter = new XmlTextWriter(writer)) {
                Xslt.Transform(xml, XsltArgs ?? new XsltArgumentList(), xWriter);
                await xWriter.FlushAsync();
            }
        }

        protected override async Task SendBodyAsync(IRequestContext context)
        {
            if (Xslt != null) {
                if (Model is XDocument)
                    await TransformAsync(context, ((XDocument)Model).CreateNavigator());
                else if (Model is IXPathNavigable)
                    await TransformAsync(context, ((IXPathNavigable)Model));
                else {
                    var ser = Serializer ?? new XmlSerializer(Model.GetType());
                    var doc = new XDocument();
                    using (var writer = doc.CreateWriter())
                        ser.Serialize(writer, Model);
                    await TransformAsync(context, doc.CreateNavigator());
                }
            }
            else {
                using (var writer = context.Response.GetStreamWriter()) {
                    if (Model is XDocument)
                        ((XDocument)Model).Save(writer);
                    else if (Model is XmlDocument)
                        ((XmlDocument)Model).Save(writer);
                    else if (Model is IXPathNavigable)
                        using (var xWriter = new XmlTextWriter(writer))
                            ((IXPathNavigable)Model).CreateNavigator().WriteSubtree(xWriter);
                    else
                        (Serializer ?? new XmlSerializer(Model.GetType()))
                            .Serialize(writer, Model);
                }
            }
        }
    }
}