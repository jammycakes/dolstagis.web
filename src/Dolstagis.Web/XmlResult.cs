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

        private async Task TransformAsync(IRequestContext context, XPathNavigator xml)
        {
            await Task.Run(() => {
                using (var writer = new StreamWriter(context.Response.Body, Encoding))
                using (var xWriter = new XmlTextWriter(writer)) {
                    if (Xslt != null) {
                        Xslt.Transform(xml, XsltArgs ?? new XsltArgumentList(), xWriter);
                    }
                    else {
                        xml.WriteSubtree(xWriter);
                    }
                }
            });
        }

        protected override async Task SendBodyAsync(IRequestContext context)
        {
            if (Xslt != null) {
                if (Model is XDocument)
                    await TransformAsync(context, ((XDocument)Model).CreateNavigator());
                else if (Model is IXPathNavigable)
                    await TransformAsync(context, ((IXPathNavigable)Model).CreateNavigator());
                else if (Model is XPathNavigator)
                    await TransformAsync(context, (XPathNavigator)Model);
                else {
                    var ser = Serializer ?? new XmlSerializer(Model.GetType());
                    var doc = new XDocument();
                    using (var writer = doc.CreateWriter())
                        ser.Serialize(writer, Model);
                    await TransformAsync(context, doc.CreateNavigator());
                }
            }
            else {
                using (var writer = new StreamWriter(context.Response.Body, Encoding))
                using (var xWriter = new XmlTextWriter(writer)) {
                    if (Model is XDocument)
                        await Task.Run(() => ((XDocument)Model).Save(xWriter));
                    else if (Model is XmlDocument)
                        await Task.Run(() => ((XmlDocument)Model).Save(xWriter));
                    else if (Model is IXPathNavigable)
                        await Task.Run(() =>
                            ((IXPathNavigable)Model).CreateNavigator().WriteSubtree(xWriter));
                    else if (Model is XPathNavigator)
                        await Task.Run(() =>
                            ((XPathNavigator)Model).WriteSubtree(xWriter));
                    else
                        (Serializer ?? new XmlSerializer(Model.GetType()))
                            .Serialize(xWriter, Model);
                }
            }
        }
    }
}
