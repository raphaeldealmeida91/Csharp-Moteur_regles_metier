using System.Linq.Expressions;
using RuleEngineApp.Models;
using RuleEngineApp.Rules;

namespace RuleEngineApp.Expressions
{
    public static class ExpressionBuilder
    {
        public static Expression<Func<T,bool>> BuildPredicate<T>(RuleSet ruleSet)
        {
            if (ruleSet == null || ruleSet.Rules.Count == 0) return x => true;

            ParameterExpression param = Expression.Parameter(typeof(T), "obj");
            Expression? combined = null;

            foreach (var rule in ruleSet.Rules)
            {
                var validated = RuleValidator.Validate<T>(rule);
                var prop = Expression.Property(param, validated.Property.Name);
                var constant = Expression.Constant(validated.ConvertedTargetValue);

                Expression comparison = rule.Operator switch
                {
                    RuleOperator.Equal => Expression.Equal(prop, constant),
                    RuleOperator.NotEqual => Expression.NotEqual(prop, constant),
                    RuleOperator.GreaterThan => Expression.GreaterThan(prop, constant),
                    RuleOperator.LessThan => Expression.LessThan(prop, constant),
                    _ => throw new NotSupportedException($"Opérateur '{rule.Operator}' non supporté")
                };

                combined = combined == null
                    ? comparison
                    : ruleSet.CombineWithAnd
                        ? Expression.AndAlso(combined, comparison)
                        : Expression.OrElse(combined, comparison);
            }

            return Expression.Lambda<Func<T,bool>>(combined!, param);
        }
    }
}