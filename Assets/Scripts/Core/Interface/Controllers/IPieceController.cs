using Core.Interface.Models;
using Managers;

namespace Core.Interface.Controllers
{
    /// <summary>
    /// Interface for piece controllers that manage piece behavior and state.
    /// </summary>
    public interface IPieceController
    {
        /// <summary>
        /// Gets the piece model containing stats and properties.
        /// </summary>
        IPieceModel PieceModel { get; }
        
        /// <summary>
        /// Gets the current position of the piece on the board.
        /// </summary>
        (int x, int y) Position { get; }
        
        /// <summary>
        /// Gets the player ID that owns this piece.
        /// </summary>
        PlayerID PlayerID { get; }

        /// <summary>
        /// Initializes the piece with a model and player ownership.
        /// </summary>
        void Initialize(IPieceModel pieceModel, PlayerID playerID);
        
        /// <summary>
        /// Sets the position of the piece.
        /// </summary>
        void SetPosition((int x, int y) newPosition);
        
        /// <summary>
        /// Gets the piece model information.
        /// </summary>
        IPieceModel GetInfo();
        
        /// <summary>
        /// Restores health to the piece.
        /// </summary>
        void RestoreHealth(int amount);
        
        /// <summary>
        /// Applies damage to the piece.
        /// </summary>
        void TakeDamage(int damage);
        
        /// <summary>
        /// Called when the piece dies.
        /// </summary>
        void Die();
        
        /// <summary>
        /// Revives the piece with reduced health.
        /// </summary>
        void Revive();
        
        /// <summary>
        /// Checks if the piece is alive.
        /// </summary>
        bool IsAlive();
        
        /// <summary>
        /// Checks if the piece's ability is ready to use.
        /// </summary>
        bool AbilityIsReady();
        
        /// <summary>
        /// Uses the piece's special ability.
        /// </summary>
        void UseAbility();
        
        /// <summary>
        /// Reduces all cooldowns by one turn.
        /// </summary>
        void ReduceCooldowns();
    }
}