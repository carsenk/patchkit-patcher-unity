using System;
using JetBrains.Annotations;

namespace PatchKit.Unity.Patcher.AppData.Local
{
    public struct Pack1Info
    {
        [NotNull] private readonly string _path;
        [NotNull] private readonly string _password;
        [NotNull] private readonly Pack1Meta _pack1Meta;

        public Pack1Info([NotNull] string path, [NotNull] string password, [NotNull] Pack1Meta pack1Meta)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Value cannot be null or empty.", "path");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            if (pack1Meta == null)
            {
                throw new ArgumentNullException("pack1Meta");
            }

            _path = path;
            _password = password;
            _pack1Meta = pack1Meta;
        }

        [NotNull]
        public string Path
        {
            get { return _path; }
        }

        [NotNull]
        public string Password
        {
            get { return _password; }
        }

        [NotNull]
        public Pack1Meta Meta
        {
            get { return _pack1Meta; }
        }
    }
}