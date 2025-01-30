using Core.Interface.Models;
using Managers;

namespace Core.Interface.Controllers
{
    public interface IPieceController
    {
        IPieceModel PieceModel { get; }
        (int x, int y) Position { get; }
        PlayerID PlayerID { get; }

        
        void Initialize(IPieceModel pieceModel, PlayerID playerID);
        void SetPosition((int x, int y) newPosition);
        
        IPieceModel GetInfo();
        void RestoreHealth(int amount);
        void TakeDamage(int damage);
        void Die();
        void Revive();
    }
}