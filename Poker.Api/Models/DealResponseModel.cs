using PokerLogic.Games;

namespace Poker.Api.Models
{
    /// <summary>
    /// Response model for dealing cards in a poker game.
    /// </summary>
    public class DealResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DealResponseModel"/> class.
        /// </summary>
        /// <param name="gameId">The unique identifier for the game.</param>
        /// <param name="players">The players dealt in the game.</param>
        public DealResponseModel(Guid gameId, IEnumerable<Player> players)
        {
            GameId = gameId;
            Players = players;
        }

        /// <summary>
        /// The unique identifier for the game.
        /// </summary>
        public Guid GameId { get; }

        /// <summary>
        /// The players dealt in the game.
        /// </summary>
        public IEnumerable<Player> Players { get; }
    }
}