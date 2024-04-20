using System;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public class PropertyActions : VisualElement
    {
        public PropertyActions(Action deleteCallback)
        {
            AddButton(deleteCallback, "Delete");
        }

        private void AddButton(Action callback, string text) => Add(new Button(callback) { text = text });
    }
}