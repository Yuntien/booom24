using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Conversa.Editor.Interfaces;
using Conversa.Runtime;
using Conversa.Runtime.Interfaces;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public abstract class BaseNodeView<T> : Node, IGroupable, INodeView where T: INode
    {
        protected T Data { get; }

        // This is so ugly... but I need access to the graphview, to delete the
        // edges attached to this port. I guess BaseNode should have had a reference
        // to the graphview
        protected ConversaGraphView GraphView => parent.parent.parent.parent as ConversaGraphView;

        protected Conversation Conversation { get; }

        protected virtual string Title => "Base node";

        public string Guid => Data.Guid;
        public string Id => Data.Guid;

        private readonly Dictionary<string, Port> portDictionary = new Dictionary<string, Port>();
        protected List<Port> Ports => portDictionary.Values.ToList();

        public UnityEvent OnBeforeChange { get; } = new UnityEvent();

        protected readonly VisualElement bodyContainer = new VisualElement();

        // Constructor

        protected BaseNodeView(T data, Conversation conversation)
        {
            Data = data;
            Conversation = conversation;
            Setup();
        }

        // Setup

        private void Setup()
        {
            title = Title;

            bodyContainer.AddToClassList("body-container");

            mainContainer.Add(bodyContainer);

            SetRect();
            SetPorts();
            SetBody();
            Refresh();
        }

        private void SetRect() => SetPosition(Data.NodeRect);

        protected virtual void SetPorts()
        {
            var portAttributes = Data.GetType().GetCustomAttributes<PortAttribute>();

            foreach (var portAttribute in portAttributes)
            {
                var direction = portAttribute.Flow == Flow.In ? Direction.Input : Direction.Output;
                var capacity = portAttribute.Capacity == Capacity.One ? Port.Capacity.Single : Port.Capacity.Multi;

                for (var i = 1; i <= portAttribute.Instances; i++)
                {
                    var port = new CustomPort(Orientation.Horizontal, direction, capacity, portAttribute.Type);
                    var label = portAttribute.Instances == 1 ? portAttribute.Name : $"{portAttribute.Name} {i}";
                    var id = portAttribute.Instances == 1 ? portAttribute.Id : $"{portAttribute.Id} {i}";

                    switch (portAttribute.Flow)
                    {
                        case Flow.In:
                            AddInputPort(port, label, id);
                            break;
                        case Flow.Out:
                            AddOutputPort(port, label, id);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            SetSlots();
        }

        private void SetSlots()
        {
            var properties = Data.GetType().GetProperties();

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<SlotAttribute>();
                if (attribute == null) continue;

                var direction = attribute.Flow == Flow.In ? Direction.Input : Direction.Output;
                var capacity = attribute.Capacity == Capacity.One ? Port.Capacity.Single : Port.Capacity.Multi;
                var port = new CustomPort(Orientation.Horizontal, direction, capacity, property.PropertyType);

                switch (attribute.Flow)
                {
                    case Flow.In:
                        port.portName = attribute.Name;
                        RegisterPort(port, attribute.Id);
                        inputContainer.Add(port);
                        AddControl(port, property);
                        break;
                    case Flow.Out:
                        port.portName = attribute.Name;
                        RegisterPort(port, attribute.Id);
                        outputContainer.Add(port);
                        break;
                }
            }
        }


        private void UpdateSlotField(CustomPort port, VisualElement el)
        {
            var isVisible = el.resolvedStyle.display == DisplayStyle.Flex;
            if (port.connected != isVisible) return;
            var newDisplay = port.connected ? DisplayStyle.None : DisplayStyle.Flex;
            el.style.display = new StyleEnum<DisplayStyle>(newDisplay);
        }

        private void AddControl(CustomPort port, PropertyInfo property)
        {
            if (property.PropertyType == typeof(string))
            {
                var value = (string)property.GetValue(Data);

                var field = new TextField();
                field.SetValueWithoutNotify(value);
                field.RegisterValueChangedCallback(e => property.SetValue(Data, e.newValue));
                field.isDelayed = true;

                var wrapper = new VisualElement();
                wrapper.AddToClassList("p-5");
                wrapper.Add(field);

                inputContainer.Add(wrapper);

                schedule.Execute(() => UpdateSlotField(port, wrapper)).Every(100);
            }

            if (property.PropertyType == typeof(float))
            {
                var value = (float)property.GetValue(Data);

                var field = new FloatField();
                field.SetValueWithoutNotify(value);
                field.RegisterValueChangedCallback(e => property.SetValue(Data, e.newValue));
                field.isDelayed = true;

                var wrapper = new VisualElement();
                wrapper.AddToClassList("p-5");
                wrapper.Add(field);

                inputContainer.Add(wrapper);

                schedule.Execute(() => UpdateSlotField(port, wrapper)).Every(100);
            }

            if (property.PropertyType == typeof(bool))
            {
                var value = (bool)property.GetValue(Data);

                var field = new Toggle();
                field.SetValueWithoutNotify(value);
                field.RegisterValueChangedCallback(e =>
                {
                    property.SetValue(Data, e.newValue);
                });

                var wrapper = new VisualElement();
                wrapper.AddToClassList("p-5");
                wrapper.Add(field);

                inputContainer.Add(wrapper);

                schedule.Execute(() => UpdateSlotField(port, wrapper)).Every(100);
            }

            if (property.PropertyType == typeof(Actor))
            {
                var field = new ObjectField();
                field.objectType = typeof(Actor);
                field.SetValueWithoutNotify(property.GetValue(Data) as Actor);
                field.RegisterCallback<ChangeEvent<UnityEngine.Object>>(e => property.SetValue(Data, e.newValue));

                var wrapper = new VisualElement();
                wrapper.AddToClassList("p-5");
                wrapper.Add(field);

                inputContainer.Add(wrapper);

                schedule.Execute(() => UpdateSlotField(port, wrapper)).Every(100);
            }
        }

        protected virtual void SetBody() { }

        //
        // Ports
        //

        public bool GetPort(string guid, out Port port) => portDictionary.TryGetValue(guid, out port);

        public string GetPortId(Port port) => portDictionary.First(x => x.Value == port).Key;

        private Action<Port, string, string> AddPort(VisualElement el) => (port, portName, portId) =>
        {
            port.portName = portName;
            RegisterPort(port, portId);
            el.Add(port);
        };

        protected Action<Port, string, string> AddInputPort => AddPort(inputContainer);

        protected Action<Port, string, string> AddOutputPort => AddPort(outputContainer);

        protected void RegisterPort(Port port, string guid) => portDictionary.Add(guid, port);

        //
        // Other
        //

        [Obsolete("Use *'UpdateData' instead of 'UpdateRect'")]
        public void UpdateRect() => UpdateData();

        public void UpdateData() => Data.NodeRect = GetPosition();

        protected void Refresh()
        {
            RefreshExpandedState();
            RefreshPorts();
        }

        public void SetRealPosition(Vector2 position)
        {
            var rect = GetPosition();
            rect.position = position;
            Data.NodeRect = rect;

            SetPosition(rect);
        }

        public INode GetNode() => Data;
    }
}
