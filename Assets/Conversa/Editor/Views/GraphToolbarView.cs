using UnityEditor.UIElements;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public class GraphToolbar : Toolbar
    {
        public UnityEvent OnSave { get; } = new UnityEvent();
        public UnityEvent<bool> OnToggleInspector { get; } = new UnityEvent<bool>();
        public UnityEvent OnReset { get; } = new UnityEvent();
        
        public GraphToolbar()
        {
            var saveButton = new ToolbarButton(OnSave.Invoke) {text = "Save"};

            var resetButton = new ToolbarButton(OnReset.Invoke) {text = "Reset"};

            var toggle = new ToolbarToggle {text = "Inspector", value = true};
            toggle.RegisterCallback<ClickEvent>(evt => OnToggleInspector.Invoke(toggle.value));
            
            Add(saveButton);
            // Add(resetButton);
            Add(toggle);
        }
    }
}
