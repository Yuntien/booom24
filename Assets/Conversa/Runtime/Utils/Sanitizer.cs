using System.Linq;
using Conversa.Runtime;
using Conversa.Runtime.Interfaces;
using Conversa.Runtime.Nodes;
using Conversa.Runtime.Nodes.PropertyNodes;

namespace Conversa.Editor.Utils
{
    // This class is meant to fix stuff like handling deprecated nodes
    public static class Sanitizer
    {
        // BooleanNode is deprecated, and should be replaced by PropertyNode 
        public static INode ReplaceBooleanNodes(INode inode)
        {
            if (!(inode is BooleanNode booleanNode)) return inode;
			
            var sanitizedNode = new PropertyNode(booleanNode.PropertyGuid, booleanNode.Name)
            {
                NodeRect = booleanNode.NodeRect,
                Guid = booleanNode.Guid
            };
            return sanitizedNode;
        }


        // Older jump nodes used to reference bookmarks by their name, and not their GUID
        // This methods makes sure that those nodes get linked by GUID
        public static void UpdateJumpNodes(Conversation conversation)
        {
            var bookmarks = conversation.AllNodes.OfType<BookmarkNode>();
            var jumps = conversation
                .UserNodes
                .OfType<BookmarkJumpNode>()
                .Where(jump => string.IsNullOrEmpty(jump.BookmarkGuid));

            bookmarks.ToList().ForEach(bookmark => jumps
                .Where(jump => jump.BookmarkName == bookmark.Name)
                .ToList()
                .ForEach(jump => jump.BookmarkGuid = bookmark.Guid)
            );
        }
    }
}