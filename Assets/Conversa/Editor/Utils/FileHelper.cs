using System.IO;
using UnityEditor;

namespace Conversa.Editor
{
    internal static class FileHelper
    {
        public static void CreateScriptAssetWithContent(string pathName, string templateContent)
        {
            var fullPath = Path.GetFullPath(pathName);
            File.WriteAllText(fullPath, templateContent);
            AssetDatabase.ImportAsset(pathName);
        }
    }
}