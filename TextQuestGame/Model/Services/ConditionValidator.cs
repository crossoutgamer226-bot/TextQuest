using System;
using System.Collections.Generic;
using System.Text;

namespace TextQuestGame.Model.Services
{
    public interface IConditionValidator
    {
        bool Validate(string condition, IGameStateService state);
    }

    public class ConditionValidator : IConditionValidator
    {
        public bool Validate(string condition, IGameStateService state)
        {
            var parts = condition.Split(':');
            if (parts.Length == 0) return true;

            var conditionType = parts[0].ToLower();

            switch (conditionType)
            {
                case "hasitem":
                    return parts.Length > 1 && state.HasItem(parts[1]);

                case "nothasitem":
                    return parts.Length > 1 && !state.HasItem(parts[1]);

                case "variable":
                    return parts.Length > 2 &&
                           state.GetVariable<string>(parts[1], "") == parts[2];

                default:
                    return false;
            }
        }
    }
}
