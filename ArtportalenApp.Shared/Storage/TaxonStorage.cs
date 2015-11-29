using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Model;
using ArtportalenApp.Models;
using Parse;

namespace ArtportalenApp.Storage
{
    public class TaxonStorage : ITaxonStorage
    {
        public async Task<IList<Taxon>> GetTaxons()
        {
            var taxons = await new ParseQuery<ApParseTaxon>()
                    .Where(t => t.Type == "Art")
                    .OrderBy(t => t.SortOrder)
                    .Limit(1000)
                    .FindAsync();

            return taxons.Select(ConvertToTaxon).ToList();            
        }

        private static Taxon ConvertToTaxon(ApParseTaxon t)
        {
            return new Taxon
            {
                Id = t.ObjectId,
                TaxonId = t.TaxonId,
                Name = t.Name,
                Prefix = t.Prefix,
                SortOrder = t.SortOrder,
                Type = t.Type,
            };
        }
    }
}
