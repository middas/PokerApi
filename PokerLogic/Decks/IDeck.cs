namespace PokerLogic.Decks
{
    public interface IDeck
    {
        IEnumerable<Card> Draw(int count);

        IEnumerable<Card> GetCards();

        void Reset();

        void Shuffle();
    }
}