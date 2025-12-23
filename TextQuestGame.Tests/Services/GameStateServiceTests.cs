using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextQuestGame.Model.Services;

namespace TextQuestGame.Tests.Services
{
    [TestClass]
    public class GameStateServiceMinimalTests
    {
        private GameStateService _service;

        [TestInitialize]
        public void Setup() => _service = new GameStateService();

        [TestMethod]
        public void AddItem_AddsItem()
        {
            _service.AddItem("Меч");
            Assert.IsTrue(_service.HasItem("Меч"));
        }

        [TestMethod]
        public void RemoveItem_RemovesItem()
        {
            _service.AddItem("Щит");
            _service.RemoveItem("Щит");
            Assert.IsFalse(_service.HasItem("Щит"));
        }

        [TestMethod]
        public void HasItem_ReturnsFalseForMissing()
            => Assert.IsFalse(_service.HasItem("Нет"));

        [TestMethod]
        public void SetVariable_StoresValue()
        {
            _service.SetVariable("здоровье", 100);
            Assert.AreEqual(100, _service.GetVariable<int>("здоровье"));
        }

        [TestMethod]
        public void GetVariable_ReturnsDefaultForMissing()
            => Assert.AreEqual(0, _service.GetVariable<int>("нет"));

        [TestMethod]
        public void CurrentSceneId_CanBeChanged()
        {
            _service.CurrentSceneId = "новая";
            Assert.AreEqual("новая", _service.CurrentSceneId);
        }

        [TestMethod]
        public void Inventory_InitiallyEmpty()
            => Assert.AreEqual(0, _service.Inventory.Count);

        [TestMethod]
        public void AddItem_NoDuplicate()
        {
            _service.AddItem("Ключ");
            _service.AddItem("Ключ");
            Assert.AreEqual(1, _service.Inventory.Count);
        }

        [TestMethod]
        public void GetVariable_WithDefault()
            => Assert.AreEqual("по умолчанию", _service.GetVariable("нет", "по умолчанию"));

        [TestMethod]
        public void Variables_InitiallyEmpty()
            => Assert.AreEqual(0, _service.Variables.Count);
    }
}