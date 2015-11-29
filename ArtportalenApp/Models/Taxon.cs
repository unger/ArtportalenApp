namespace ArtportalenApp.Models
{
    public class Taxon
    {
        public string Id { get; set; }

        public int TaxonId { get; set; }

        public int? Prefix { get; set; }

        public int? SortOrder { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
