using System;
using System.Collections.Generic;
using System.Linq;
using Conversa.Runtime.Interfaces;
using UnityEngine;

namespace Conversa.Runtime
{
    [Serializable]
    public class LinearDialogueData : IEventNode
    {
        [SerializeField] private string guid;
        [SerializeField] private Vector2 position;
        [SerializeReference] private List<string> blocks = new List<string>();
        
        // Properties

        public string Guid
        {
            get => guid;
            set => guid = value;
        }

        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        public List<string> Blocks => blocks;

        public Rect NodeRect { get; set; }

        // Methods
        
        public LinearDialogueData(Vector2 position)
        {
            Guid = System.Guid.NewGuid().ToString();
            this.position = position;
        }

        public bool ContainsPort(string portGuid, Flow flow)=>
            (portGuid == "previous" && flow == Flow.In) || (portGuid == "next" && flow == Flow.Out);

        public bool IsValid(Conversation conversation) => true;

        public NodePort GetNodePort(string portGuid) => new NodePort(Guid, portGuid);
        
        public void Process(Conversation conversation, ConversationEvents conversationEvents)
        {
            var blockList = conversation.GetNodesFromStack(Guid);
            var firstBlockNode = blockList.FirstOrDefault();
            conversation.Process(firstBlockNode, conversationEvents);
        }

        public INode NextNode(Conversation conversation)
        {
            var nodePort = GetNodePort("next");
            var oppositeNodes = conversation.GetOppositeNodes(nodePort).ToList();
            var firstNode = oppositeNodes.FirstOrDefault(); 
            return firstNode;
        }
    }
}