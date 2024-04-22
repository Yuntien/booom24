using System.IO;
using UnityEditor;
using UnityEngine;

namespace Conversa.Editor
{
    internal class ConversaFolderLocator : ScriptableObject
    {
        // Used to locate the place where the auto-generated nodes will be placed
        public static string GetFolderPath()
        {
            var locator = CreateInstance<ConversaFolderLocator>();
            var script = MonoScript.FromScriptableObject(locator);

            var path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(script));
            if (Path.DirectorySeparatorChar != '/')
                path = path.Replace(Path.DirectorySeparatorChar, '/');

            DestroyImmediate(locator);
            return path;
        }
    }
}