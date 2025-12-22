using System;
using System.Collections.Generic;
using System.Text;

namespace TextQuestGame.Model.Services
{
    public class SceneService : ISceneService
    {
        private readonly Dictionary<string, Scene> _scenes;
        private readonly string _scenesFilePath;
        private readonly IChoiceService _choiceService;

        public SceneService(string scenesFilePath = "scenes.json", IChoiceService choiceService = null)
        {
            _scenesFilePath = scenesFilePath;
            _scenes = SceneLoader.LoadScenes(_scenesFilePath);
            _choiceService = choiceService ?? new ChoiceService();
        }

        public IScene GetScene(string sceneId)
        {
            if (_scenes.TryGetValue(sceneId, out var scene))
                return scene;

            return new Scene
            {
                Id = "error",
                Text = "Сцена не найдена",
                ImagePath = "Images/default.jpg",
                Choices = new List<Choice>()
            };
        }

        public IScene GetCurrentScene(IGameStateService state)
        {
            return GetScene(state.CurrentSceneId);
        }

        public string GetSceneImagePath(string sceneId)
        {
            var scene = GetScene(sceneId) as Scene;
            return ValidateImagePath(scene?.ImagePath);
        }

        public List<Choice> GetAvailableChoices(IScene scene, IGameStateService state)
        {
            var availableChoices = new List<Choice>();

            foreach (var choice in scene.Choices)
            {
                if (_choiceService.CheckCondition(choice.Condition, state))
                {
                    availableChoices.Add(choice);
                }
            }

            return availableChoices;
        }

        private string ValidateImagePath(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return "Images/default.jpg";

            if (File.Exists(imagePath))
                return imagePath;

            return "Images/default.jpg";
        }
    }
}
