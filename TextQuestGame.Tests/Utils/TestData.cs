using System.Collections.Generic;
using TextQuestGame;

namespace TextQuestGame.Tests
{
    public static class TestData
    {
        public static Scene CreateScene() => new Scene
        {
            Id = "test",
            Text = "Тест",
            ImagePath = "test.jpg",
            Choices = new List<Choice>
            {
                new Choice { Text = "Выбор", NextSceneId = "next" }
            }
        };

        public static GameState CreateGameState() => new GameState
        {
            CurrentSceneId = "start",
            Inventory = new List<string> { "Item1", "Item2" }
        };
    }
}
