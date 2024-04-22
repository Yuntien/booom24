using System;
using Conversa.Runtime;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public class ActorField : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<ActorField, UxmlTraits> { }

        private Action<string, bool, Actor> changeEventCallback;

        private readonly ObjectField actorObjectField;
        private readonly TextField actorTextField;
        private readonly Toggle useActorProfileToggle;
        
        public ActorField()
        {
            AddStyles();
            
            actorObjectField = new ObjectField("Actor");
            actorObjectField.objectType = typeof(Actor);
            actorObjectField.RegisterCallback<ChangeEvent<UnityEngine.Object>>(e =>
            {
                switch (e.newValue)
                {
                    case null:
                        changeEventCallback.Invoke(actorTextField.value, useActorProfileToggle.value, null);
                        break;
                    case Actor actor:
                        changeEventCallback.Invoke(actorTextField.value, useActorProfileToggle.value, actor);
                        break;
                }
            });
            
            actorTextField = new TextField("Actor");
            actorTextField.RegisterValueChangedCallback(evt => HandleChangeData());
            actorTextField.isDelayed = true;

            useActorProfileToggle = new Toggle("Use actor profile");
            useActorProfileToggle.RegisterValueChangedCallback(evt => HandleChangeData());

            Add(actorObjectField);
            Add(actorTextField);
            Add(useActorProfileToggle);

        }

        private void HandleChangeData()
        {
            UpdateVisibility();
            FireChangeCallback();
        }

        private void FireChangeCallback() =>
            changeEventCallback.Invoke(actorTextField.value, useActorProfileToggle.value, actorObjectField.value as Actor);

        private void AddStyles() => styleSheets.Add(Resources.Load<StyleSheet>("ActorField"));

        private void UpdateVisibility()
        {
            ShowIf(actorObjectField, useActorProfileToggle.value);
            ShowIf(actorTextField, !useActorProfileToggle.value);
        }
        
        // public

        public void OnChange(Action<string, bool, Actor> callback) => changeEventCallback = callback;

        public void SetValueWithoutNotify(string staticActor, bool useActorProfile, Actor actorProfile)
        {
            actorObjectField.SetValueWithoutNotify(actorProfile);
            actorTextField.SetValueWithoutNotify(staticActor);
            useActorProfileToggle.SetValueWithoutNotify(useActorProfile);
            UpdateVisibility();
        }
        
        // Static
        
        private static DisplayStyle GetDisplayStyle(bool visible) => visible ? DisplayStyle.Flex : DisplayStyle.None;

        private static void ShowIf(VisualElement el, bool visible) =>
            el.style.display = new StyleEnum<DisplayStyle>(GetDisplayStyle(visible));


    }
}