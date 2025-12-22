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

        public GamePresenter(IGameView view, IGameFacade game)
        {
            _view = view;
            _game = game;

            // Подписка на события View
            _view.ChoiceSelected += OnChoiceSelected;
            _view.SaveRequested += OnSaveRequested;
            _view.LoadRequested += OnLoadRequested;
            _view.NewGameRequested += OnNewGameRequested;

            // Инициализация представления
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
                // Получаем текущую сцену
                var scene = _game.GetCurrentScene();
                if (scene == null)
                {
                    _view.ShowError("Не удалось загрузить сцену");
                    return;
                }

                // Отображаем сцену в интерфейсе
                _view.DisplayScene(scene);

                // Обновляем инвентарь
                _view.UpdateInventory(_game.GetInventory());

                // Обновляем информацию о состоянии игры
                var gameInfo = BuildGameInfo();
                _view.UpdateGameInfo(gameInfo);

                // Проверяем, есть ли доступные выборы
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

            // Добавляем количество предметов в инвентаре
            var inventory = _game.GetInventory();
            if (inventory != null)
            {
                info += $" | Предметов: {inventory.Count}";
            }

            // Добавляем здоровье, если есть такая переменная
            var health = GetVariableSafe<int>("health", 100);
            info += $" | Здоровье: {health}";

            // Добавляем деньги, если есть такая переменная
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
                // Если в IGameFacade есть метод GetVariable, используем его
                // Иначе используем defaultValue
                return _game.GetVariable<T>(name, default);
                //return defaultValue;
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
                _view.ShowMessage("🎮 Конец этой сюжетной линии. Начните новую игру или загрузите сохранение.");
            }
        }

        // Метод для принудительного обновления интерфейса (может пригодиться)
        public void RefreshView()
        {
            UpdateView();
        }
    }
}