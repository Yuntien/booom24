using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public class ConversaSettingsWindow : EditorWindow
    {
        private void OnEnable()
        {
            titleContent = new GUIContent("Conversa settings");
            Draw();
        }

        private void OnDisable()
        {
            rootVisualElement.Clear();
        }

        private void Draw()
        {
            rootVisualElement.Clear();
            var template = Resources.Load<VisualTreeAsset>("Templates/Settings");
            template.CloneTree(rootVisualElement);

            var actorProfilesToggle = rootVisualElement.Q<Toggle>("actor-profiles");
            actorProfilesToggle.value = ConversaSettings.instance.UseActorProfileByDefault; 
            actorProfilesToggle.RegisterValueChangedCallback(evt => ConversaSettings.instance.UseActorProfileByDefault = evt.newValue);

            var stopEventsToggle = rootVisualElement.Q<Toggle>("stop-events");
            stopEventsToggle.value = ConversaSettings.instance.StopEventsByDefault; 
            stopEventsToggle.RegisterValueChangedCallback(evt => ConversaSettings.instance.StopEventsByDefault = evt.newValue);
        }

        private static ConversaSettingsWindow settingsWindow;

        [MenuItem("Conversa/Settings")]
        public static void ShowWindow()
        {
            if (settingsWindow == null)
            {
                settingsWindow = CreateWindow<ConversaSettingsWindow>(typeof(ConversaEditorWindow), typeof(SceneView));
                return;
            }
            
            settingsWindow.Focus();

            
            
        }

        [MenuItem("Conversa/Tutorials & help")]
        private static void OpenMyWebsite() =>
            Application.OpenURL("https://github.com/enriquemorenotent/unity-conversa-support/wiki");
    }
}