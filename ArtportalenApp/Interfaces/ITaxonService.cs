using System.Collections.Generic;
using System.Threading.Tasks;
using ArtportalenApp.Models;

namespace ArtportalenApp.Interfaces
{
    public interface ITaxonService
    {
        Task<IList<Taxon>> GetTaxons();

        Task<IList<Taxon>> GetSpecies();
    }
}