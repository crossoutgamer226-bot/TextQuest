using System;
using System.Collections.Generic;
using System.Text;

namespace TextQuestGame
{
    [Serializable]
    public class Choice
    {
        public string Text { get; set; }
        public string NextSceneId { get; set; }
        public string Condition { get; set; }
        public string Effect { get; set; }
    }
}
