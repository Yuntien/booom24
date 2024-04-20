using UnityEngine;

namespace Conversa.Runtime
{
    [CreateAssetMenu(fileName = "Actor", menuName = "Conversa/Actor", order = 0)]
    public class Actor : ScriptableObject
    {
        [SerializeField] private string displayName;

        public string DisplayName
        {
            get => displayName;
            set => displayName = value;
        }
    }
}