# RuleEngineApp

Moteur de règles dynamique et générique en C#.  

---

## Fonctionnalités

- Évaluation de règles pour n'importe quel type métier (`Customer`, `Order`, etc.)
- Génération dynamique d'expressions LINQ via **Expression Trees**
- Cache des règles compilées pour des performances optimales
- Support de **AND / OR** pour combiner plusieurs règles
- Sérialisation / désérialisation des règles via JSON
- Gestion des erreurs claire via exceptions

---

## Structure

```text
RuleEngineApp/
│
├── Models/          ← DTO + entités métier
├── Rules/           ← logique des règles et moteur
├── Expressions/     ← Expression Trees
├── Cache/           ← cache SHA256 des règles compilées
└── Program.cs       ← exemple d’usage
```

## Exemple JSON pour RuleSet

```json
{
  "CombineWithAnd": true,
  "Rules": [
    { "MemberName": "Age", "Operator": "GreaterThan", "TargetValue": "18" },
    { "MemberName": "LoyaltyPoints", "Operator": "GreaterThan", "TargetValue": "100" }
  ]
}
```

Exemple d'utilisation

```csharp
using System.Text.Json;
using RuleEngineApp.Models;
using RuleEngineApp.Rules;

// Charger les clients
var customers = new List<Customer>
{
    new Customer { Name="Alice", Age=25, LoyaltyPoints=120, IsSubscribed=true },
    new Customer { Name="Bob", Age=17, LoyaltyPoints=50, IsSubscribed=false },
    new Customer { Name="Charlie", Age=30, LoyaltyPoints=200, IsSubscribed=true }
};

// Charger les règles depuis un fichier JSON
var json = File.ReadAllText("rules.json");
var ruleSet = JsonSerializer.Deserialize<RuleSet>(json);

// Compiler la predicate via cache SHA256
var predicate = RuleEngine.BuildPredicate<Customer>(ruleSet!);

// Filtrer
var filtered = customers.Where(predicate).ToList();
```

## Performance

- Reflection : lent (~50ms pour 60k objets)
- Expression compilée + cache : très rapide (~4ms pour 60k objets)
- Cache SHA256 → recompilation uniquement si le RuleSet change
