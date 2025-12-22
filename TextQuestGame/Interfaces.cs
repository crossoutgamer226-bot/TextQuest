using System;
using System.Collections.Generic;
using System.Text;

namespace TextQuestGame
{
    public interface IScene
    {
        string Id { get; }
        string Text { get; }
        List<Choice> Choices { get; }
        public string ImagePath { get; set; }
    }

    public interface IGameState
    {
        string CurrentSceneId { get; set; }
        List<string> Inventory { get; }
    }

    public interface IGameStateService
    {
        string CurrentSceneId { get; set; }
        List<string> Inventory { get; }
        Dictionary<string, object> Variables { get; }

        void AddItem(string item);
        void RemoveItem(string item);
        bool HasItem(string item);
        void SetVariable(string name, object value);
        T GetVariable<T>(string name, T defaultValue = default);
    }

    public interface ISceneService
    {
        IScene GetScene(string sceneId);
        IScene GetCurrentScene(IGameStateService state);
        string GetSceneImagePath(string sceneId);
        List<Choice> GetAvailableChoices(IScene scene, IGameStateService state);
    }

    public interface IChoiceService
    {
        bool CheckCondition(string condition, IGameStateService state);
        void ApplyEffect(string effect, IGameStateService state);
        void ProcessChoice(Choice choice, IGameStateService state);
    }

    public interface ISaveLoadService
    {
        void SaveGame(string path, IGameStateService state);
        void LoadGame(string path, IGameStateService state);
        void ResetGame(IGameStateService state);
    }

    public interface IGameFacade
    {
        IScene GetCurrentScene();
        void MakeChoice(int choiceIndex);
        void SaveGame(string path);
        void LoadGame(string path);
        void ResetGame();
        List<string> GetAvailableChoices();
        string GetCurrentSceneImagePath();
        List<string> GetInventory();
    }


}
