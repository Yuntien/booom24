using UnityEditor;
using UnityEngine;

namespace Conversa.Editor
{
    [FilePath("ProjectSettings/ConversaSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class ConversaSettings : ScriptableSingleton<ConversaSettings>
    {
        [SerializeField] private bool useActorProfileByDefault;
        [SerializeField] private bool stopEventsByDefault;
        
        public bool UseActorProfileByDefault
        {
            get => useActorProfileByDefault;
            set
            {
                useActorProfileByDefault = value;
                Save(true);
            }
        }
        
        public bool StopEventsByDefault
        {
            get => stopEventsByDefault;
            set
            {
                stopEventsByDefault = value;
                Save(true);
            }
        }
    }
}