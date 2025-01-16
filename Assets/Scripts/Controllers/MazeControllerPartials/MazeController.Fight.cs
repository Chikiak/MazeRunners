using System;
using Core.Interfaces.Entities;

namespace Controllers
{
    public partial class MazeController
    {
        #region Fight
        private void AllFight()
        {
            var tlist = _maze.TokensMaze;
            foreach (var list in tlist)
            {
                if (list.Count == 2)
                {
                    Fight(list[0], list[1]);
                }
            }
        }
        private void Fight(ITokenController token1, ITokenController token2)
        {
            token1.TakeDamage(token2.Model.Dmg);
            if (token1.Model.Position != token2.Model.Position) return;
            token2.TakeDamage(token1.Model.Dmg);
        }

        private void TokenDead(ITokenController token)
        {
            GameManager.DeadTokens.Add(token);
            token.Die();
            OnMazeChanged();
        }
        private void HandleTokenDead(ITokenController token)
        {
            _maze.TokensMaze[token.Model.Position.Item1, token.Model.Position.Item2].Remove(token);
            token.Model.SetHealth(token.Model.MaxHealth);
            token.Revive();
            Random r = new Random();
            int pos1 = r.Next(0, _maze.Size);
            int pos2 = r.Next(0, _maze.Size);
            token.Model.SetPosition((pos1, pos2));
            _maze.TokensMaze[token.Model.Position.Item1, token.Model.Position.Item2].Add(token);
            OnMazeChanged();
        }

        #endregion
    }
}