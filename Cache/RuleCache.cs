using System.Collections.Concurrent;
using RuleEngineApp.Models;
using RuleEngineApp.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace RuleEngineApp.Cache
{
    public static class RuleCache<T>
    {
        private static readonly ConcurrentDictionary<string, Func<T,bool>> _cache = new();

        public static Func<T,bool> GetOrAdd(RuleSet ruleSet)
        {
            string key = GetRuleSetHash(ruleSet);
            return _cache.GetOrAdd(key, _ => ExpressionBuilder.BuildPredicate<T>(ruleSet).Compile());
        }

        private static string GetRuleSetHash(RuleSet ruleSet)
        {
            using var sha = SHA256.Create();
            var json = System.Text.Json.JsonSerializer.Serialize(ruleSet);
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(json));
            return Convert.ToHexString(hash);
        }
    }
}