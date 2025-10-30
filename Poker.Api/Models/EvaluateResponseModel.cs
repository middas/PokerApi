using PokerLogic.Games;

namespace Poker.Api.Models
{
    public class EvaluateResponseModel
    {

        public EvaluateResponseModel(IEnumerable<Player> players)
        {
            Players = players;
        }

        public IEnumerable<Player> Players { get; }
    }
}