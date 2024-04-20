namespace Conversa.Runtime.Interfaces
{
	public interface IValueNode
	{
		T GetValue<T>(string portGuid, Conversation conversation);
	}
}