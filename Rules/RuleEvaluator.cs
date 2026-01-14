using RuleEngineApp.Models;

namespace RuleEngineApp.Rules
{
    public static class RuleEvaluator
    {
        public static bool Evaluate<T>(T target, RuleDefinition rule, ValidatedRule validatedRule)
        {
            var value = validatedRule.Property.GetValue(target);

            return rule.Operator switch
            {
                RuleOperator.Equal => Equals(value, validatedRule.ConvertedTargetValue),
                RuleOperator.NotEqual => !Equals(value, validatedRule.ConvertedTargetValue),
                RuleOperator.GreaterThan => Compare(value, validatedRule.ConvertedTargetValue) > 0,
                RuleOperator.LessThan => Compare(value, validatedRule.ConvertedTargetValue) < 0,
                _ => throw new NotSupportedException($"Opérateur '{rule.Operator}' non supporté")
            };
        }

        private static int Compare(object? left, object right)
        {
            if (left is IComparable c) return c.CompareTo(right);
            throw new InvalidOperationException($"Type {left?.GetType().Name} non comparable");
        }
    }
}