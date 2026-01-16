using Core.Interface.Controllers;
using Core.Interface.Models;

namespace Core.Abilities
{
    /// <summary>
    /// Interface for piece abilities using Strategy Pattern.
    /// </summary>
    public interface IAbility
    {
        /// <summary>
        /// Executes the ability for the given piece.
        /// </summary>
        /// <param name="pieceController">The piece using the ability.</param>
        /// <param name="cubeModel">The cube model containing all cells.</param>
        void Execute(IPieceController pieceController, ICubeModel cubeModel);
    }
}
