namespace RuleEngineApp.Models
{
    public class Customer
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public double LoyaltyPoints { get; set; }
        public bool IsSubscribed { get; set; }
    }
}