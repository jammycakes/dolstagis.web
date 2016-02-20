﻿using System;
using System.IO;

namespace Dolstagis.Web.Static
{
    public class FileResource : IResource
    {
        FileInfo _fileInfo;

        public FileResource(string physicalPath)
        {
            _fileInfo = new FileInfo(physicalPath);
            IsFile = _fileInfo.Exists;
            LastModified = _fileInfo.LastWriteTimeUtc;
            Length = _fileInfo.Length;
        }

        public bool IsFile { get; private set; }

        public DateTime LastModified { get; private set; }

        public long? Length { get; private set; }

        public string Name { get { return _fileInfo.Name; } }

        public System.IO.Stream Open()
        {
            return new FileStream(_fileInfo.FullName, FileMode.Open, FileAccess.Read);
        }
    }
}
