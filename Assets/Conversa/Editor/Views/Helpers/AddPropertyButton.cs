using System;
using System.Reflection;
using Conversa.Runtime.Attributes;
using Conversa.Runtime.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public class AddPropertyButton : VisualElement
    {
        private readonly ConversaGraphView graphView;
        private readonly Action onAdd;
        
        public AddPropertyButton(ConversaGraphView graphView, Action onAdd)
        {
            this.onAdd = onAdd; 
            this.graphView = graphView;
            var button = new Button(DisplayAddPropertyMenu) { text = "+"};
            button.AddToClassList("transparent-button");
            button.AddToClassList("bold");
            button.AddToClassList("font-size-l");
            Add(button);
        }
        
        private void DisplayAddPropertyMenu()
        {
            var menu = new GenericMenu();

            foreach (var type in TypeCache.GetTypesWithAttribute<ConversationPropertyAttribute>())
            {
                var propertyTypeName = GetPropertyName(type);
                var instance = CreateInstance(type, propertyTypeName);
                menu.AddItem(new GUIContent(propertyTypeName), false, () => HandleAddProperty(instance));
            }

            menu.ShowAsContext();
        }

        private void HandleAddProperty(BaseProperty property)
        {
            Undo.RegisterCompleteObjectUndo(graphView.conversation, "Conversa property added");
            graphView.conversation.AddProperty(property);
            onAdd();
        }
        
        private static string GetPropertyName(MemberInfo type) =>
            type.GetCustomAttribute<ConversationPropertyAttribute>().PropertyName;

        private static BaseProperty CreateInstance(Type type, string propertyTypeName) =>
            Activator.CreateInstance(type, propertyTypeName) as BaseProperty;

    }
}