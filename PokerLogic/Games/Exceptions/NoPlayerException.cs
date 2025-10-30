namespace PokerLogic.Games.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when no players are available in the game.
    /// </summary>
    /// <remarks>This exception is typically used to indicate that an operation requiring at least one player
    /// cannot proceed because no players are currently available.</remarks>
    [Serializable]
    public sealed class NoPlayerException : Exception
    {
        /// <summary>
        /// Represents an exception that is thrown when no players are available in the game.
        /// </summary>
        /// <remarks>This exception is typically used to indicate that an operation requiring at least one
        /// player  cannot proceed because no players are currently available.</remarks>
        public NoPlayerException() : base("No players are available in the game.")
        {
        }
    }
}
