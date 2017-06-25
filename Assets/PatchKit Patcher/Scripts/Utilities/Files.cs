using System.IO;

namespace PatchKit.Patcher.Utilities
{
    public class Files
    {
        public static void CreateParents(string path)
        {
            var dirName = Path.GetDirectoryName(path);
            if (dirName != null)
            {
                Directory.CreateDirectory(dirName);
            }
        }
    }
}