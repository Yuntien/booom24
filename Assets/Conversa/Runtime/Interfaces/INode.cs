using Conversa.Editor;
using UnityEngine;

namespace Conversa.Runtime.Interfaces
{
    public interface INode : IIdentifiable, IPortable
    {
        public Rect NodeRect { get; set; }

        bool IsValid(Conversation conversation);
        NodePort GetNodePort(string portGuid);
    }
}