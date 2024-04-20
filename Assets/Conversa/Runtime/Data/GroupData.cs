using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace Conversa.Runtime
{
    [MovedFrom(true, null, "Assembly-CSharp")]
    [Serializable]
    public class GroupData: ISerializationCallbackReceiver
    {
        [FormerlySerializedAs("id")]
        [SerializeField] private string guid;
        [SerializeField] private string title;
        [SerializeField] private Vector2 position;
        [SerializeField] private List<string> elements;

        // Properties
        
        public string Guid => guid;
        
        public HashSet<string> Elements { get; private set; } = new HashSet<string>();
        
        public Vector2 Position
        {
            get => position;
            set => position = value;
        }
        
        public string Title
        {
            get => title;
            set => title = value;
        }
        
        // Methods

        public GroupData(string title)
        {
            guid = System.Guid.NewGuid().ToString();
            Title = title;
            elements = new List<string>();
        }

        // ISerializationCallbackReceiver
        
        public void OnBeforeSerialize() => elements = Elements.ToList();

        public void OnAfterDeserialize() => Elements = new HashSet<string>(elements);
    }
}