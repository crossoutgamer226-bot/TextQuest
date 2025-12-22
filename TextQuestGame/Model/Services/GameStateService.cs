using System;
using System.Collections.Generic;
using System.Text;

namespace TextQuestGame.Model.Services
{
    public class GameStateService : IGameStateService
    {
        private GameState _state;

        public GameStateService()
        {
            _state = new GameState();
        }

        public string CurrentSceneId
        {
            get => _state.CurrentSceneId;
            set => _state.CurrentSceneId = value;
        }

        public List<string> Inventory => _state.Inventory;
        public Dictionary<string, object> Variables => _state.Variables;

        public void AddItem(string item)
        {
            if (!string.IsNullOrEmpty(item) && !_state.Inventory.Contains(item))
            {
                _state.Inventory.Add(item);
            }
        }

        public void RemoveItem(string item)
        {
            _state.Inventory.Remove(item);
        }

        public bool HasItem(string item)
        {
            return _state.Inventory.Contains(item);
        }

        public void SetVariable(string name, object value)
        {
            _state.Variables[name] = value;
        }

        public T GetVariable<T>(string name, T defaultValue = default)
        {
            if (_state.Variables.TryGetValue(name, out var value))
            {
                if (value is T typedValue)
                    return typedValue;

                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }
    }
}
