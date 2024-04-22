using System;
using System.Collections.Generic;
using System.Linq;
using Conversa.Runtime;
using Conversa.Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
	public class ChoiceNodeView : BaseNodeView<ChoiceNode>
	{
		protected override string Title => "Choice";

		private Label actorLabel;
		private Label messageLabel;
		private VisualElement optionList;

		// Constructors

		public ChoiceNodeView(Conversation conversation)
			: base(new ChoiceNode(ConversaSettings.instance.UseActorProfileByDefault), conversation) { }

		public ChoiceNodeView(ChoiceNode data, Conversation conversation) : base(data, conversation) { }

		// Methods

		protected override void SetBody()
		{
			var template = Resources.Load<VisualTreeAsset>("NodeViews/ChoiceNode");

			template.CloneTree(bodyContainer);

			actorLabel = bodyContainer.Q<Label>("actor");
			actorLabel.text = Data.Actor;

			messageLabel = bodyContainer.Q<Label>("message");
			messageLabel.text = Data.Message;

			optionList = bodyContainer.Q(classes: "option-list");

			Data.Options.ForEach(AddOption);

			schedule.Execute(HandleNodeChange).Every(100);
		}

		private void AddOption(PortDefinition<BaseNode> option)
		{
			var optionElement = new ChoiceOption(option);
			optionList.Add(optionElement);
			RegisterPort(optionElement.port, option.Guid);
		}

		private void RemoveOldEntries()
		{
			var optionElements = bodyContainer.Query<ChoiceOption>().ToList();

			var optionElementsToRemove = optionElements
				.Where(x => Data.Options.ToList().All(y => y.Guid != x.portDefinition.Guid));

			var edgesToRemove = new List<GraphElement>();

			foreach (var element in optionElementsToRemove)
			{
				edgesToRemove.AddRange(element.port.connections);
				element.RemoveFromHierarchy();
			}

			if (edgesToRemove.Count > 0)
				GraphView.DeleteElements(edgesToRemove);
		}

		private void UpdateEntries()
		{
			var optionElements = bodyContainer.Query<ChoiceOption>().ToList();
			foreach (var el in optionElements)
				el.Update();
		}

		private void AddNewEntries()
		{
			var optionElements = bodyContainer.Query<ChoiceOption>().ToList();

			Data.Options
				.Where(x => optionElements.TrueForAll(y => y.portDefinition.Guid != x.Guid))
				.ToList()
				.ForEach(AddOption);
		}


		private void HandleNodeChange()
		{
			var actorName = Data.UseActorProfile ? Data.Actor :
				Data.ActorProfile != null ? Data.ActorProfile.DisplayName : "";
			
			if (actorLabel.text != actorName)
				actorLabel.text = actorName;

			if (messageLabel.text != Data.Message)
				messageLabel.text = Data.Message;

			RemoveOldEntries();
			UpdateEntries();
			AddNewEntries();
		}

		// We need to extend this method, so that when we delete the node, the edges attached to
		// each option are included in the list of elements to delete, for "graphViewChanged"
		public override void CollectElements(HashSet<GraphElement> collectedElementSet, Func<GraphElement, bool> conditionFunc)
		{
			base.CollectElements(collectedElementSet, conditionFunc);

			var choicePorts = bodyContainer.Query<Port>().ToList();
			collectedElementSet.UnionWith(choicePorts.SelectMany(port => port.connections));
		}
	}
}