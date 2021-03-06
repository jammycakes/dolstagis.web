﻿using System;
using System.IO;
using System.Reflection;

namespace Dolstagis.Web.Static
{
    public class AssemblyResource : IResource
    {
        private Assembly _assembly;
        private string _resourceName;

        public AssemblyResource(Assembly assembly, string resourceName)
        {
            _assembly = assembly;
            _resourceName = resourceName;
            var info = _assembly.GetManifestResourceInfo(resourceName);
            IsFile = (info != null);
            LastModified = File.GetLastWriteTime(_assembly.Location);
            Length = null;
        }

        public AssemblyResource(Type baseClass, string resourceName)
            : this(baseClass.Assembly, baseClass.Namespace + "." + resourceName)
        { }

        public bool IsFile { get; private set; }

        public DateTime LastModified { get; private set; }

        public long? Length { get; private set; }

        public string Name { get { return _resourceName; } }

        public System.IO.Stream Open()
        {
            return _assembly.GetManifestResourceStream(_resourceName);
        }
    }
}
