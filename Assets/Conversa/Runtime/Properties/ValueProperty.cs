using System;
using Conversa.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime.Properties
{
    [MovedFrom(true, null, "Assembly-CSharp")]
    [Serializable]
    public abstract class ValueProperty<T> : BaseProperty, IValueProperty
    {
        [SerializeField] private T value;
        [SerializeField] private T initialValue;

        public T Value
        {
            get => value;
            set => this.value = value;
        }

        public T InitialValue
        {
            get => initialValue;
            set => initialValue = value;
        }

        public object GetValueObject() => value;

        protected ValueProperty(string name) : base(name) { }

        public Type GetValueType() => typeof(T);

        public void Reset() => Value = InitialValue;
    }
}