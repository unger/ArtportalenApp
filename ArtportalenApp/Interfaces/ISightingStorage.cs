using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Models;

namespace ArtportalenApp.Interfaces
{
    public interface ISightingStorage
    {
        Task<IList<Sighting>> Match(string daysFilter, string ruleId);
    }
}
