using System;
using System.Collections.Generic;
using System.Text;

namespace TextQuestGame.Model.Services
{
    public interface IEffectApplier
    {
        void Apply(string effect, IGameStateService state);
    }
    public class EffectApplier : IEffectApplier
    {
        public void Apply(string effect, IGameStateService state)
        {
            var parts = effect.Split(':');
            if (parts.Length == 0) return;

            var effectType = parts[0].ToLower();

            switch (effectType)
            {
                case "additem":
                    if (parts.Length > 1) state.AddItem(parts[1]);
                    break;

                case "removeitem":
                    if (parts.Length > 1) state.RemoveItem(parts[1]);
                    break;

                case "setvariable":
                    if (parts.Length > 2) state.SetVariable(parts[1], parts[2]);
                    break;
            }
        }
    }
}
