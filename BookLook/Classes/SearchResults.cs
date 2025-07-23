using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BookLook.Classes
{ 
    public partial class BookSearchResults
    {
        [JsonProperty("numFound")]
        public long NumFound { get; set; } = 0;

        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("numFoundExact")]
        public bool NumFoundExact { get; set; }

        [JsonProperty("num_found")]
        public long BookSearchResultsNumFound { get; set; }

        [JsonProperty("documentation_url")]
        public Uri? DocumentationUrl { get; set; }

        [JsonProperty("q")]
        public string? Q { get; set; }

        [JsonProperty("offset")]
        public object? Offset { get; set; }

        [JsonProperty("docs")]
        public List<BookDoc>? Docs { get; set; }
    }

    public class BookDoc
    {
        [JsonProperty("author_key")]
        public List<string>? AuthorKey { get; set; }

        [JsonProperty("author_name")]
        public List<string>? AuthorName { get; set; }

        [JsonProperty("cover_edition_key")]
        public string? CoverEditionKey { get; set; }

        [JsonProperty("cover_i")]
        public long CoverI { get; set; }

        [JsonProperty("ebook_access")]
        public string? EbookAccess { get; set; }

        [JsonProperty("edition_count")]
        public long EditionCount { get; set; }

        [JsonProperty("first_publish_year")]
        public long FirstPublishYear { get; set; }

        [JsonProperty("has_fulltext")]
        public bool HasFulltext { get; set; }

        [JsonProperty("ia")]
        public List<string>? Ia { get; set; }

        [JsonProperty("ia_collection_s")]
        public string? IaCollectionS { get; set; }

        [JsonProperty("key")]
        public string? Key { get; set; }

        [JsonProperty("language")]
        public List<string>? Language { get; set; }

        [JsonProperty("lending_edition_s", NullValueHandling = NullValueHandling.Ignore)]
        public string? LendingEditionS { get; set; }

        [JsonProperty("lending_identifier_s", NullValueHandling = NullValueHandling.Ignore)]
        public string? LendingIdentifierS { get; set; }

        [JsonProperty("public_scan_b")]
        public bool PublicScanB { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("subtitle", NullValueHandling = NullValueHandling.Ignore)]
        public string? Subtitle { get; set; }

        [JsonProperty("subject", NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? Subject { get; set; }
    }

    public partial class BookSearchResults
    {
        public static BookSearchResults? FromJson(string? json)
        {
            return string.IsNullOrEmpty(json) ? new BookSearchResults() : JsonConvert.DeserializeObject<BookSearchResults>(json) ;
        }
    }

    public static class Serialize
    {
        public static string? ToJson(this BookSearchResults self) => JsonConvert.SerializeObject(self);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}