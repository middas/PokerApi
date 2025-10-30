namespace PokerLogic
{
    /// <summary>
    /// Represents a hand of playing cards.
    /// </summary>
    /// <remarks>This class is used to manage a collection of cards, typically in a card game
    /// context.</remarks>
    public sealed class Hand
    {
        /// <summary>
        /// Gets the collection of cards.
        /// </summary>
        public List<Card> Cards { get; } = [];

        /// <summary>
        /// Resets the hand by clearing all cards.
        /// </summary>
        public void Reset()
        {
            Cards.Clear();
        }
    }
}