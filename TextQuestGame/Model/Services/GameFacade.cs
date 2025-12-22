using System;
using System.Collections.Generic;
using System.Text;

namespace TextQuestGame.Model.Services
{
    public class GameFacade : IGameFacade
    {
        private readonly IGameStateService _stateService;
        private readonly ISceneService _sceneService;
        private readonly IChoiceService _choiceService;
        private readonly ISaveLoadService _saveLoadService;

        public GameFacade(
            IGameStateService stateService,
            ISceneService sceneService,
            IChoiceService choiceService,
            ISaveLoadService saveLoadService)
        {
            _stateService = stateService;
            _sceneService = sceneService;
            _choiceService = choiceService;
            _saveLoadService = saveLoadService;
        }

        public IScene GetCurrentScene()
        {
            return _sceneService.GetCurrentScene(_stateService);
        }

        public void MakeChoice(int choiceIndex)
        {
            var scene = GetCurrentScene();

            if (choiceIndex >= 0 && choiceIndex < scene.Choices.Count)
            {
                var choice = scene.Choices[choiceIndex];
                _choiceService.ProcessChoice(choice, _stateService);
            }
        }

        public void SaveGame(string path)
        {
            _saveLoadService.SaveGame(path, _stateService);
        }

        public void LoadGame(string path)
        {
            _saveLoadService.LoadGame(path, _stateService);
        }

        public void ResetGame()
        {
            _saveLoadService.ResetGame(_stateService);
        }

        public List<string> GetAvailableChoices()
        {
            var scene = GetCurrentScene();
            var availableChoices = _sceneService.GetAvailableChoices(scene, _stateService);
            return availableChoices.Select(c => c.Text).ToList();
        }

        public string GetCurrentSceneImagePath()
        {
            return _sceneService.GetSceneImagePath(_stateService.CurrentSceneId);
        }

        public List<string> GetInventory()
        {
            return new List<string>(_stateService.Inventory);
        }
    }
}
