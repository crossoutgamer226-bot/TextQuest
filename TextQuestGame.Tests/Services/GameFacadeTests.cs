using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TextQuestGame.Model.Services;
using System.Collections.Generic;
using TextQuestGame;

namespace TextQuestGame.Tests.Services
{
    [TestClass]
    public class GameFacadeTests
    {
        private Mock<IGameStateService> _stateMock;
        private Mock<ISceneService> _sceneMock;
        private Mock<IChoiceService> _choiceMock;
        private Mock<ISaveLoadService> _saveLoadMock;
        private GameFacade _facade;

        [TestInitialize]
        public void Setup()
        {
            _stateMock = new Mock<IGameStateService>();
            _sceneMock = new Mock<ISceneService>();
            _choiceMock = new Mock<IChoiceService>();
            _saveLoadMock = new Mock<ISaveLoadService>();

            _facade = new GameFacade(
                _stateMock.Object,
                _sceneMock.Object,
                _choiceMock.Object,
                _saveLoadMock.Object
            );
        }

        [TestMethod]
        public void GetCurrentScene_ReturnsSceneFromSceneService()
        {
            var expectedScene = new Mock<IScene>().Object;
            _sceneMock.Setup(s => s.GetCurrentScene(_stateMock.Object))
                     .Returns(expectedScene);

            var result = _facade.GetCurrentScene();

            Assert.AreEqual(expectedScene, result);
        }

        [TestMethod]
        public void MakeChoice_ValidIndex_ProcessesChoice()
        {
            var sceneMock = new Mock<IScene>();
            var choices = new List<Choice>
            {
                new Choice { Text = "Выбор 1" },
                new Choice { Text = "Выбор 2" }
            };

            sceneMock.Setup(s => s.Choices).Returns(choices);
            _sceneMock.Setup(s => s.GetCurrentScene(_stateMock.Object))
                     .Returns(sceneMock.Object);

            _facade.MakeChoice(0);

            _choiceMock.Verify(c => c.ProcessChoice(choices[0], _stateMock.Object), Times.Once);
        }

        [TestMethod]
        public void MakeChoice_InvalidIndex_DoesNothing()
        {
            var sceneMock = new Mock<IScene>();
            var choices = new List<Choice> { new Choice { Text = "Выбор 1" } };
            sceneMock.Setup(s => s.Choices).Returns(choices);
            _sceneMock.Setup(s => s.GetCurrentScene(_stateMock.Object))
                     .Returns(sceneMock.Object);

            _facade.MakeChoice(10);

            _choiceMock.Verify(c => c.ProcessChoice(It.IsAny<Choice>(), It.IsAny<IGameStateService>()), Times.Never);
        }

        [TestMethod]
        public void SaveGame_CallsSaveLoadService()
        {
            const string path = "save.json";

            _facade.SaveGame(path);

            _saveLoadMock.Verify(s => s.SaveGame(path, _stateMock.Object), Times.Once);
        }

        [TestMethod]
        public void LoadGame_CallsSaveLoadService()
        {
            const string path = "save.json";

            _facade.LoadGame(path);

            _saveLoadMock.Verify(s => s.LoadGame(path, _stateMock.Object), Times.Once);
        }

        [TestMethod]
        public void ResetGame_CallsSaveLoadService()
        {
            _facade.ResetGame();

            _saveLoadMock.Verify(s => s.ResetGame(_stateMock.Object), Times.Once);
        }

        [TestMethod]
        public void GetAvailableChoices_ReturnsChoiceTexts()
        {
            var sceneMock = new Mock<IScene>();
            var choices = new List<Choice>
            {
                new Choice { Text = "Выбор 1" },
                new Choice { Text = "Выбор 2" }
            };

            var availableChoices = new List<Choice> { choices[0] };

            sceneMock.Setup(s => s.Choices).Returns(choices);
            _sceneMock.Setup(s => s.GetCurrentScene(_stateMock.Object))
                     .Returns(sceneMock.Object);
            _sceneMock.Setup(s => s.GetAvailableChoices(sceneMock.Object, _stateMock.Object))
                     .Returns(availableChoices);

            var result = _facade.GetAvailableChoices();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Выбор 1", result[0]);
        }

        [TestMethod]
        public void GetInventory_ReturnsCopyOfInventory()
        {
            var inventory = new List<string> { "Меч", "Щит", "Зелье" };
            _stateMock.Setup(s => s.Inventory).Returns(inventory);

            var result = _facade.GetInventory();

            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEqual(inventory, result);
        }

        [TestMethod]
        public void GetCurrentSceneImagePath_ReturnsImagePath()
        {
            const string sceneId = "test_scene";
            const string expectedPath = "Images/test.jpg";

            _stateMock.Setup(s => s.CurrentSceneId).Returns(sceneId);
            _sceneMock.Setup(s => s.GetSceneImagePath(sceneId)).Returns(expectedPath);

            var result = _facade.GetCurrentSceneImagePath();

            Assert.AreEqual(expectedPath, result);
        }

        [TestMethod]
        public void GetVariable_ReturnsValueFromStateService()
        {
            const string varName = "health";
            const int expectedValue = 100;

            _stateMock.Setup(s => s.GetVariable<int>(varName, default))
                     .Returns(expectedValue);

            var result = _facade.GetVariable<int>(varName);

            Assert.AreEqual(expectedValue, result);
        }
    }
}
