using System;
using System.Collections.Generic;
using System.Text;

namespace TextQuestGame
{
    [Serializable]
    public class Scene : IScene
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public List<Choice> Choices { get; set; } = new List<Choice>();
    }

}
