using System;
using System.Collections.Generic;
using System.Text;

namespace TextQuestGame.Model.Services
{
    public class ChoiceService : IChoiceService
    {
        private readonly IConditionValidator _conditionValidator;
        private readonly IEffectApplier _effectApplier;

        public ChoiceService()
        {
            _conditionValidator = new ConditionValidator();
            _effectApplier = new EffectApplier();
        }

        public bool CheckCondition(string condition, IGameStateService state)
        {
            if (string.IsNullOrEmpty(condition))
                return true;

            return _conditionValidator.Validate(condition, state);
        }

        public void ApplyEffect(string effect, IGameStateService state)
        {
            if (!string.IsNullOrEmpty(effect))
            {
                _effectApplier.Apply(effect, state);
            }
        }

        public void ProcessChoice(Choice choice, IGameStateService state)
        {
            if (choice == null || state == null)
                return;

            if (CheckCondition(choice.Condition, state))
            {
                ApplyEffect(choice.Effect, state);
                if (!string.IsNullOrEmpty(choice.NextSceneId))
                {
                    state.CurrentSceneId = choice.NextSceneId;
                }
            }
        }
    }
}
