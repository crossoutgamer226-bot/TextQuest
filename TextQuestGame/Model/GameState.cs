using System;
using System.Collections.Generic;
using System.Text;

namespace TextQuestGame
{
    [Serializable]
    public class GameState : IGameState
    {
        public string CurrentSceneId { get; set; } = "start";
        public List<string> Inventory { get; set; } = new List<string>();
        public Dictionary<string, object> Variables { get; set; } = new Dictionary<string, object>();
    }

}
