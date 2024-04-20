using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace Conversa.Editor
{
    public class CustomNodeGenerator : EndNameEditAction
    {
        public enum NodeType { EventNode, DataNode }

        public NodeType nodeType;

        private static void CreateDirectory(string pathName)
        {
            var folderPath = Path.GetDirectoryName(pathName);
            var folderName = Path.GetFileName(pathName);
            var folderGuid = AssetDatabase.CreateFolder(folderPath, folderName);
            var folder = AssetDatabase.LoadAssetAtPath(pathName, typeof(Object));

            ProjectWindowUtil.ShowCreatedAsset(folder);
        }

        private static string GetTemplatePath(string templateName) =>
            $"{ConversaFolderLocator.GetFolderPath()}/Templates/{templateName}.cs.txt";

        private static string GetTemplateContent(string templateName) =>
            File.ReadAllText(GetTemplatePath(templateName));

        private static string GenerateFromTemplate(string templateName, string nodeName) =>
            GetTemplateContent(templateName).Replace("#NODENAME#", nodeName);

        private static void WriteTemplate(string path, string filename, string content) => 
            FileHelper.CreateScriptAssetWithContent($"{path}/{filename}", content);

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var nodeName = Regex.Replace(Path.GetFileName(pathName), @"\s+", "");
            
            CreateDirectory(pathName);
            CreateDirectory(pathName + "/Editor");

            if (nodeType == NodeType.EventNode)
            {
                var nodeScript = GenerateFromTemplate("EventNode", nodeName);
                var nodeViewScript = GenerateFromTemplate("NodeView", nodeName);
                var nodeEventScript = GenerateFromTemplate("NodeEvent", nodeName);
                var menuModifierScript = GenerateFromTemplate("NodeMenuModifier", nodeName);
            
                WriteTemplate(pathName, $"{nodeName}Node.cs", nodeScript);
                WriteTemplate(pathName, $"{nodeName}Event.cs", nodeEventScript);
                WriteTemplate(pathName + "/Editor", $"{nodeName}NodeView.cs", nodeViewScript);
                WriteTemplate(pathName + "/Editor", $"{nodeName}MenuModifier.cs", menuModifierScript);
            }
            else
            {
                var nodeScript = GenerateFromTemplate("DataNode", nodeName);
                var nodeViewScript = GenerateFromTemplate("NodeView", nodeName);
                var menuModifierScript = GenerateFromTemplate("NodeMenuModifier", nodeName);
            
                WriteTemplate(pathName, $"{nodeName}Node.cs", nodeScript);
                WriteTemplate(pathName + "/Editor", $"{nodeName}NodeView.cs", nodeViewScript);
                WriteTemplate(pathName + "/Editor", $"{nodeName}MenuModifier.cs", menuModifierScript);
            }
        }
        
        [MenuItem("Assets/Create/Conversa/Create custom node/Event", false, 9999 )]
        public static void CreateEventNodeType() => CreateNodeType(NodeType.EventNode);

        [MenuItem("Assets/Create/Conversa/Create custom node/Data", false, 9999)]
        public static void CreateDataNodeType() => CreateNodeType(NodeType.DataNode);

        private static void CreateNodeType(NodeType nodeType)
        {
            var doCreate = CreateInstance<CustomNodeGenerator>();
            doCreate.nodeType = nodeType;
            var folderIcon = EditorGUIUtility.IconContent(EditorResources.emptyFolderIconName).image as Texture2D;
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, doCreate, "NewNodeName", folderIcon, null);
        }
    }
}