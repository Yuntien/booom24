namespace Conversa.Editor
{
    // This attribute is used to execute functions, right after a property is deleted.
    // It can be used, for example, to remove Property nodes of those properties
    public class DeletePropertyHandlerAttribute : System.Attribute { }
    
    // This attribute is used to execute functions, right after a property is edited.
    // It can be used, for example, to update property nodes, if a property changes
    // its name
    public class EditPropertyHandlerAttribute : System.Attribute { }
    
    public class DeleteSelectionHandlerAttribute : System.Attribute { }

}