using Conversa.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
	public class ConversaEditorWindow : EditorWindow
	{
		public string assetGuid;

		// This is an alias, because I HATE that a "Rect" is  called "position", which sounds more like a "Vector2"
		public Rect WindowRect => position;

		private ConversaGraphView graphView;
		private ConversaEditorView graphEditorView;

		[SerializeField] private Conversation persistedAsset;
		[SerializeField] private Conversation inMemoryAsset;

		private void HandleUndo() => Paint();

		private void SetEventListeners() => Undo.undoRedoPerformed += HandleUndo;

		private bool IsDirty()
		{
			var clone = JsonUtility.ToJson(inMemoryAsset);
			var original = JsonUtility.ToJson(persistedAsset);

			return clone != original;
		}

		private static int CloseDirtyConversationDialog() =>
			EditorUtility.DisplayDialogComplex(
				"Conversation has been modified",
				"Your changes will be lost if you do not save them",
				"Save", "Cancel", "Discard changes");

		private void Awake()
		{
			saveChangesMessage = "Your changes will be lost if you do not save them";
			SetEventListeners();
		}

		private void OnEnable()
		{
			if (assetGuid == null) return;
			if (inMemoryAsset == null)
				Initialize(assetGuid);
			else
				Paint();
		}

		private void OnDestroy()
		{
			// Footnote #3
			if (inMemoryAsset != null && IsDirty())
			{
				switch (CloseDirtyConversationDialog())
				{
					// Save
					case 0:
						HandleSave();
						Undo.ClearUndo(inMemoryAsset);
						break;

					// Cancel
					case 1:
						Clone();
						// We don't want to destroy the inMemoryAsset, since it is used by the clone
						return;

					// Discard
					case 2:
						Undo.ClearUndo(inMemoryAsset);
						break;
				}
			}
			DestroyImmediate(inMemoryAsset); // Footnote #2
		}

		private void Clone()
		{
			var newWindow = CreateWindow<ConversaEditorWindow>(typeof(ConversaEditorWindow), typeof(SceneView));
			newWindow.Initialize(this);
		}

		public void Initialize(string guid)
		{
			assetGuid = guid;

			persistedAsset = General.LoadAsset<Conversation>(guid);
			titleContent = new GUIContent(persistedAsset.name);


			inMemoryAsset = Instantiate(persistedAsset);
			inMemoryAsset.name = persistedAsset.name;
			inMemoryAsset.hideFlags = HideFlags.HideAndDontSave; // Footnote #2
			inMemoryAsset.Validate();

			Paint();
		}

		public void Initialize(ConversaEditorWindow editor)
		{
			assetGuid = editor.assetGuid;
			inMemoryAsset = editor.inMemoryAsset;
			persistedAsset = editor.persistedAsset;
			titleContent = new GUIContent(persistedAsset.name);
			Paint();
		}


		private void Paint()
		{
			if (inMemoryAsset == null || string.IsNullOrEmpty(assetGuid))
				return;

			graphView = new ConversaGraphView(this, inMemoryAsset);

			graphEditorView = new ConversaEditorView(graphView);
			graphEditorView.OnSave.AddListener(HandleSave);
			graphEditorView.OnReset.AddListener(HandleReset);

			rootVisualElement.Clear();
			rootVisualElement.Add(graphEditorView);

			graphEditorView.StretchToParentSize();
		}

		// Handlers

		private void HandleSave()
		{
			inMemoryAsset.Validate();
			if (!IsDirty()) return;

			var instance = Instantiate(inMemoryAsset);
			persistedAsset.Apply(instance); // Footnote #1
			EditorUtility.SetDirty(persistedAsset);
		}

		private void HandleReset()
		{
			General.ImportAsset(assetGuid);
			Initialize(assetGuid);
		}
	}
}

// Footnotes:

// 1) "inMemoryAsset" must remain being the object manipulated in the editor. To persist it, what we
// do is a copy of it, and use that copy to overwrite "persistedAsset".
// If we used "inMemoryAsset" directly, and then keep working with the graph, we would be editing
// the persisted copy instead

// 2) When you press PLAY in the editor, and a scene loads, Unity calls "Resources.UnloadUnusedAssets".
// Since "inMemoryAsset" is not being referenced anywhere, Unity would set it to null. So if you want
// to keep working on the graph, after pressing PLAY, you need to use the flag "HideFlags.HideAndDontSave"
// to tell Unity to not unload it, and ignore it.
//
// As a consequence, you also have to take care of manually destroying it in "OnDestroy"

// 3) When you double click on the tab name, the windows maximizes. If you double click once more, it
// returns to its original size. Doing this "unmaximize" will call "OnDestroy" and clone the editor
// window, so that you can continue working on the graph. It will serialize the editor, and set the
// values of to null. This is the only way to tell apart "unmaximize" from "close".