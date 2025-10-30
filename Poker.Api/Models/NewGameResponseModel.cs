namespace Poker.Api.Models
{
    public class NewGameResponseModel
    {
        public NewGameResponseModel(Guid gameId)
        {
            GameId = gameId;
        }

        public Guid GameId { get; set; }
    }
}
