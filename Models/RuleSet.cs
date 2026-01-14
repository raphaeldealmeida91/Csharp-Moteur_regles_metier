namespace RuleEngineApp.Models
{
    public class RuleSet
    {
        public List<RuleDefinition> Rules { get; set; } = [];
        public bool CombineWithAnd { get; set; } = true;
    }
}