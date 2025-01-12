namespace Core.Interfaces.Entities
{
    public interface IDamageable
    {
        int Health { get; }
        int MaxHealth { get; }
        void SetHealth(int health);
        void SetMaxHealth(int maxHealth);
    }
}