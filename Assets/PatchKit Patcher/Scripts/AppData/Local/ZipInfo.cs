using System;
using JetBrains.Annotations;

namespace PatchKit.Unity.Patcher.AppData.Local
{
    public struct ZipInfo
    {
        [NotNull] private readonly string _path;
        [CanBeNull] private readonly string _password;

        public ZipInfo([NotNull] string path, [CanBeNull] string password)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Value cannot be null or empty.", "path");
            }

            _path = path;
            _password = password;
        }

        [NotNull]
        public string Path
        {
            get { return _path; }
        }

        [CanBeNull]
        public string Password
        {
            get { return _password; }
        }
    }
}