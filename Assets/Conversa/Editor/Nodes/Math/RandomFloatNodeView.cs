using Conversa.Runtime;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Conversa.Editor.Nodes.Math
{
	public class RandomFloatView : BaseNodeView<RandomFloatNode>
	{
		protected override string Title => "Random";

		public RandomFloatView(Conversation conversation) : base(new RandomFloatNode(), conversation) { }

		public RandomFloatView(RandomFloatNode data, Conversation conversation) : base(data, conversation) { }

		private FloatField minField, maxField;

		protected override void SetBody()
		{
			var labelMin = new Label("Min");
			var labelMax = new Label("Max");
			
			minField = new FloatField();
			minField.SetValueWithoutNotify(Data.Min);
			minField.RegisterValueChangedCallback(UpdateMin);
			minField.isDelayed = true;

			maxField = new FloatField();
			maxField.SetValueWithoutNotify(Data.Max);
			maxField.RegisterValueChangedCallback(UpdateMax);
			maxField.isDelayed = true;

			var wrapper = new VisualElement();
			wrapper.AddToClassList("p-5");
			wrapper.Add(labelMin);
			wrapper.Add(minField);
			wrapper.Add(labelMax);
			wrapper.Add(maxField);

			bodyContainer.Add(wrapper);
		}

		private void UpdateMin(ChangeEvent<float> e)
		{
			OnBeforeChange.Invoke();
			Data.Min = e.newValue;
		}

		private void UpdateMax(ChangeEvent<float> e)
		{
			OnBeforeChange.Invoke();
			Data.Max = e.newValue;
		}
	}
}