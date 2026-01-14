using RuleEngineApp.Models;
using RuleEngineApp.Cache;

namespace RuleEngineApp.Rules
{
    public static class RuleEngine
    {
        public static bool Evaluate<T>(T target, RuleSet ruleSet)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (ruleSet == null || ruleSet.Rules.Count == 0) return true;

            bool result = ruleSet.CombineWithAnd;

            foreach (var rule in ruleSet.Rules)
            {
                var validated = RuleValidator.Validate<T>(rule);
                bool r = RuleEvaluator.Evaluate(target, rule, validated);

                result = ruleSet.CombineWithAnd ? result & r : result | r;

                if (ruleSet.CombineWithAnd && !result) return false;
                if (!ruleSet.CombineWithAnd && result) return true;
            }
            return result;
        }

        public static Func<T,bool> BuildPredicate<T>(RuleSet ruleSet)
        {
            if (ruleSet == null || ruleSet.Rules.Count == 0) return x => true;
            return RuleCache<T>.GetOrAdd(ruleSet);
        }
    }
}