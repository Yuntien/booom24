using Conversa.Runtime;

namespace Conversa.Editor
{
    public class RandomFlowNodeView : BaseNodeView<RandomFlowNode>
    {
        protected override string Title => "Random";

        // Constructors

        public RandomFlowNodeView(Conversation conversation) : base(new RandomFlowNode(), conversation) { }

        public RandomFlowNodeView(RandomFlowNode data, Conversation conversation) : base(data, conversation) { }

        // Methods

        protected override void SetBody() { }
    }
}