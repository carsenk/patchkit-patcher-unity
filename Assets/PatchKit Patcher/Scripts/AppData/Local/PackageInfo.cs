using System;
using System.ComponentModel;
using JetBrains.Annotations;

namespace PatchKit.Unity.Patcher.AppData.Local
{
    public struct PackageInfo
    {
        [NotNull] private readonly string _path;
        private readonly PackageType _type;
        [CanBeNull] private readonly string _password;
        [CanBeNull] private readonly Pack1Meta _pack1Meta;

        public PackageInfo([NotNull] string path, PackageType type, [CanBeNull] string password,
            [CanBeNull] Pack1Meta pack1Meta)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Value cannot be null or empty.", "path");
            }

            if (!Enum.IsDefined(typeof(PackageType), type))
            {
                throw new InvalidEnumArgumentException("type", (int) type, typeof(PackageType));
            }

            _path = path;
            _type = type;
            _password = password;
            _pack1Meta = pack1Meta;
        }

        [NotNull]
        public string Path
        {
            get { return _path; }
        }

        public PackageType Type
        {
            get { return _type; }
        }

        [CanBeNull]
        public string Password
        {
            get { return _password; }
        }

        [CanBeNull]
        public Pack1Meta Meta
        {
            get { return _pack1Meta; }
        }
    }
}