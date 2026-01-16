using Core.Interface.Controllers;
using Core.Interface.Models;

namespace Core.Traps
{
    /// <summary>
    /// Interface for trap effects using Strategy Pattern.
    /// </summary>
    public interface ITrapEffect
    {
        /// <summary>
        /// Activates the trap effect on the given piece.
        /// </summary>
        /// <param name="piece">The piece that triggered the trap.</param>
        /// <param name="trap">The trap that was triggered.</param>
        void Activate(IPieceController piece, ITrap trap);
    }
}
