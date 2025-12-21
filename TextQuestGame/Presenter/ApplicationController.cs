using System;
using System.Collections.Generic;
using System.Text;

namespace TextQuestGame.Presenter
{
    public class ApplicationController : IDisposable
    {
        private GamePresenter _presenter;
        private IGameService _gameService;
        private IGameView _view;

        public void Start()
        {
            try
            {
                CheckRequiredFiles();

                _gameService = new GameService();

                _view = new MainForm();

                _presenter = new GamePresenter(_view, _gameService);

                Application.Run(_view as Form);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка запуска: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }

        private void CheckRequiredFiles()
        {
            if (!File.Exists("scenes.json"))
            {
                CreateDefaultScenesFile();
            }

            if (!Directory.Exists("Images"))
            {
                Directory.CreateDirectory("Images");
                MessageBox.Show("Создана папка Images для картинок. Добавьте туда файлы .jpg для сцен.",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

       