using PokerLogic.Games;

namespace Poker.Api.Models
{
    public class DealResponseModel
    {
        public DealResponseModel(Guid gameId, IEnumerable<Player> players)
        {
            GameId = gameId;
            Players = players;
        }

        public Guid GameId { get; }

        public IEnumerable<Player> Players { get; }
    }
}