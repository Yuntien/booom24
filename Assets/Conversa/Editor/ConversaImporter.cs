using System.Linq;
using Conversa.Runtime;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Conversa.Editor
{
	[CustomEditor(typeof(Conversation))]
	public class ConversaImporter : UnityEditor.Editor
	{
		// Called when the mouse hovers the inspector, while the ScriptableObject is
		// selected on the Project tab.
		public override void OnInspectorGUI()
		{
			if (!GUILayout.Button("Edit conversation graph")) return;
			if (target is Conversation conversation) OnOpenAsset(conversation.GetInstanceID(), 0);
		}

		// Helpers

		private static T[] GetEditorWindows<T>() where T : Object => Resources.FindObjectsOfTypeAll<T>();

		private static bool IsAssetOfType<T>(string path) where T : Object =>
			AssetDatabase.LoadAssetAtPath<T>(path) != null;

		// Menu handler

		[OnOpenAsset(0)]
		public static bool OnOpenAsset(int instanceID, int line)
		{
			var path = AssetDatabase.GetAssetPath(instanceID);

			// This function is called, when ANY asset is clicked. We have to stop, if we are not clicking on
			// a Conversation asset
			if (!IsAssetOfType<Conversation>(path))
				return false;

			var guid = AssetDatabase.AssetPathToGUID(path);

			if (guid == "")
			{
				Debug.LogError("Conversa error: Wrong guid");
				return false;
			}

			var editorWindow = GetEditorWindows<ConversaEditorWindow>().ToList().FirstOrDefault(x => x.assetGuid == guid);

			if (editorWindow == null)
			{
				editorWindow = CreateEditorWindow(guid);
			}

			editorWindow.Focus();

			return true;
		}

		private static ConversaEditorWindow CreateEditorWindow(string guid)
		{
			var window = EditorWindow.CreateWindow<ConversaEditorWindow>(typeof(SceneView));
			window.Initialize(guid);
			return window;
		}
	}
}