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
            if (string.IsNullOrEmpty(effect)) return;

            // Разделяем эффекты по запятой или точке с запятой
            var effects = effect.Split(new[] { ',',';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var singleEffect in effects)
            {
                ApplySingleEffect(singleEffect.Trim(), state);
            }
        }
        private void ApplySingleEffect(string effect, IGameStateService state)
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
                    if (parts.Length > 2)
                    {
                        var name = parts[1];
                        var value = parts[2];

                        // Пытаемся определить тип
                        if (int.TryParse(value, out int intValue))
                            state.SetVariable(name, intValue);
                        else if (bool.TryParse(value, out bool boolValue))
                            state.SetVariable(name, boolValue);
                        else
                            state.SetVariable(name, value);
                    }
                 break;

            }
        }
    }
}
