using System.Text.Json.Serialization;
using RuleEngineApp.Rules;

namespace RuleEngineApp.Models
{
    public class RuleDefinition
    {
        public string MemberName { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RuleOperator Operator { get; set; }

        public string TargetValue { get; set; } = string.Empty;
    }


}