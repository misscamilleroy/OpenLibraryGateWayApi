using Xunit;
using OpenLibraryGateWayApi.Extensions;
using System.Text;
using OpenLibraryGateWayApi.Enums;

namespace OpenLibraryGateWayApi.Tests.ExtensionTests
{
    public class TestStringBuilderExtension
    {
        private string _searchResultFields = "key,title,cover_edition_key,author_key,author_name,edition_count,ebook_access,edition_count,first_publish_year,language,subject";

        [Fact]
        public void StringBuilderExtension_Add_Author_Works()
        {
            var testSb = new StringBuilder();
            testSb.AddQueryString(UriParameterName.author, "any");

            Assert.Equal("?author=any", testSb.ToString());
        }

        [Fact]
        public void StringBuilderExtension_Add_Page_Works()
        {
            var testSb = new StringBuilder();
            testSb.AddQueryString(UriParameterName.page, 1);

            Assert.Equal("?page=1", testSb.ToString());
        }

        [Fact]
        public void StringBuilderExtension_Add_Fields_Works()
        {
            var testSb = new StringBuilder();
            testSb.AddQueryString(UriParameterName.fields, _searchResultFields);
            Assert.Equal($"?fields={_searchResultFields}", testSb.ToString());
        }

        [Fact]
        public void StringBuilderExtension_Append_Author_Works()
        {
            var testSb = new StringBuilder();
            testSb.AppendQueryString(UriParameterName.author, "any");

            Assert.Equal("&author=any", testSb.ToString());
        }

        [Fact]
        public void StringBuilderExtension_Append_Page_Works()
        {
            var testSb = new StringBuilder();
            testSb.AppendQueryString(UriParameterName.page, 1);

            Assert.Equal("&page=1", testSb.ToString());
        }

        [Fact]
        public void StringBuilderExtension_Append_Fields_Works()
        {
            var testSb = new StringBuilder();
            testSb.AppendQueryString(UriParameterName.fields, _searchResultFields);
            Assert.Equal($"&fields={_searchResultFields}", testSb.ToString());
        }
    }
}
