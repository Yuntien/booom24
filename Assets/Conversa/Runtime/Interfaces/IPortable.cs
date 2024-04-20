namespace Conversa.Runtime
{
    public interface IPortable
    {
        bool ContainsPort(string portGuid, Flow flow);
    }
}