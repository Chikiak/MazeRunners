using Core.Interfaces.Entities;

namespace Controllers
{
    public class TokenController : ITokenController
    {
        public TokenController(IToken model, Players playerID)
        {
            _model = model;
            PLAYER_ID = playerID;
        }

        protected IToken _model;
        public IToken Model => _model;
        
        protected ITokenView _view;
        public ITokenView View => _view;

        protected Players PLAYER_ID;
        public Players PlayerID => PLAYER_ID;
        
        protected bool _isAlive = true;
        public bool IsAlive => _isAlive;
        
        
        public void UseAbility()
        {
            GameManager.AbilityUsed.Invoke(this);
        }

        public void SetView(ITokenView newView)
        {
            _view = newView;
        }

        public void TakeDamage(int damage)
        {
            int newHealth = Model.Health - damage;
            Model.SetHealth(newHealth);
            GameManager.OnShowInfo?.Invoke(_model);
            if (Model.Health <= 0)
                Die();
        }

        public void RestoreHealth(int amount)
        {
            int newHealth = Model.Health + amount;
            Model.SetHealth(newHealth);
            GameManager.OnShowInfo?.Invoke(_model);
        }

        public void Die()
        {
            _isAlive = false;
            GameManager.DeadTokens.Add(this);
        }

        public void Revive()
        {
            Model.SetHealth(Model.MaxHealth / 2);
            _isAlive = true;
            GameManager.DeadTokens.Remove(this);
        }
    }
}