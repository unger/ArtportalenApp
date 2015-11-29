using System.Collections.Generic;
using System.Threading.Tasks;
using ArtportalenApp.Models;

namespace ArtportalenApp.Interfaces
{
    public interface IRuleStorage
    {
        Task<IList<Rule>> GetRules();

        Task SaveRule(Rule rule);
        
        Task DeleteRule(string id);
    }
}