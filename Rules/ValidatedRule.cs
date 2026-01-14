using System.Reflection;

namespace RuleEngineApp.Rules
{
    public class ValidatedRule(PropertyInfo property, object convertedTargetValue)
    {
        public PropertyInfo Property { get; } = property;
        public object ConvertedTargetValue { get; } = convertedTargetValue;
    }
}