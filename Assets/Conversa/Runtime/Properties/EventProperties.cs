using System;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime.Properties
{
    [MovedFrom(true, null, "Assembly-CSharp")]
    [Serializable]
    public class EventProperty : BaseProperty
    {
        // Possible more attributes in the future, like number of arguments and such
        public EventProperty(string name) : base(name) { }
    }
}