using Conversa.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
	public class ChoiceOptionForm : VisualElement
	{
		public UnityEvent OnDelete { get; } = new UnityEvent();

		public PortDefinition<BaseNode> portDefinition;

		public ChoiceOptionForm(PortDefinition<BaseNode> portDefinition)
		{
			this.portDefinition = portDefinition;

			var option = Resources.Load<VisualTreeAsset>("Helpers/ChoiceOptionForm");
			option.CloneTree(this);

			var textField = this.Q<TextField>();
			textField.isDelayed = true;
			textField.name = portDefinition.Label;
			textField.RegisterValueChangedCallback(evt => portDefinition.Label = evt.newValue);
			textField.SetValueWithoutNotify(portDefinition.Label);

			var button = this.Q<Button>();
			button.clickable.clicked += OnDelete.Invoke;
		}
	}
}