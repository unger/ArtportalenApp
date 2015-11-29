using System.Collections.Generic;
using System.Threading.Tasks;
using ArtportalenApp.Models;

namespace ArtportalenApp.Interfaces
{
    public interface ITaxonStorage
    {
        Task<IList<Taxon>> GetTaxons();
    }
}