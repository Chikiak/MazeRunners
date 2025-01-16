using System;
using System.Collections.Generic;
using Core.Interfaces;
using Core.Interfaces.Entities;
using UnityEngine;

namespace Controllers
{
    public partial class MazeController
    {
        #region Abilities
        private void HandleAblityUsed(ITokenController tokenController)
        {
            if (tokenController == null) return;

            if (!tokenController.Model.AbilityIsReady)
            {
                Debug.Log($"Ability isnt ready, ready in {tokenController.Model.CurrentCooldown} turns");
                return;
            }

            HealerAbility(tokenController);
        }
        private void HealerAbility(ITokenController tokenController)
        {
            if (tokenController.Model.Name is TokensNames.Healer)
            {
                var model = tokenController.Model;
                List<ICell> cells = GetCellsInRange(model.SpecialRangeType, model.SpecialRange, model.Position);

                foreach (var cell in cells)
                {
                    if (cell.Tokens != null)
                    {
                        var tokenList = _maze.TokensMaze[cell.Position.Item1, cell.Position.Item2];
                        cell.ClearTokens();
                        foreach (var token in tokenList)
                        {
                            token.RestoreHealth(20);
                            cell.AddToken(token);
                        }
                    }
                }

                tokenController.Model.SetAbilityIsReady(false);
            }
            else
            {
                DestroyerAbility(tokenController);
            }
        }
        private void DestroyerAbility(ITokenController tokenController)
        {
            if (tokenController.Model.Name is TokensNames.Destroyer)
            {
                var model = tokenController.Model;
                List<ICell> cells = GetCellsInRange(model.SpecialRangeType, model.SpecialRange, model.Position);

                foreach (var cell in cells)
                {
                    foreach (var d in Enum.GetValues(typeof(Directions)))
                    {
                        cell.SetWall((Directions)d, false);
                    }

                    if (cell.Position.Item1 == 0)
                    {
                        cell.SetWall(Directions.Left, true);
                    }
                    else if (cell.Position.Item1 == _maze.Size - 1)
                    {
                        cell.SetWall(Directions.Right, true);
                    }

                    if (cell.Position.Item2 == 0)
                    {
                        cell.SetWall(Directions.Up, true);
                    }
                    else if (cell.Position.Item2 == _maze.Size - 1)
                    {
                        cell.SetWall(Directions.Down, true);
                    }
                }

                tokenController.Model.SetAbilityIsReady(false);
                OnMazeChanged();
            }
        }

        #endregion
    }
}