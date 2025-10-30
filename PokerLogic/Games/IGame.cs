namespace PokerLogic.Games
{
    /// <summary>
    /// Represents a game interface that provides methods for dealing hands, determining winners, resetting the game,
    /// and validating hands.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Gets the number of players currently in the game.
        /// </summary>
        int PlayerCount { get; }

        /// <summary>
        /// Adds a player to the game.
        /// </summary>
        /// <param name="player">The player to add. Cannot be null.</param>
        void AddPlayer(Player player);

        /// <summary>
        /// Deals a hand of cards to each player in the specified collection.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="Player"/> objects, each representing a player with a dealt <see cref="Hand"/>.</returns>
        IEnumerable<Player> Deal();

        /// <summary>
        /// Determines the winners from a collection of players based on their scores.
        /// </summary>
        /// <remarks>The method evaluates each player's score to determine the winners.</remarks>
        /// <returns>An enumerable collection of players with their hand's <see cref="Constants.HandRank"/> and if
        /// they are the winner of the hand</returns>
        IEnumerable<Player> DetermineWinners();

        /// <summary>
        /// Removes the specified player from the game.
        /// </summary>
        /// <remarks>If the specified player is not part of the game, this method has no effect.</remarks>
        /// <param name="player">The player to remove. Cannot be null.</param>
        void RemovePlayer(Player player);

        /// <summary>
        /// Resets the state of the object to its initial configuration.
        /// </summary>
        /// <remarks>This method reinitializes the object's state, clearing any temporary data or
        /// settings. It should be used when a fresh start is needed without creating a new instance.</remarks>
        void Reset();

        /// <summary>
        /// Validates the hands of the given players and identifies those with valid hands.
        /// </summary>
        void ValidateHands();
    }
}