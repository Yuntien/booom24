using Conversa.Runtime;
using Conversa.Runtime.Nodes.MathOperators;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
	public class AbsoluteFloatNodeView : BaseNodeView<AbsoluteFloatNode>
	{
		protected override string Title => "Float";

		public AbsoluteFloatNodeView(Conversation conversation) : base(new AbsoluteFloatNode(), conversation) { }

		public AbsoluteFloatNodeView(AbsoluteFloatNode data, Conversation conversation) : base(data, conversation) { }

		private FloatField floatField;

		protected override void SetBody()
		{
			floatField = new FloatField();
			floatField.SetValueWithoutNotify(Data.Value);
			floatField.RegisterValueChangedCallback(UpdateValue);
			floatField.isDelayed = true;

			var wrapper = new VisualElement();
			wrapper.AddToClassList("p-5");
			wrapper.Add(floatField);

			bodyContainer.Add(wrapper);
		}

		private void UpdateValue(ChangeEvent<float> e)
		{
			OnBeforeChange.Invoke();
			Data.Value = e.newValue;
		}
	}
}