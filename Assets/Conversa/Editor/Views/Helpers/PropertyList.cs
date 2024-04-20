using System;
using System.Collections.Generic;
using System.Linq;
using Conversa.Runtime.Interfaces;
using Conversa.Runtime.Properties;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public class PropertyList : VisualElement
    {
        private readonly ConversaGraphView graphView;

        public PropertyList(ConversaGraphView graphView)
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
            var label = new Label("Properties");
            label.AddToClassList("h1");

            var wrapper = new VisualElement();
            wrapper.AddToClassList("mb-s");
            wrapper.AddToClassList("flex-direction-row");
            wrapper.AddToClassList("justify-space-between");
            wrapper.Add(label);
            wrapper.Add(new AddPropertyButton(graphView, Draw));
            
            Add(wrapper);
        }
        
        private IEnumerable<IValueProperty> GetValueProperties() => graphView.conversation.GetValueProperties();

        private void AddEntry(IValueProperty property)
        {
            if (property is BaseProperty baseProperty)
                Add(new PropertyEntry(
                    baseProperty,
                    HandleEditProperty(baseProperty),
                    HandleDeleteProperty(baseProperty)
                ));
        }
        
        private Action<string> HandleEditProperty(BaseProperty property) => newValue =>
        {
            graphView.conversation.EditProperty(property.Guid, newValue);
            Handlers.ExecuteEditProperty(new object[] { graphView, property });
            Draw();
        };

        private Action HandleDeleteProperty(BaseProperty property) => () =>
        {
            graphView.conversation.RemoveProperty(property.Guid);
            Handlers.ExecuteDeleteProperty(new object[] { graphView, property.Guid });
            Draw();
        };
    }
}