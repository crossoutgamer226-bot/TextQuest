using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextQuestGame;

namespace TextQuestGame.Tests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void Scene_Creation_Works()
        {
            var scene = new Scene
            {
                Id = "test",
                Text = "Test",
                ImagePath = "test.jpg"
            };
            Assert.AreEqual("test", scene.Id);
        }

        [TestMethod]
        public void Choice_Creation_Works()
        {
            var choice = new Choice
            {
                Text = "Выбор",
                NextSceneId = "next"
            };
            Assert.AreEqual("Выбор", choice.Text);
        }

        [TestMethod]
        public void GameState_Creation_Works()
        {
            var state = new GameState();
            Assert.AreEqual("start", state.CurrentSceneId);
            Assert.IsNotNull(state.Inventory);
        }
    }
}
