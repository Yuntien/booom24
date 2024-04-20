namespace Conversa.Runtime.Interfaces
{
    public interface IValueProperty
    {
        string Name { get; }
        string Guid { get; }
        System.Type GetValueType();
        object GetValueObject();
        void Reset();
    }
}