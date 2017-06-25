using System;

namespace PatchKit.Patcher.AppData.Local
{
    public class LibrsyncException : Exception
    {
        public LibrsyncException(int status) : base(string.Format("librsync failure - {0}", status))
        {
        }
    }
}