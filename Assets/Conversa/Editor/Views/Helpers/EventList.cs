using System;
using System.Collections.Generic;
using System.Linq;
using Conversa.Runtime.Properties;
using UnityEditor;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public class EventList : VisualElement
    {
        private ConversaGraphView graphView;

        public EventList(ConversaGraphView graphView)
        {
            this.graphView = graphView;
            Draw();
        }

        private void Draw()
        {
            Clear();
            DrawHeader();
            
            GetValueProperties().ToList().ForEach(AddEntry);

        }
        
        private void DrawHeader()
        {
            var label = new Label("Events");
            label.AddToClassList("h1");

            var button = new Button(HandleAddProperty) { text = "+" };
            button.AddToClassList("transparent-button");
            button.AddToClassList("bold");
            button.AddToClassList("font-size-l");

            var wrapper = new VisualElement();
            wrapper.AddToClassList("mb-s");
            wrapper.AddToClassList("flex-direction-row");
            wrapper.AddToClassList("justify-space-between");
            wrapper.Add(label);
            wrapper.Add(button);
            
            Add(wrapper);
        }

        private IEnumerable<BaseProperty> GetProperties() => graphView.conversation.Properties;

        private IEnumerable<BaseProperty> GetValueProperties() => GetProperties().ToList().Where(IsNotEventProperty);

        private void AddEntry(BaseProperty property) =>
            Add(new PropertyEntry(property, HandleEditProperty(property), HandleDeleteProperty(property)));

        private Action<string> HandleEditProperty(BaseProperty property) => newValue =>
        {
            property.Name = newValue;
            Handlers.ExecuteEditProperty(new object[] { graphView, property });
            Draw();
        };

        private Action HandleDeleteProperty(BaseProperty property) => () =>
        {
            graphView.conversation.RemoveProperty(property.Guid);
            Handlers.ExecuteDeleteProperty(new object[] { graphView, property.Guid });
            Draw();
        };

        private void HandleAddProperty()
        {
            var property = new EventProperty("New event");
            Undo.RegisterCompleteObjectUndo(graphView.conversation, "Conversa property added");
            graphView.conversation.AddProperty(property);
            Draw();
        }
        
        

        private static bool IsNotEventProperty(BaseProperty property) => property.GetType() == typeof(EventProperty);
    }
}