using PokerLogic.Decks;

namespace PokerLogic.Tests.Decks
{
    [TestFixture]
    internal class StandardDeckTests
    {
        [Test]
        public void StandardDeck_CardsShouldBeInExpectedOrder()
        {
            // Arrange
            StandardDeck deck = new();
            Stack<Card> expectedOrder = [];
            foreach (Constants.Suit suit in Enum.GetValues<Constants.Suit>())
            {
                foreach (Constants.Rank rank in Enum.GetValues<Constants.Rank>())
                {
                    expectedOrder.Push(new Card(suit, rank));
                }
            }
            // Act
            IEnumerable<Card> actualCards = deck.GetCards();
            // Assert
            Assert.That(actualCards, Is.EqualTo(expectedOrder), "The cards should be in the expected order.");
        }

        [Test]
        public void StandardDeck_Draw_MoreThanAvailableCards_ShouldThrowException()
        {
            // Arrange
            StandardDeck deck = new();
            int drawCount = 60;
            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => deck.Draw(drawCount));
            Assert.That(ex.Message, Does.Contain("Count must be non-negative and less than or equal to the number of cards in the deck."));
        }

        [Test]
        public void StandardDeck_Draw_NegativeCount_ShouldThrowException()
        {
            // Arrange
            StandardDeck deck = new();
            int drawCount = -5;
            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => deck.Draw(drawCount));
            Assert.That(ex.Message, Does.Contain("Count must be non-negative and less than or equal to the number of cards in the deck."));
        }

        [Test]
        public void StandardDeck_Draw_ShouldReturnCorrectNumberOfCards()
        {
            // Arrange
            StandardDeck deck = new();
            int drawCount = 5;
            // Act
            IEnumerable<Card> drawnCards = deck.Draw(drawCount);
            // Assert
            Assert.That(drawnCards.Count(), Is.EqualTo(drawCount), $"Drawing {drawCount} cards should return {drawCount} cards.");
            Assert.That(deck.GetCards().Count(), Is.EqualTo(52 - drawCount), $"The deck should have {52 - drawCount} cards remaining after drawing {drawCount} cards.");
        }

        [Test]
        public void StandardDeck_Draw_ZeroCount_ShouldReturnEmptyCollection()
        {
            // Arrange
            StandardDeck deck = new();
            int drawCount = 0;
            // Act
            IEnumerable<Card> drawnCards = deck.Draw(drawCount);
            // Assert
            Assert.That(drawnCards.Count(), Is.EqualTo(0), "Drawing zero cards should return an empty collection.");
            Assert.That(deck.GetCards().Count(), Is.EqualTo(52), "The deck should still have 52 cards after drawing zero cards.");
        }

        [Test]
        public void StandardDeck_Reset_ShouldRestoreOriginalOrder()
        {
            // Arrange
            StandardDeck deck = new();
            deck.Shuffle();
            _ = deck.Draw(5);
            // Act
            deck.Reset();
            IEnumerable<Card> resetOrder = deck.GetCards();
            Stack<Card> expectedOrder = [];
            foreach (Constants.Suit suit in Enum.GetValues<Constants.Suit>())
            {
                foreach (Constants.Rank rank in Enum.GetValues<Constants.Rank>())
                {
                    expectedOrder.Push(new Card(suit, rank));
                }
            }
            // Assert
            Assert.That(resetOrder, Is.EqualTo(expectedOrder), "The order and number of cards should be restored to the original after resetting.");
        }

        [Test]
        public void StandardDeck_ShouldContain52UniqueCards()
        {
            // Arrange
            StandardDeck deck = new();
            // Act
            IEnumerable<Card> cards = deck.GetCards();
            // Assert
            Assert.That(cards.Count, Is.EqualTo(52), "The deck should contain 52 cards.");
            Assert.That(cards.Distinct().Count(), Is.EqualTo(52), "All cards in the deck should be unique.");
        }

        [Test]
        public void StandardDeck_ShouldContainAllSuitsAndRanks()
        {
            // Arrange
            StandardDeck deck = new();
            List<Constants.Suit> expectedSuits = [.. Enum.GetValues<Constants.Suit>().Cast<Constants.Suit>()];
            List<Constants.Rank> expectedRanks = [.. Enum.GetValues<Constants.Rank>().Cast<Constants.Rank>()];
            // Act
            IEnumerable<Card> cards = deck.GetCards();
            List<Constants.Suit> actualSuits = [.. cards.Select(c => c.Suit).Distinct()];
            List<Constants.Rank> actualRanks = [.. cards.Select(c => c.Rank).Distinct()];
            // Assert
            Assert.That(actualSuits.Count, Is.EqualTo(expectedSuits.Count), "The deck should contain all suits.");
            Assert.That(actualRanks.Count, Is.EqualTo(expectedRanks.Count), "The deck should contain all ranks.");
        }

        [Test]
        public void StandardDeck_Shuffle_ShouldRandomizeCardOrder()
        {
            // Arrange
            StandardDeck deck = new();
            // Capture the original order of cards
            IEnumerable<Card> originalOrder = [.. deck.GetCards()];
            // Act
            deck.Shuffle();
            IEnumerable<Card> shuffledOrder = deck.GetCards();
            // Assert
            Assert.That(shuffledOrder, Is.Not.EqualTo(originalOrder), "The order of cards should be different after shuffling.");
            Assert.That(shuffledOrder.Count(), Is.EqualTo(52), "The deck should still contain 52 cards after shuffling.");
            Assert.That(shuffledOrder.Distinct().Count(), Is.EqualTo(52), "All cards in the deck should still be unique after shuffling.");
        }
    }
}