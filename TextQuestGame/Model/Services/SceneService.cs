using System;
using System.Collections.Generic;
using System.Text;

namespace TextQuestGame.Model.Services
{
    public class SceneService : ISceneService
    {
        public class SceneService : ISceneService
        {
            private readonly Dictionary<string, Scene> _scenes;
            private readonly string _scenesFilePath;
            private readonly IChoiceService _choiceService;

            // Добавляем зависимость от IChoiceService
            public SceneService(string scenesFilePath = "scenes.json", IChoiceService choiceService = null)
            {
                _scenesFilePath = scenesFilePath;
                _scenes = SceneLoader.LoadScenes(_scenesFilePath);
                _choiceService = choiceService ?? new ChoiceService(); // Можно передать через DI
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
                    // Используем ChoiceService для проверки условий
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
Альтернативный подход: полностью удалить проверку условий из SceneService
Если вы хотите еще более чистое разделение ответственности, можно убрать фильтрацию из GetAvailableChoices и делать ее на уровне фасада:

csharp
namespace TextQuestGame.Services
    {
        public class SceneService : ISceneService
        {
            private readonly Dictionary<string, Scene> _scenes;
            private readonly string _scenesFilePath;

            public SceneService(string scenesFilePath = "scenes.json")
            {
                _scenesFilePath = scenesFilePath;
                _scenes = SceneLoader.LoadScenes(_scenesFilePath);
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
                return scene.Choices.ToList();
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
