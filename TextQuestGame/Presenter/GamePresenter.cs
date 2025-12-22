using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TextQuestGame.Model;
using TextQuestGame.Model.Services;

namespace TextQuestGame.Presenter
{
    public class GamePresenter
    {
        private readonly IGameFacade _game;
        private readonly IGameView _view;

        public GamePresenter(MainForm view, IGameService game)
        {
            _view = view;
            _game = game;

            _view.ChoiceSelected += OnChoiceSelected;
            _view.SaveRequested += OnSaveRequested;
            _view.LoadRequested += OnLoadRequested;
            _view.NewGameRequested += OnNewGameRequested;

            UpdateView();
        }

        private void OnChoiceSelected(int index)
        {
            try
            {
                _game.MakeChoice(index);
                UpdateView();
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при выборе: {ex.Message}");
            }
        }

        private void OnSaveRequested()
        {
            try
            {
                _game.SaveGame("save.json");
                _view.ShowMessage("Игра успешно сохранена!");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void OnLoadRequested()
        {
            try
            {
                _game.LoadGame("save.json");
                UpdateView();
                _view.ShowMessage("Игра загружена!");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка загрузки: {ex.Message}\nФайл сохранения не найден или поврежден.");
            }
        }

        private void OnNewGameRequested()
        {
            try
            {
                var result = MessageBox.Show(
                    "Начать новую игру?\nВсе несохраненные данные будут потеряны.",
                    "Новая игра",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _game.ResetGame();
                    UpdateView();
                    _view.ShowMessage("Новая игра начата!");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка начала новой игры: {ex.Message}");
            }
        }

        private void UpdateView()
        {
            try
            {

                var scene = _game.GetCurrentScene();
                if (scene == null)
                {
                    _view.ShowError("Не удалось загрузить сцену");
                    return;
                }


                _view.DisplayScene(scene);


                _view.UpdateInventory(_game.GetInventory());


                var gameInfo = BuildGameInfo();
                _view.UpdateGameInfo(gameInfo);


                CheckAvailableChoices();
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка обновления интерфейса: {ex.Message}");
            }
        }

        private string BuildGameInfo()
        {
            var info = $"Сцена: {_game.GetCurrentScene().Id}";


            var inventory = _game.GetInventory();
            if (inventory != null)
            {
                info += $" | Предметов: {inventory.Count}";
            }

            var health = GetVariableSafe<int>("health", 100);
            info += $" | Здоровье: {health}";

            var money = GetVariableSafe<int>("money", 0);
            if (money > 0)
            {
                info += $" | Деньги: {money}";
            }

            return info;
        }

        private T GetVariableSafe<T>(string name, T defaultValue = default)
        {
            try
            {

                return _game.GetVariable<T>(name, default);

            }
            catch
            {
                return defaultValue;
            }
        }

        private void CheckAvailableChoices()
        {
            var scene = _game.GetCurrentScene();
            if (scene != null && scene.Choices.Count == 0)
            {
                _view.ShowMessage("Конец этой сюжетной линии. Начните новую игру или загрузите сохранение.");
            }
        }
        public void RefreshView()
        {
            UpdateView();
        }
    }
}
    