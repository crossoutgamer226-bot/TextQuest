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
                // СОЗДАЕМ СЕРВИСЫ
                // 1. Сервис состояния игры
                IGameStateService stateService = new GameStateService();

                // 2. Сервис сцен
                ISceneService sceneService = new SceneService("scenes.json");

                // 3. Сервис обработки выборов
                IChoiceService choiceService = new ChoiceService();

                // 4. Сервис сохранения/загрузки
                ISaveLoadService saveLoadService = new SaveLoadService();

                // 5. СОЗДАЕМ ФАСАД
                _gameFacade = new GameFacade(
                    stateService,
                    sceneService,
                    choiceService,
                    saveLoadService
                );

                // Создаём представление
                _view = new MainForm();

                // Создаём презентер
                _presenter = new GamePresenter(_view, _gameFacade);

                // Запускаем приложение
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
            // Очистка ресурсов
            _presenter = null;
            _gameFacade = null;
            _view = null;
        }
    }
}