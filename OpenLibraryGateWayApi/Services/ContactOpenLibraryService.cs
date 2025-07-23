using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using OpenLibraryGateWayApi.ApiModels;
using OpenLibraryGateWayApi.Enums;
using OpenLibraryGateWayApi.Extensions;

namespace OpenLibraryGateWayApi.Services
{
    public interface IContactOpenLibraryService
    {
        public Task<OpenLibrarySearchResult> GetOpenLibrarySearchResults(string authorName, int page = 1);

    }

    public class ContactOpenLibraryService : IContactOpenLibraryService
    {
        private string _openLibraryBaseUri = "https://openlibrary.org";
        private string _requestUriSearchJsonResults = "/search.json";
        private string _searchResultFields = "key,title,cover_edition_key,author_key,author_name,edition_count,ebook_access,edition_count,first_publish_year,language,subject";
        private readonly HttpClient _openLibraryClient;

        public ContactOpenLibraryService(HttpClient httpClient) { 
            _openLibraryClient =  httpClient;
            if (httpClient.BaseAddress == null)
            {
                _openLibraryClient.BaseAddress = new Uri(_openLibraryBaseUri);
            }
        }

        public async Task<OpenLibrarySearchResult> GetOpenLibrarySearchResults(string authorName, int page )
        {
            var queryString = new StringBuilder();
            if (page == 0)
            {
                page = 1;
            }
            queryString.Append(_requestUriSearchJsonResults);

            queryString.AddQueryString(UriParameterName.author, Uri.EscapeDataString(authorName));
            queryString.AppendQueryString(UriParameterName.page, page);
            queryString.AppendQueryString(UriParameterName.fields, _searchResultFields);

            try
            {
                var response = await _openLibraryClient.GetAsync(queryString.ToString());
                response.EnsureSuccessStatusCode(); // let's throw this
                //if (response.IsSuccessStatusCode)
                //{
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var results = JsonConvert.DeserializeObject<OpenLibrarySearchResult>(jsonString);
                    if (results != null && results.docs != null && results.docs.Length > 1)
                    { 
                        // Note: OpenLib does not have the option to sort by author name
                        //    therefore, the author may show up in any order
                        // this does not solve the challenge that we only receive 100 doc records at a time 
                        //     which is difficult when the author name is mixed throughout all of the pages
                        Array.Sort(results.docs, static (docA, docB) =>
                        {
                            if (docA.author_name == null && docB.author_name == null) return 0; // Both null, consider equal
                            if (docA.author_name == null ) return -1; // Empty DocA authorlist comes after DocB
                            if (docB.author_name == null) return 1;

                            var authorNamesDocA = String.Join(",", docA.author_name);
                            var authorNamesDocB = String.Join(",", docB.author_name);

                            if (authorNamesDocA.Length == 0 && authorNamesDocB.Length == 0) return 0; // Both null, consider equal
                            if (authorNamesDocA.Length == 0) return -1;  // Empty DocA authorlist comes after DocB
                            if (authorNamesDocB.Length == 0) return 1;


                            // Neither is null, compare their actual values
                            int nameCompare = string.Compare(authorNamesDocA, authorNamesDocB);
                            if (nameCompare !=0)
                            {
                                return nameCompare;
                            }

                            // author name(s) the same, now sort by publish year
                            return docA.first_publish_year.CompareTo(docB.first_publish_year);
                        });
                     }

            return results ?? new OpenLibrarySearchResult();
                //}
            }
            catch (HttpResponseException ex)
            {
                throw new Exception("OpenLibrary.org returned 503 - Service Unavailable", ex);
            }
            //return new OpenLibrarySearchResult();
        }
    }
}
