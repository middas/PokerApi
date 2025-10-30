namespace PokerLogic.Games
{
    /// <summary>
    /// Represents a game interface that provides methods for dealing hands, determining winners, resetting the game,
    /// and validating hands.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Deals a hand of cards to each player in the specified collection.
        /// </summary>
        /// <param name="players">A collection of players to whom hands will be dealt. Cannot be null or empty.</param>
        /// <returns>An enumerable collection of <see cref="Player"/> objects, each representing a player with a dealt <see cref="Hand"/>.</returns>
        IEnumerable<Player> Deal(IEnumerable<Player> players);

        /// <summary>
        /// Determines the winners from a collection of players based on their scores.
        /// </summary>
        /// <remarks>The method evaluates each player's score to determine the winners. Ensure that the
        /// players collection is not null and contains valid player objects.</remarks>
        /// <param name="players">A collection of players to evaluate. Each player must have a valid score.</param>
        /// <returns>An enumerable collection of players with their hand's <see cref="Constants.HandRank"/> and if 
        /// they are the winner of the hand</returns>
        IEnumerable<Player> DetermineWinners(IEnumerable<Player> players);

        /// <summary>
        /// Resets the state of the object to its initial configuration.
        /// </summary>
        /// <remarks>This method reinitializes the object's state, clearing any temporary data or
        /// settings. It should be used when a fresh start is needed without creating a new instance.</remarks>
        void Reset();

        /// <summary>
        /// Validates the hands of the given players and identifies those with valid hands.
        /// </summary>
        /// <param name="players">A collection of players whose hands are to be validated.</param>
        void ValidateHands(IEnumerable<Player> players);
    }
}