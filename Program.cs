using System.Text.Json;
using RuleEngineApp.Models;
using RuleEngineApp.Rules;

Console.WriteLine("===== Test moteur de règles dynamique depuis JSON =====");

// Clients
var customers = new List<Customer>
{
    new() { Name="Alice", Age=25, LoyaltyPoints=120, IsSubscribed=true },
    new() { Name="Bob", Age=17, LoyaltyPoints=50, IsSubscribed=false },
    new() { Name="Charlie", Age=30, LoyaltyPoints=200, IsSubscribed=true }
};

// Charger les règles depuis un fichier JSON
RuleSet ruleSet;
try
{
    var json = File.ReadAllText("rules.json");
    ruleSet = JsonSerializer.Deserialize<RuleSet>(json) 
              ?? throw new InvalidOperationException("Impossible de charger le RuleSet depuis le JSON.");
}
catch (Exception ex)
{
    Console.WriteLine($"Erreur lors du chargement des règles : {ex.Message}");
    return;
}

// Compiler via cache et filtrer
Func<Customer,bool> predicate;
try
{
    predicate = RuleEngine.BuildPredicate<Customer>(ruleSet);
}
catch (Exception ex)
{
    Console.WriteLine($"Erreur lors de la compilation des règles : {ex.Message}");
    return;
}

// Filtrer
var filtered = customers.Where(predicate).ToList();

Console.WriteLine("Clients filtrés :");
foreach (var c in filtered)
    Console.WriteLine($"{c.Name} - Age: {c.Age}, Points: {c.LoyaltyPoints}");