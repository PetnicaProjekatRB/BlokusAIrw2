using System;
using System.Linq;

namespace BlokusDll
{
    public interface IBlokusAi
    {
        /// <summary>
        /// Validates the AI. Should return true in order to use this AI.
        /// </summary>
        /// <returns>True if should use this AI</returns>
        bool Validate();

        /// <summary>
        /// (Re)starts the AI
        /// </summary>
        /// <param name="player">Player number for AI to play with</param>
        /// <returns>True if successful, false otherwise</returns>
        bool Start(Player player);

        /// <summary>
        /// Used to modify internal parameters of the AI
        /// </summary>
        /// <param name="name">Parameter name to modify</param>
        /// <param name="value">New value to assign</param>
        /// <returns>
        /// 0 if successful, 
        /// 1 if parameter value not correct, 
        /// 2 if parameter name not found, 
        /// 3 if forbidden,
        /// -1 for internal error/exception</returns>
        int Modify(string name, string value);

        /// <summary>
        /// Plays a move on the game board
        /// </summary>
        /// <param name="grid">Game grid to play on</param>
        /// <returns>True if move mlayed, false otherwise</returns>
        bool Play(ref GameGrid grid);

        /// <summary>
        /// Gets the current hand of the AI
        /// </summary>
        /// <returns> All the pieces in hand </returns>
        Hand GetHand();
    }
}
