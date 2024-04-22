using System;
using System.Reflection;
using Conversa.Runtime;
using Conversa.Runtime.Attributes;
using Conversa.Runtime.Properties;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public class PropertyTypeName : Button { }
    
    public class PropertyEntry : VisualElement
    {
        private readonly BaseProperty property;
        private readonly Action<string> editCallback;
        private readonly Action deleteCallback;
        
        public PropertyEntry(BaseProperty property, Action<string> editCallback, Action deleteCallback)
        {
            this.property = property;
            this.editCallback = editCallback;
            this.deleteCallback = deleteCallback;
            Draw();
        }

        private void Draw()
        {
            Clear();

            var header = new VisualElement();
            header.AddToClassList("mb-s");
            header.AddToClassList("header");
            header.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.RowReverse);
            header.Add(new PropertyTypeName { text = GetPropertyName()});
            header.Add(new PropertyActions(HandleDelete));
            Add(header);

            var nameField = new TextField("Name") { value = property.Name };
            nameField.RegisterValueChangedCallback(HandleValueChanged);
            nameField.isDelayed = true;
            Add(nameField);

            DrawInitialValueInput();
        }

        private void DrawInitialValueInput()
        {
            switch (property)
            {
                case FloatProperty floatProperty:
                {
                    var field = new FloatField("Initial value") { value = floatProperty.InitialValue };
                    field.RegisterValueChangedCallback(evt => floatProperty.InitialValue = evt.newValue);
                    field.isDelayed = true;
                    
                    Add(field);
                    
                    return;
                }
                case BooleanProperty booleanProperty:
                {
                    var field = new Toggle("Initial value") { value = booleanProperty.InitialValue };
                    field.RegisterValueChangedCallback(evt => booleanProperty.InitialValue = evt.newValue);
                    
                    Add(field);

                    break;
                }
                case StringProperty stringProperty:
                {
                    var field = new TextField("Initial value") { value = stringProperty.InitialValue };
                    field.RegisterValueChangedCallback(evt => stringProperty.InitialValue = evt.newValue);
                    
                    Add(field);

                    break;
                }
                case ActorProperty actorProperty:
                {
                    var field = new ObjectField("Initial value");
                    field.objectType= typeof(Actor);
                    field.value = actorProperty.InitialValue;
                    field.RegisterValueChangedCallback(evt => actorProperty.InitialValue = evt.newValue as Actor);
                    
                    Add(field);

                    break;
                }
            }
        }

        private void HandleDelete() => deleteCallback();
        
        private void HandleValueChanged(ChangeEvent<string> e)
        {
            editCallback(e.newValue);
        }

        private string GetPropertyName()
        {
            if (property is EventProperty)
                return "Event";
            
            var type = property.GetType();
            var attributes = type.GetCustomAttribute<ConversationPropertyAttribute>();
            return attributes?.PropertyName;
        }
    }
}