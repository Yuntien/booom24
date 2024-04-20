using Conversa.Runtime;
using UnityEngine;

namespace Conversa.Demo.Scripts
{
    [CreateAssetMenu(fileName = "Actor", menuName = "Conversa Demo/Actor with avatar", order = 0)]
    public class AvatarActor : Actor
    {
        [SerializeField] private Sprite avatar;
        public Sprite Avatar => avatar;
    }
}