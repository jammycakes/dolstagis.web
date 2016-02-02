using System;
using System.Collections.Generic;

namespace Dolstagis.Web.ModelBinding
{
    public interface IConverter
    {
        bool CanConvert(Type type);

        object Convert(Type type, string name, IDictionary<string, string[]> data);

        int Priority { get; }
    }
}
