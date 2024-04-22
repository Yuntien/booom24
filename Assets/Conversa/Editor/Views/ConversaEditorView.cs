using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public class ConversaEditorView : VisualElement
    {
        public UnityEvent OnSave { get; } = new UnityEvent();
        public UnityEvent OnReset { get; } = new UnityEvent();

        private readonly Inspector inspector;

        public ConversaEditorView(ConversaGraphView graphView)
        {
            SetupStyles();

            var toolbar = new GraphToolbar();
            toolbar.OnSave.AddListener(OnSave.Invoke);
            toolbar.OnReset.AddListener(OnReset.Invoke);
            toolbar.OnToggleInspector.AddListener(HandleToggleInspector);

            var content = new VisualElement {name = "content"};
            content.Add(graphView);
            
            inspector = new Inspector(graphView); 
            content.Add(inspector);

            Clear();
            Add(toolbar);
            Add(content);
        }
        
        private void SetupStyles() => styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));

        private void HandleToggleInspector(bool value) => inspector.visible = value;
    }
}