namespace Core.Interfaces.Entities
{
    public interface IMovable
    {
        (int, int) Position { get; }
        int Speed { get; }
        int RemainingMovs { get; }
        
        void SetPosition((int, int) newPosition);
        void SetSpeed(int newSpeed);
        void SetRemainingMovs(int newRemainingMovs);
    }
}