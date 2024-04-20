using Conversa.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public class ConversaNote : StickyNote, IGroupable
    {
        private readonly StickyNoteData data;

        public string Id => data.Guid;

        public ConversaNote(StickyNoteData data)
        {
            fontSize = StickyNoteFontSize.Small;
            capabilities = Capabilities.Movable | Capabilities.Deletable | Capabilities.Ascendable | Capabilities.Selectable | Capabilities.Copiable | Capabilities.Groupable;
            this.data = data;
            Setup();
        }

        private void Setup()
        {
            title = data.Title;
            contents = data.Contents;
            SetPosition(data.Rect);
            this.Q<TextField>("title-field").RegisterCallback<BlurEvent>(HandleUpdateNote);
            this.Q<TextField>("contents-field").RegisterCallback<BlurEvent>(HandleUpdateNote);
        }

        private void HandleUpdateNote(BlurEvent e)
        {
            data.Title = title;
            data.Contents = contents;
        }

        public override void OnResized()
        {
            base.OnResized();
            UpdateData();
        }

        public void UpdateData()
        {
            data.Rect = GetPosition();
        }
    }
}