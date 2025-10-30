using PokerLogic.Decks;
using PokerLogic.Games;
using PokerLogic.Games.Exceptions;
using PokerLogic.Games.Poker;

namespace PokerLogic.Tests.Games.Poker
{
    [TestFixture]
    internal class FiveCardPokerGameTests
    {
        [Test]
        public void Deal_EmptyPlayers_ThrowsArgumentException()
        {
            // Arrange
            var game = new FiveCardPokerGame();
            // Act & Assert
            Assert.Throws<NoPlayerException>(() => game.Deal(), "Dealing with empty players should throw ArgumentException.");
        }

        [Test]
        public void Deal_OnePlayer_ShouldDealFiveCards()
        {
            // Arrange
            var game = new FiveCardPokerGame();
            var player = new Player("Justin");
            // Act
            game.AddPlayer(player);
            game.Deal();
            // Assert
            Assert.That(player.Hand.Count, Is.EqualTo(5), "Player should have 5 cards after dealing.");
        }

        [Test]
        public void Deal_TwoPlayers_ShouldDealFiveCardsPerPlayer()
        {
            // Arrange
            var game = new FiveCardPokerGame();
            var player = new Player("Justin");
            var player2 = new Player("Leah");
            // Act
            game.AddPlayer(player);
            game.AddPlayer(player2);
            game.Deal();
            // Assert
            Assert.That(player.Hand.Count, Is.EqualTo(5), "Player should have 5 cards after dealing.");
            Assert.That(player2.Hand.Count, Is.EqualTo(5), "Player2 should have 5 cards after dealing.");
        }

        [Test]
        public void DetermineWinners_TwoPlayers_RemovePlayerTwo_ShouldReturnOneWinner()
        {
            // Arrange
            var game = new FiveCardPokerGame();
            var player = new Player("Justin");
            var player2 = new Player("Leah");
            // Act
            game.AddPlayer(player);
            game.AddPlayer(player2);
            game.Deal();
            game.RemovePlayer(player2);
            game.ValidateHands();
            Player[] result = [.. game.DetermineWinners()];
            // Assert
            Assert.That(player.Hand.Count, Is.EqualTo(5), "Player should have 5 cards after dealing.");
            Assert.That(player2.Hand.Count, Is.EqualTo(5), "Player2 should have 5 cards after dealing.");
            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0].HasValidHand, Is.True, "Winner's hand should be valid.");
            Assert.That(result[0].HandRank.HasValue, Is.True, "Winner's hand rank should be set.");
        }

        [Test]
        public void DetermineWinners_TwoPlayers_ShouldReturnAtLeastOneWinner()
        {
            // Arrange
            var game = new FiveCardPokerGame();
            var player = new Player("Justin");
            var player2 = new Player("Leah");
            // Act
            game.AddPlayer(player);
            game.AddPlayer(player2);
            game.Deal();
            game.ValidateHands();
            Player[] result = [.. game.DetermineWinners()];
            // Assert
            Assert.That(player.Hand.Count, Is.EqualTo(5), "Player should have 5 cards after dealing.");
            Assert.That(player2.Hand.Count, Is.EqualTo(5), "Player2 should have 5 cards after dealing.");
            Assert.That(result, Has.Length.AtLeast(1));
            Assert.That(result[0].HasValidHand, Is.True, "Winner's hand should be valid.");
            Assert.That(result[0].HandRank.HasValue, Is.True, "Winner's hand rank should be set.");
        }

        [Test]
        public void ValidateHands_OnePlayerWithBadCard_ShouldReturnHandInvalid()
        {
            var game = new FiveCardPokerGame();
            var player = new Player("Justin");
            // Act
            game.AddPlayer(player);
            game.Deal();
            player.Hand.Add(new Card(Constants.Suit.Clubs, Constants.Rank.Ace));
            game.ValidateHands();
            // Assert
            Assert.That(player.Hand.Count, Is.EqualTo(6), "Player should have 6 cards after dealing and extra card.");
            Assert.That(player.HasValidHand, Is.False, "Player's hand should not be valid after validation.");
        }

        [Test]
        public void ValidateHands_OnePlayerWithDuplicateCard_ShouldReturnHandInvalid()
        {
            var game = new FiveCardPokerGame();
            var player = new Player("Justin");
            // Act
            game.AddPlayer(player);
            game.Deal();
            player.Hand.Add(player.Hand.First());
            game.ValidateHands();
            // Assert
            Assert.That(player.Hand.Count, Is.EqualTo(6), "Player should have 6 cards after dealing and extra card.");
            Assert.That(player.HasValidHand, Is.False, "Player's hand should not be valid after validation.");
        }

        [Test]
        public void ValidateHands_TwoPlayers_ShouldReturnBothHandsValid()
        {
            // Arrange
            var game = new FiveCardPokerGame();
            var player = new Player("Justin");
            var player2 = new Player("Leah");
            // Act
            game.AddPlayer(player);
            game.AddPlayer(player2);
            game.Deal();
            game.ValidateHands();
            // Assert
            Assert.That(player.Hand.Count, Is.EqualTo(5), "Player should have 5 cards after dealing.");
            Assert.That(player2.Hand.Count, Is.EqualTo(5), "Player2 should have 5 cards after dealing.");
            Assert.That(player.HasValidHand, Is.True, "Player's hand should be valid after validation.");
            Assert.That(player2.HasValidHand, Is.True, "Player2's hand should be valid after validation.");
        }

        [Test]
        public void ValidateHands_TwoPlayersThatShareCard_ShouldReturnBothHandsInvalid()
        {
            var game = new FiveCardPokerGame();
            var player = new Player("Justin");
            var player2 = new Player("Leah");
            // Act
            game.AddPlayer(player);
            game.AddPlayer(player2);
            game.Deal();
            player.Hand.Add(player2.Hand.First());
            game.ValidateHands();
            // Assert
            Assert.That(player.Hand.Count, Is.EqualTo(6), "Player should have 6 cards after dealing and extra card.");
            Assert.That(player2.Hand.Count, Is.EqualTo(5), "Player2 should have 5 cards after dealing.");
            Assert.That(player.HasValidHand, Is.False, "Player's hand should not be valid after validation.");
            Assert.That(player2.HasValidHand, Is.False, "Player2's hand should not be valid after validation.");
        }

        [Test]
        public void ValidateHands_TwoPlayersWithReset_ShouldReturnBothHandsInvalid()
        {
            // Arrange
            var game = new FiveCardPokerGame();
            var player = new Player("Justin");
            var player2 = new Player("Leah");
            // Act
            game.AddPlayer(player);
            game.AddPlayer(player2);
            game.Deal();
            game.Reset();
            game.ValidateHands();
            // Assert
            Assert.That(player.Hand.Count, Is.EqualTo(5), "Player should have 5 cards after dealing.");
            Assert.That(player2.Hand.Count, Is.EqualTo(5), "Player2 should have 5 cards after dealing.");
            Assert.That(player.HasValidHand, Is.False, "Player's hand should not be valid after validation.");
            Assert.That(player2.HasValidHand, Is.False, "Player2's hand should not be valid after validation.");
        }
    }
}