using Conversa.Runtime.Interfaces;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

namespace Conversa.Editor.Interfaces
{
    public interface INodeView : IIdentifiable
    {
        string GetPortId(Port port);
        UnityEvent OnBeforeChange { get; }

        bool GetPort(string guid, out Port port);
        void UpdateRect();
        void UpdateData();
        void SetRealPosition(Vector2 position);
        INode GetNode();
    }
}