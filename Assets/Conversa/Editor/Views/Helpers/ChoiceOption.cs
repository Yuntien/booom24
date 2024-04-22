using Conversa.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
	public class ChoiceOption : VisualElement
	{
		public readonly PortDefinition<BaseNode> portDefinition;

		public readonly Port port;

		private readonly Label label;

		public ChoiceOption(PortDefinition<BaseNode> portDefinition)
		{
			this.portDefinition = portDefinition;

			var option = Resources.Load<VisualTreeAsset>("Helpers/ChoiceOption");
			option.CloneTree(this);

			label = this.Q<Label>("name");
			label.text = portDefinition.Label;
			
			port = General.OutputFlowPort();
			port.portName = "";
			Add(port);
		}

		public void Update()
		{
			label.text = portDefinition.Label;
		}
	}
}