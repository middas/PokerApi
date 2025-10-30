using PokerLogic.Games;

namespace Poker.Api.Models
{
    /// <summary>
    /// Response model for evaluating a poker game.
    /// </summary>
    public class EvaluateResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluateResponseModel"/> class.
        /// </summary>
        /// <param name="players">The players in the evaluated game.</param>
        public EvaluateResponseModel(IEnumerable<Player> players)
        {
            Players = players;
        }

        /// <summary>
        /// The players in the evaluated game.
        /// </summary>
        public IEnumerable<Player> Players { get; }
    }
}