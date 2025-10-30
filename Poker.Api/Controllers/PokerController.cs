using Microsoft.AspNetCore.Mvc;
using Poker.Api.Models;
using PokerLogic.Games;
using PokerLogic.Games.Poker;
using System.ComponentModel;

namespace Poker.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PokerController : ControllerBase
    {
        private static readonly Dictionary<Guid, IGame> games = [];

        private readonly ILogger<PokerController> logger;

        public PokerController(ILogger<PokerController> logger)
        {
            this.logger = logger;
        }

        [HttpPost("Deal")]
        [ProducesResponseType<DealResponseModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public IActionResult Deal([FromBody][Description("The player names")] string[] playerNames)
        {
            logger.LogInformation("Dealing a new poker hand.");

            try
            {
                IGame game = new FiveCardPokerGame();
                Guid guid = Guid.NewGuid();
                games[guid] = game;

                IEnumerable<Player> players = playerNames.Select(name => new Player(name));
                IEnumerable<Player> handResults = game.Deal(players);

                return Ok(new DealResponseModel(guid, handResults));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error dealing poker hands: {Message}", ex.Message);
                return BadRequest(new ProblemDetails()
                {
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    Title = $"Error dealing poker hands.",
                    Status = StatusCodes.Status400BadRequest,
                    Extensions = new Dictionary<string, object?>
                    {
                        {"errors", new[] { ex.Message } },
                        {"traceId", Request.Headers.TraceParent.ToString() }
                    }
                });
            }
        }

        [HttpPut("Evaluate/{gameId}")]
        [ProducesResponseType<EvaluateResponseModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public IActionResult Evaluate(string gameId, [FromBody] IEnumerable<Player> players)
        {
            Guid guid = Guid.Parse(gameId);
            if (games.TryGetValue(guid, out IGame? game))
            {
                logger.LogInformation("Evaluating poker hands for game {Guid}.", guid);
                try
                {
                    game.ValidateHands(players);
                    IEnumerable<Player> evaledPlayers = game.DetermineWinners(players);
                    games.Remove(guid);

                    return Ok(new EvaluateResponseModel(evaledPlayers));
                }
                catch (ArgumentException ex)
                {
                    logger.LogError(ex, "Error evaluating poker hands: {Message}", ex.Message);
                    return BadRequest();
                }
            }
            else
            {
                logger.LogWarning("Game with ID {Guid} not found.", guid);
                return NotFound();
            }
        }
    }
}