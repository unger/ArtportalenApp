using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;

namespace ArtportalenApp.Services
{
    public class TaxonService : ITaxonService
    {
        private readonly ITaxonStorage _taxonStorage;

        public TaxonService(ITaxonStorage taxonStorage)
        {
            _taxonStorage = taxonStorage;
        }

        public Task<IList<Taxon>> GetTaxons()
        {
            return _taxonStorage.GetTaxons();
        }

        public Task<IList<Taxon>> GetSpecies()
        {
            return _taxonStorage.GetSpecies();
        }
    }
}
