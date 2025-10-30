namespace PokerLogic.Decks
{
    /// <summary>
    /// Represents a collection of cards that can be drawn, shuffled, and reset.
    /// </summary>
    /// <remarks>The <see cref="IDeck"/> interface provides methods to manipulate a deck of cards, including
    /// drawing a specified number of cards, retrieving the remaining cards, shuffling the deck, and resetting it to its
    /// initial state. It is designed for use in card games or any application requiring card deck management.</remarks>
    public interface IDeck
    {
        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Draws a specified number of cards from the deck.
        /// </summary>
        /// <param name="count">The number of cards to draw. Must be a positive integer.</param>
        /// <returns>An <see cref="IEnumerable{Card}"/> containing the drawn cards. The collection will contain exactly <paramref
        /// name="count"/> cards if available; otherwise, it will contain all remaining cards.</returns>
        IEnumerable<Card> Draw(int count);

        /// <summary>
        /// Retrieves the cards remaining in the deck
        /// </summary>
        /// <returns>An enumerable collection of <see cref="Card"/> objects representing the available cards.</returns>
        IEnumerable<Card> GetCards();

        /// <summary>
        /// Resets the state of the object to its initial configuration.
        /// </summary>
        /// <remarks>This method reinitializes the object's state, clearing any temporary data or
        /// settings. It should be used when a fresh start is needed without creating a new instance.</remarks>
        void Reset();

        /// <summary>
        /// Randomizes the order of elements in the collection.
        /// </summary>
        /// <remarks>This method modifies the collection in place, altering the sequence of its elements.
        /// It is useful for scenarios where a non-deterministic order is required, such as in games or randomized
        /// testing.</remarks>
        void Shuffle();
    }
}