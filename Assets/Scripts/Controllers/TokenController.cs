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
            if (Model.Health <= 0)
                Die();
        }

        public void RestoreHealth(int amount)
        {
            int newHealth = Model.Health + amount;
            Model.SetHealth(newHealth);
        }

        public void Die()
        {
            throw new System.NotImplementedException();
        }
    }
}