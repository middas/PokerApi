using static PokerLogic.Constants;

namespace PokerLogic.Decks
{
    public class StandardDeck : IDeck
    {
        // Random number generator for shuffling static to ensure rapid calls produce different results
        private static Random rng = new();

        private Stack<Card> cards = [];

        public StandardDeck()
        {
            Reset();
        }

        public IEnumerable<Card> Draw(int count)
        {
            if (count < 0 || count > cards.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be non-negative and less than or equal to the number of cards in the deck.");
            }

            List<Card> drawnCards = [];
            for (int i = 0; i < count; i++)
            {
                drawnCards.Add(cards.Pop());
            }

            return drawnCards;
        }

        public IEnumerable<Card> GetCards()
        {
            return cards;
        }

        public void Reset()
        {
            cards.Clear();
            foreach (Suit suit in Enum.GetValues<Suit>())
            {
                foreach (Rank rank in Enum.GetValues<Rank>())
                {
                    cards.Push(new Card(suit, rank));
                }
            }
        }

        public void Shuffle()
        {
            List<Card> cards = [.. this.cards];
            this.cards.Clear();

            while (cards.Count > 0)
            {
                int index = rng.Next(0, cards.Count);
                this.cards.Push(cards[index]);
                cards.RemoveAt(index);
            }
        }
    }
}