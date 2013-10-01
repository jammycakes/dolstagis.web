﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Views
{
    public interface IResource
    {
        bool Exists { get; }

        DateTime LastModified { get; }

        long? Length { get; }

        Stream Open();
    }
}
