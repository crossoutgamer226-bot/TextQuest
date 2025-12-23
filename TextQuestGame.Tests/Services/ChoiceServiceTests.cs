using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextQuestGame.Model.Services;
using TextQuestGame;

namespace TextQuestGame.Tests.Services
{
    [TestClass]
    public class ChoiceServiceTests
    {
        [TestMethod]
        public void ProcessChoice_ValidChoice_Works()
        {
            var service = new ChoiceService();
            var state = new GameStateService();
            var choice = new Choice { Text = "Далее", NextSceneId = "next", Effect = "AddItem:Предмет" };
            service.ProcessChoice(choice, state);
            Assert.AreEqual("next", state.CurrentSceneId);
            Assert.IsTrue(state.HasItem("Предмет"));
        }

        [TestMethod]
        public void ProcessChoice_WithCondition_WhenMet_Works()
        {
            var service = new ChoiceService();
            var state = new GameStateService();
            state.AddItem("Ключ");
            var choice = new Choice { Condition = "hasitem:Ключ", NextSceneId = "room", Effect = "RemoveItem:Ключ" };
            service.ProcessChoice(choice, state);
            Assert.IsFalse(state.HasItem("Ключ"));
        }

        [TestMethod]
        public void ProcessChoice_WithCondition_WhenNotMet_DoesNothing()
        {
            var service = new ChoiceService();
            var state = new GameStateService();
            var choice = new Choice { Condition = "hasitem:Нет", NextSceneId = "room", Effect = "AddItem:Предмет" };
            var initialScene = state.CurrentSceneId;
            service.ProcessChoice(choice, state);
            Assert.AreEqual(initialScene, state.CurrentSceneId);
            Assert.IsFalse(state.HasItem("Предмет"));
        }

        [TestMethod]
        public void ProcessChoice_NullChoice_NoException()
        {
            var service = new ChoiceService();
            var state = new GameStateService();
            service.ProcessChoice(null, state); // Теперь без исключения!
        }

        [TestMethod]
        public void CheckCondition_Empty_ReturnsTrue()
        {
            var service = new ChoiceService();
            var state = new GameStateService();
            Assert.IsTrue(service.CheckCondition("", state));
        }

        [TestMethod]
        public void ApplyEffect_Empty_DoesNothing()
        {
            var service = new ChoiceService();
            var state = new GameStateService();
            service.ApplyEffect("", state);
        }

        [TestMethod]
        public void ProcessChoice_NoNextScene_KeepsScene()
        {
            var service = new ChoiceService();
            var state = new GameStateService();
            var initial = state.CurrentSceneId;
            var choice = new Choice { Effect = "AddItem:Test" };
            service.ProcessChoice(choice, state);
            Assert.AreEqual(initial, state.CurrentSceneId);
            Assert.IsTrue(state.HasItem("Test"));
        }

        [TestMethod]
        public void ProcessChoice_MultipleEffects_AllApplied()
        {
            var service = new ChoiceService();
            var state = new GameStateService();
            var choice = new Choice { Effect = "AddItem:A;AddItem:B;SetVariable:x:10" };
            service.ProcessChoice(choice, state);
            Assert.IsTrue(state.HasItem("A"));
            Assert.IsTrue(state.HasItem("B"));
            Assert.AreEqual(10, state.GetVariable<int>("x"));
        }

        [TestMethod]
        public void CheckCondition_HasItem_Works()
        {
            var service = new ChoiceService();
            var state = new GameStateService();
            state.AddItem("Меч");
            Assert.IsTrue(service.CheckCondition("hasitem:Меч", state));
            Assert.IsFalse(service.CheckCondition("hasitem:Щит", state));
        }

        [TestMethod]
        public void ProcessChoice_VariableCondition_Works()
        {
            var service = new ChoiceService();
            var state = new GameStateService();
            state.SetVariable("статус", "готов");
            var choice = new Choice { Condition = "variable:статус:готов", NextSceneId = "next" };
            service.ProcessChoice(choice, state);
            Assert.AreEqual("next", state.CurrentSceneId);
        }
    }
}