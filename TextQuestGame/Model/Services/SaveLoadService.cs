using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace TextQuestGame.Model.Services
{
    public class SaveLoadService : ISaveLoadService
    {
        public void SaveGame(string path, IGameStateService state)
        {
            try
            {
                var gameState = new GameState
                {
                    CurrentSceneId = state.CurrentSceneId,
                    Inventory = new List<string>(state.Inventory),
                    Variables = new Dictionary<string, object>(state.Variables)
                };

                var json = JsonSerializer.Serialize(gameState, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сохранения: {ex.Message}", ex);
            }
        }

        public void LoadGame(string path, IGameStateService state)
        {
            try
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException($"Файл не найден: {path}");

                var json = File.ReadAllText(path);
                var loadedState = JsonSerializer.Deserialize<GameState>(json);

                state.CurrentSceneId = loadedState.CurrentSceneId;

                state.Inventory.Clear();
                foreach (var item in loadedState.Inventory)
                    state.AddItem(item);

                state.Variables.Clear();
                foreach (var kvp in loadedState.Variables)
                    state.SetVariable(kvp.Key, kvp.Value);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка загрузки: {ex.Message}", ex);
            }
        }

        public void ResetGame(IGameStateService state)
        {
            state.CurrentSceneId = "start";
            state.Inventory.Clear();
            state.Variables.Clear();
            state.SetVariable("health", 100);
            state.SetVariable("money", 0);
        }
    }
}
