namespace OpenLibraryGateWayApi.ApiModels
{
    public class OpenLibrarySearchResult
    {
        public int numFound { get; set; }
        public int start { get; set; }
        public bool numFoundExact { get; set; }
        public int num_found { get; set; }
        public string? documentation_url { get; set; }
        public string? q { get; set; }
        public object? offset { get; set; }
        public Doc[]? docs { get; set; }
    }

    public class Doc
    {
        public string[]? author_key { get; set; }
        public string[]? author_name { get; set; }
        public string[]? subject { get; set; }
        public string? cover_edition_key { get; set; }
        public int cover_i { get; set; }
        public string? ebook_access { get; set; }
        public int edition_count { get; set; }
        public int first_publish_year { get; set; }
        public bool has_fulltext { get; set; }
        public string[]? ia { get; set; }
        public string? ia_collection_s { get; set; }
        public string? key { get; set; }
        public string[]? language { get; set; }
        public string? lending_edition_s { get; set; }
        public string? lending_identifier_s { get; set; }
        public bool public_scan_b { get; set; }
        public string? title { get; set; }
    }
}

