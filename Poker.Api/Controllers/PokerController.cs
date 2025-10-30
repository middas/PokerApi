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

        [HttpPut("{gameId}/Player/{playerName}")]
        [ProducesResponseType<DealResponseModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public IActionResult AddPlayer(string gameId, string playerName)
        {
            Guid guid = Guid.Parse(gameId);
            if (games.TryGetValue(guid, out IGame? game))
            {
                logger.LogInformation("Adding player {PlayerName} to game {Guid}.", playerName, guid);
                try
                {
                    game.AddPlayer(new Player(playerName));
                    return Ok();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error adding player to poker game: {Message}", ex.Message);
                    return BadRequest();
                }
            }
            else
            {
                logger.LogWarning("Game with ID {Guid} not found.", guid);
                return NotFound();
            }
        }

        [HttpGet("{gameId}/Deal")]
        [ProducesResponseType<DealResponseModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Deal(string gameId)
        {
            Guid guid = Guid.Parse(gameId);
            logger.LogInformation("Dealing a new poker hand.");

            try
            {
                if (games.TryGetValue(guid, out IGame? game))
                {
                    game.Reset();
                    IEnumerable<Player> handResults = game.Deal();
                    return Ok(new DealResponseModel(guid, handResults));
                }
                else
                {
                    logger.LogWarning("Game with ID {Guid} not found.", guid);
                    return NotFound();
                }
            }
            catch (Exception ex)
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

        [HttpGet("{gameId}/Evaluate")]
        [ProducesResponseType<EvaluateResponseModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public IActionResult Evaluate(string gameId)
        {
            Guid guid = Guid.Parse(gameId);
            if (games.TryGetValue(guid, out IGame? game))
            {
                logger.LogInformation("Evaluating poker hands for game {Guid}.", guid);
                try
                {
                    game.ValidateHands();
                    IEnumerable<Player> evaledPlayers = game.DetermineWinners();

                    return Ok(new EvaluateResponseModel(evaledPlayers));
                }
                catch (Exception ex)
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

        [HttpPost("New")]
        [ProducesResponseType<DealResponseModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public IActionResult New([FromBody][Description("The player names")] string[]? playerNames)
        {
            logger.LogInformation("Creating a new poker game.");

            try
            {
                IGame game = new FiveCardPokerGame();
                Guid guid = Guid.NewGuid();
                games[guid] = game;

                if (playerNames is not null && playerNames.Length > 0)
                {
                    foreach (string name in playerNames)
                    {
                        game.AddPlayer(new Player(name));
                    }
                }

                return Ok(new NewGameResponseModel(guid));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating new poker game: {Message}", ex.Message);
                return BadRequest(new ProblemDetails()
                {
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    Title = $"Error creating new poker game.",
                    Status = StatusCodes.Status400BadRequest,
                    Extensions = new Dictionary<string, object?>
                    {
                        {"errors", new[] { ex.Message } },
                        {"traceId", Request.Headers.TraceParent.ToString() }
                    }
                });
            }
        }

        [HttpDelete("{gameId}/Player/{playerName}")]
        [ProducesResponseType<DealResponseModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public IActionResult RemovePlayer(string gameId, string playerName)
        {
            Guid guid = Guid.Parse(gameId);
            if (games.TryGetValue(guid, out IGame? game))
            {
                logger.LogInformation("Removing player {PlayerName} from game {Guid}.", playerName, guid);
                try
                {
                    game.RemovePlayer(new Player(playerName));
                    if (game.PlayerCount < 1)
                    {
                        logger.LogInformation("No players remaining in game {Guid}. Removing game.", guid);
                        games.Remove(guid);
                    }

                    return Ok();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error removing player from poker game: {Message}", ex.Message);
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