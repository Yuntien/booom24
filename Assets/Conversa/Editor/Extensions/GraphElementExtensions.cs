using UnityEditor.Experimental.GraphView;

namespace Conversa.Editor.Extensions
{
    public static class GraphElementExtensions
    {
        public static void SetPermanent(this GraphElement element)
        {
            element.capabilities &= ~Capabilities.Deletable;
            element.capabilities &= ~Capabilities.Copiable;
            element.capabilities &= ~Capabilities.Renamable;
            element.capabilities &= ~Capabilities.Resizable;
        }
    }
}