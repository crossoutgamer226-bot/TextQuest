using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TextQuestGame.Model.Services;

namespace TextQuestGame.Tests.Services
{
    [TestClass]
    public class EffectApplierTests
    {
        private Mock<IGameStateService> _stateMock;
        private EffectApplier _applier;

        [TestInitialize]
        public void Setup()
        {
            _stateMock = new Mock<IGameStateService>();
            _applier = new EffectApplier();
        }

        [TestMethod]
        public void Apply_AddItem_CallsAddItemOnState()
        {
            const string effect = "AddItem:Меч";

            _applier.Apply(effect, _stateMock.Object);

            _stateMock.Verify(s => s.AddItem("Меч"), Times.Once);
        }

        [TestMethod]
        public void Apply_RemoveItem_CallsRemoveItemOnState()
        {
            const string effect = "RemoveItem:Ключ";

            _applier.Apply(effect, _stateMock.Object);

            _stateMock.Verify(s => s.RemoveItem("Ключ"), Times.Once);
        }

        [TestMethod]
        public void Apply_SetVariable_WithInteger_SetsVariable()
        {
            const string effect = "SetVariable:здоровье:100";

            _applier.Apply(effect, _stateMock.Object);

            _stateMock.Verify(s => s.SetVariable("здоровье", 100), Times.Once);
        }

        [TestMethod]
        public void Apply_SetVariable_WithString_SetsVariable()
        {
            const string effect = "SetVariable:имя:Иван";

            _applier.Apply(effect, _stateMock.Object);

            _stateMock.Verify(s => s.SetVariable("имя", "Иван"), Times.Once);
        }

        [TestMethod]
        public void Apply_SetVariable_WithBoolean_SetsVariable()
        {
            const string effect = "SetVariable:победа:true";

            _applier.Apply(effect, _stateMock.Object);

            _stateMock.Verify(s => s.SetVariable("победа", true), Times.Once);
        }

        [TestMethod]
        public void Apply_MultipleEffects_SeparatedBySemicolon_AllApplied()
        {
            const string effect = "AddItem:Меч;AddItem:Щит;SetVariable:золото:50";

            _applier.Apply(effect, _stateMock.Object);

            _stateMock.Verify(s => s.AddItem("Меч"), Times.Once);
            _stateMock.Verify(s => s.AddItem("Щит"), Times.Once);
            _stateMock.Verify(s => s.SetVariable("золото", 50), Times.Once);
        }

        [TestMethod]
        public void Apply_MultipleEffects_SeparatedByComma_AllApplied()
        {
            const string effect = "AddItem:Карта,AddItem:Компас";

            _applier.Apply(effect, _stateMock.Object);

            _stateMock.Verify(s => s.AddItem("Карта"), Times.Once);
            _stateMock.Verify(s => s.AddItem("Компас"), Times.Once);
        }

        [TestMethod]
        public void Apply_EmptyEffect_DoesNothing()
        {
            const string effect = "";

            _applier.Apply(effect, _stateMock.Object);

            _stateMock.Verify(s => s.AddItem(It.IsAny<string>()), Times.Never);
            _stateMock.Verify(s => s.RemoveItem(It.IsAny<string>()), Times.Never);
            _stateMock.Verify(s => s.SetVariable(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        }

        [TestMethod]
        public void Apply_NullEffect_DoesNothing()
        {
            const string effect = null;

            _applier.Apply(effect, _stateMock.Object);

            _stateMock.Verify(s => s.AddItem(It.IsAny<string>()), Times.Never);
            _stateMock.Verify(s => s.RemoveItem(It.IsAny<string>()), Times.Never);
            _stateMock.Verify(s => s.SetVariable(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        }

        [TestMethod]
        public void Apply_UnknownEffectType_IgnoresIt()
        {
            const string effect = "UnknownEffect:Something";

            _applier.Apply(effect, _stateMock.Object);

            _stateMock.Verify(s => s.AddItem(It.IsAny<string>()), Times.Never);
            _stateMock.Verify(s => s.RemoveItem(It.IsAny<string>()), Times.Never);
            _stateMock.Verify(s => s.SetVariable(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        }
    }
}
