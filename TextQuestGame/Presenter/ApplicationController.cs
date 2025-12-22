using System;
using System.IO;
using System.Windows.Forms;
using TextQuestGame.Model.Services;

namespace TextQuestGame.Presenter
{
    public class ApplicationController : IDisposable
    {
        private GamePresenter _presenter;
        private IGameFacade _gameFacade;
        private IGameView _view;

        public void Start()
        {
            try
            {
                IGameStateService stateService = new GameStateService();
                ISceneService sceneService = new SceneService("scenes.json");
                IChoiceService choiceService = new ChoiceService();
                ISaveLoadService saveLoadService = new SaveLoadService();
                _gameFacade = new GameFacade(
                    stateService,
                    sceneService,
                    choiceService,
                    saveLoadService
                );
                _view = new MainForm();
                _presenter = new GamePresenter(_view, _gameFacade);
                Application.Run(_view as Form);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка запуска: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }

        
        public void Dispose()
        {
            _presenter = null;
            _gameFacade = null;
            _view = null;
        }
    }
}
