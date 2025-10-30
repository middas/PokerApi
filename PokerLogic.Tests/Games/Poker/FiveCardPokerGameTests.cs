using PokerLogic.Games;
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
            var players = new List<Player>();
            // Act & Assert
            Assert.Throws<ArgumentException>(() => game.Deal(players), "Dealing with empty players should throw ArgumentException.");
        }

        [Test]
        public void Deal_NullPlayers_ThrowsArgumentNullException()
        {
            // Arrange
            var game = new FiveCardPokerGame();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => game.Deal(null!), "Dealing with null players should throw ArgumentNullException.");
        }

        [Test]
        public void Deal_OnePlayer_ShouldDealFiveCards()
        {
            // Arrange
            var game = new FiveCardPokerGame();
            var player = new Player("Justin");
            var players = new List<Player> { player };
            // Act
            game.Deal(players);
            // Assert
            Assert.That(player.Hand.Cards.Count, Is.EqualTo(5), "Player should have 5 cards after dealing.");
        }

        [Test]
        public void Deal_TwoPlayers_ShouldDealFiveCardsPerPlayer()
        {
            // Arrange
            var game = new FiveCardPokerGame();
            var player = new Player("Justin");
            var player2 = new Player("Leah");
            Player[] players = [player, player2];
            // Act
            game.Deal(players);
            // Assert
            Assert.That(player.Hand.Cards.Count, Is.EqualTo(5), "Player should have 5 cards after dealing.");
            Assert.That(player2.Hand.Cards.Count, Is.EqualTo(5), "Player2 should have 5 cards after dealing.");
        }

        [Test]
        public void DetermineWinners_TwoPlayers_ShouldReturnAtLeastOneWinner()
        {
            // Arrange
            var game = new FiveCardPokerGame();
            var player = new Player("Justin");
            var player2 = new Player("Leah");
            Player[] players = [player, player2];
            // Act
            game.Deal(players);
            game.ValidateHands(players);
            Player[] result = [.. game.DetermineWinners(players)];
            // Assert
            Assert.That(player.Hand.Cards.Count, Is.EqualTo(5), "Player should have 5 cards after dealing.");
            Assert.That(player2.Hand.Cards.Count, Is.EqualTo(5), "Player2 should have 5 cards after dealing.");
            Assert.That(result, Has.Length.AtLeast(1));
            Assert.That(result[0].HasValidHand, Is.True, "Winner's hand should be valid.");
            Assert.That(result[0].HandRank.HasValue, Is.True, "Winner's hand rank should be set.");
        }

        [Test]
        public void ValidateHands_OnePlayerWithBadCard_ShouldReturnHandInvalid()
        {
            var game = new FiveCardPokerGame();
            var player = new Player("Justin");
            var players = new List<Player> { player };
            // Act
            game.Deal(players);
            player.GiveCards([new Card(Constants.Suit.Clubs, Constants.Rank.Ace)]);
            game.ValidateHands(players);
            // Assert
            Assert.That(player.Hand.Cards.Count, Is.EqualTo(6), "Player should have 6 cards after dealing and extra card.");
            Assert.That(player.HasValidHand, Is.False, "Player's hand should not be valid after validation.");
        }

        [Test]
        public void ValidateHands_OnePlayerWithDuplicateCard_ShouldReturnHandInvalid()
        {
            var game = new FiveCardPokerGame();
            var player = new Player("Justin");
            var players = new List<Player> { player };
            // Act
            game.Deal(players);
            player.GiveCards([player.Hand.Cards.First()]);
            game.ValidateHands(players);
            // Assert
            Assert.That(player.Hand.Cards.Count, Is.EqualTo(6), "Player should have 6 cards after dealing and extra card.");
            Assert.That(player.HasValidHand, Is.False, "Player's hand should not be valid after validation.");
        }

        [Test]
        public void ValidateHands_TwoPlayers_ShouldReturnBothHandsValid()
        {
            // Arrange
            var game = new FiveCardPokerGame();
            var player = new Player("Justin");
            var player2 = new Player("Leah");
            Player[] players = [player, player2];
            // Act
            game.Deal(players);
            game.ValidateHands(players);
            // Assert
            Assert.That(player.Hand.Cards.Count, Is.EqualTo(5), "Player should have 5 cards after dealing.");
            Assert.That(player2.Hand.Cards.Count, Is.EqualTo(5), "Player2 should have 5 cards after dealing.");
            Assert.That(player.HasValidHand, Is.True, "Player's hand should be valid after validation.");
            Assert.That(player2.HasValidHand, Is.True, "Player2's hand should be valid after validation.");
        }

        [Test]
        public void ValidateHands_TwoPlayersThatShareCard_ShouldReturnBothHandsInvalid()
        {
            var game = new FiveCardPokerGame();
            var player = new Player("Justin");
            var player2 = new Player("Leah");
            var players = new List<Player> { player, player2 };
            // Act
            game.Deal(players);
            player.GiveCards([player2.Hand.Cards.First()]);
            game.ValidateHands(players);
            // Assert
            Assert.That(player.Hand.Cards.Count, Is.EqualTo(6), "Player should have 6 cards after dealing and extra card.");
            Assert.That(player2.Hand.Cards.Count, Is.EqualTo(5), "Player2 should have 5 cards after dealing.");
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
            Player[] players = [player, player2];
            // Act
            game.Deal(players);
            game.Reset();
            game.ValidateHands(players);
            // Assert
            Assert.That(player.Hand.Cards.Count, Is.EqualTo(5), "Player should have 5 cards after dealing.");
            Assert.That(player2.Hand.Cards.Count, Is.EqualTo(5), "Player2 should have 5 cards after dealing.");
            Assert.That(player.HasValidHand, Is.False, "Player's hand should not be valid after validation.");
            Assert.That(player2.HasValidHand, Is.False, "Player2's hand should not be valid after validation.");
        }
    }
}