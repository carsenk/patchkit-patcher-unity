using System.IO;

namespace PatchKit.Patcher.AppData
{
    public static class StringPathExtensions
    {
        public static string PathCombine(this string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }
    }
}