using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Extensions;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using Parse;

namespace ArtportalenApp.Storage
{
    public class RuleStorage : IRuleStorage
    {
        public async Task<IList<Rule>> GetRules()
        {
            var rules = await new ParseQuery<ApParseRule>()
                .FindAsync();

            return rules.Select(ConvertToRule).ToList();
        }

        public async Task SaveRule(Rule rule)
        {
            ApParseRule parseRule;
            if (string.IsNullOrEmpty(rule.Id))
            {
                parseRule = new ApParseRule
                {
                    ACL = new ParseACL(ParseUser.CurrentUser)
                    {
                        PublicReadAccess = false, 
                        PublicWriteAccess = false
                    }
                };
            }
            else
            {
                parseRule = ParseObject.CreateWithoutData<ApParseRule>(rule.Id);
            }

            parseRule.Name = rule.Name;
            parseRule.Prefix = rule.Prefix;
            parseRule.Taxons = rule.Taxons.IsNullOrEmpty() ? null : rule.Taxons.ToList();
            parseRule.Kommuner = rule.Kommuner.IsNullOrEmpty() ? null : rule.Kommuner.ToList();
            parseRule.Landskap = rule.Landskap.IsNullOrEmpty() ? null : rule.Landskap.ToList();
            parseRule.IsActive = rule.IsActive;
            parseRule.User = ParseUser.CurrentUser;

            await parseRule.SaveAsync();
        }

        public Task DeleteRule(string id)
        {
            var parseRule = ParseObject.CreateWithoutData<ApParseRule>(id);
            return parseRule.DeleteAsync();
        }

        private static Rule ConvertToRule(ApParseRule r)
        {
            return new Rule
            {
                Id = r.ObjectId,
                Prefix = r.Prefix,
                Name = r.Name,
                Taxons = r.Taxons != null ? r.Taxons.ToArray() : null,
                Kommuner = r.Kommuner != null ? r.Kommuner.ToArray() : null,
                Landskap = r.Landskap != null ? r.Landskap.ToArray() : null,
                IsActive = r.IsActive,
            };
        }
    }
}
