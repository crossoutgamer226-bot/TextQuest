using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TextQuestGame.Model.Services;

namespace TextQuestGame.Tests.Services
{
    [TestClass]
    public class ConditionValidatorTests
    {
        [TestMethod]
        public void Validate_HasItem_TrueWhenExists()
        {
            var validator = new ConditionValidator();
            var mock = new Mock<IGameStateService>();
            mock.Setup(s => s.HasItem("Ключ")).Returns(true);
            Assert.IsTrue(validator.Validate("hasitem:Ключ", mock.Object));
        }

        [TestMethod]
        public void Validate_HasItem_FalseWhenMissing()
        {
            var validator = new ConditionValidator();
            var mock = new Mock<IGameStateService>();
            mock.Setup(s => s.HasItem("Ключ")).Returns(false);
            Assert.IsFalse(validator.Validate("hasitem:Ключ", mock.Object));
        }

        [TestMethod]
        public void Validate_NotHasItem_TrueWhenMissing()
        {
            var validator = new ConditionValidator();
            var mock = new Mock<IGameStateService>();
            mock.Setup(s => s.HasItem("Ключ")).Returns(false);
            Assert.IsTrue(validator.Validate("nothasitem:Ключ", mock.Object));
        }

        [TestMethod]
        public void Validate_Variable_TrueWhenMatches()
        {
            var validator = new ConditionValidator();
            var mock = new Mock<IGameStateService>();
            mock.Setup(s => s.GetVariable<string>("имя", "")).Returns("Иван");
            Assert.IsTrue(validator.Validate("variable:имя:Иван", mock.Object));
        }

        [TestMethod]
        public void Validate_Variable_FalseWhenDifferent()
        {
            var validator = new ConditionValidator();
            var mock = new Mock<IGameStateService>();
            mock.Setup(s => s.GetVariable<string>("имя", "")).Returns("Петр");
            Assert.IsFalse(validator.Validate("variable:имя:Иван", mock.Object));
        }

        [TestMethod]
        public void Validate_Empty_ReturnsTrue()
        {
            var validator = new ConditionValidator();
            var mock = new Mock<IGameStateService>();
            Assert.IsTrue(validator.Validate("", mock.Object));
        }

        [TestMethod]
        public void Validate_Null_ReturnsTrue()
        {
            var validator = new ConditionValidator();
            var mock = new Mock<IGameStateService>();
            Assert.IsTrue(validator.Validate(null, mock.Object));
        }

        [TestMethod]
        public void Validate_InvalidFormat_ReturnsFalse()
        {
            var validator = new ConditionValidator();
            var mock = new Mock<IGameStateService>();
            Assert.IsFalse(validator.Validate("invalid", mock.Object));
        }

        [TestMethod]
        public void Validate_HasItem_EmptyItem_ReturnsFalse()
        {
            var validator = new ConditionValidator();
            var mock = new Mock<IGameStateService>();
            Assert.IsFalse(validator.Validate("hasitem:", mock.Object));
        }

        [TestMethod]
        public void Validate_Integration_RealState()
        {
            var validator = new ConditionValidator();
            var state = new GameStateService();
            state.AddItem("Меч");
            state.SetVariable("здоровье", "100");
            Assert.IsTrue(validator.Validate("hasitem:Меч", state));
            Assert.IsTrue(validator.Validate("variable:здоровье:100", state));
        }
    }
}
