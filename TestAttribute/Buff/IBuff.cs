namespace TestMap
{
    public interface IBuff
    {
        int BuffId { get; }
        int BuffType { get; }
        int BuffOrder { get; }
        
        void OnBuffAdded();
        void OnBuffRemoved();
    }
}