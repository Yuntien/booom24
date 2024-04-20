using System.Collections.Generic;
using System.Linq;
using Conversa.Runtime;
using Conversa.Runtime.Interfaces;
using unity_conversa.Runtime.Nodes.PropertyNodes;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
	public class SetPropertyNodeView : BaseNodeView<SetPropertyNode>
	{
		protected override string Title => "Set property";

		// Constructors

		public SetPropertyNodeView(Conversation conversation) : base(new SetPropertyNode(), conversation) { }

		public SetPropertyNodeView(SetPropertyNode data, Conversation conversation) : base(data, conversation) { }

		// Methods

		protected override void SetBody()
		{
			bodyContainer.AddToClassList("p-10");
			Conversation.OnPropertyListModified.AddListener(DrawDropdown);
			DrawDropdown();
		}

		private void DrawDropdown()
		{
			bodyContainer.Clear();
			var propertyList = GetPropertyList();
			if (propertyList == null || !propertyList.Any())
			{
				var text = new TextElement { text = "No properties available" };
				bodyContainer.Add(text);
			}
			else
			{
				var selectedValue = GetSelectedProperty() ?? GetDefaultProperty();
				var dropdown = new PopupField<IValueProperty>(propertyList, selectedValue, FormatProperty, FormatProperty);
				dropdown.RegisterCallback<ChangeEvent<IValueProperty>>(HandleChangeSelectedProperty);
				bodyContainer.Add(dropdown);
			}
			UpdateVisiblePorts();
		}

		private void HandleChangeSelectedProperty(ChangeEvent<IValueProperty> evt)
		{
			Data.PropertyGuid = evt.newValue.Guid;
			UpdateVisiblePorts();
		}

		private void UpdateVisiblePorts()
		{
			var selectedProperty = GetSelectedProperty();

			foreach (var port in Ports.Where(port => port.portType != typeof(BaseNode)))
				EnablePort(port, selectedProperty != null && port.portType == selectedProperty.GetValueType());

			Refresh();
		}

		private void EnablePort(Port port, bool enabled)
		{
			UpdateVisibility(port, enabled);
			if (enabled) return;
			if (port.connections == null || !port.connections.Any()) return;
			GraphView.DeleteElements(port.connections);
			port.DisconnectAll();
		}

		private void UpdateVisibility(VisualElement port, bool isVisible) => port.style.display =
			new StyleEnum<DisplayStyle>(isVisible ? DisplayStyle.Flex : DisplayStyle.None);

		private List<IValueProperty> GetPropertyList() => Conversation.GetValueProperties().ToList();

		private IValueProperty GetDefaultProperty()
		{
			var list = GetPropertyList();
			if (list == null || !list.Any()) return null;
			return list[0];
		}

		private IValueProperty GetSelectedProperty()
		{
			var property = Conversation.GetProperty(Data.PropertyGuid);
			if (property is IValueProperty valueProperty)
				return valueProperty;

			return GetDefaultProperty();
		}

		private static string FormatProperty(IValueProperty property) => property.Name;
	}
}