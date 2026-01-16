using Core.Abilities;
using Core.Interface.Controllers;
using Managers;
using UnityEngine;

namespace Core.Controllers
{
    public partial class CubeController
    {
        #region Abilities

        private void HandleAbilityUsed()
        {
            var pieceController = PieceManager.SelectedPiece;

            if (!pieceController.AbilityIsReady())
            {
                Debug.Log($"Ability isn't ready, ready in {pieceController.PieceModel.CurrentCooldown} turns");
                return;
            }

            // Use Strategy Pattern via AbilityFactory
            var ability = AbilityFactory.CreateAbility(pieceController.PieceModel.PieceType, _mazeGenerator);
            ability.Execute(pieceController, Model);
            
            pieceController.PieceModel.SetCurrentCooldown(pieceController.PieceModel.AbilityCooldown);
        }

        #endregion
    }
}