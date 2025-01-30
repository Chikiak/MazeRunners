using Core.Interface.Controllers;
using Core.Interface.Models;
using Managers;

namespace Core.Controllers
{
    public class PieceController : IPieceController
    {
        #region Properties
        
        private IPieceModel _pieceModel;
        public IPieceModel PieceModel => _pieceModel;
        private (int x, int y) _position;
        public (int x, int y) Position => _position;
        private PlayerID _playerID;
        public PlayerID PlayerID => _playerID;
        
        #endregion
        
        #region Methods
        
        public void Initialize(IPieceModel pieceModel, PlayerID playerID)
        {
            _pieceModel = pieceModel;
            _playerID = playerID;
        }

        public void SetPosition((int x, int y) newPosition)
        {
            _position = newPosition;
        }

        public IPieceModel GetInfo()
        {
            return _pieceModel;
        }

        public void RestoreHealth(int amount)
        {
            int newHealth = _pieceModel.Health + amount;
            _pieceModel.SetHealth(newHealth);
        }

        public void TakeDamage(int damage)
        {
            int newHealth = _pieceModel.Health - damage;
            _pieceModel.SetHealth(newHealth);
        }

        public void Die()
        {
            throw new System.NotImplementedException();
        }

        public void Revive()
        {
            throw new System.NotImplementedException();
        }
        
        #endregion
    }
}