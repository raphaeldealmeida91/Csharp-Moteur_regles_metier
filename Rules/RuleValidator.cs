using System.Reflection;
using RuleEngineApp.Models;

namespace RuleEngineApp.Rules
{
    public static class RuleValidator
    {
        public static ValidatedRule Validate<T>(RuleDefinition rule)
        {
            var property = typeof(T).GetProperty(
                rule.MemberName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (property == null)
                throw new ArgumentException($"Propriété '{rule.MemberName}' introuvable sur {typeof(T).Name}");

            var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

            try
            {
                var convertedValue = Convert.ChangeType(rule.TargetValue, targetType);
                return new ValidatedRule(property, convertedValue!);
            }
            catch
            {
                throw new ArgumentException($"Valeur '{rule.TargetValue}' incompatible avec {targetType.Name}");
            }
        }
    }
}