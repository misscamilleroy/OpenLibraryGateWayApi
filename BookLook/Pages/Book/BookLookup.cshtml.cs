using BookLook.Classes;
using BookLook.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BookLook.Pages.Book
{
    public class BookLookupModel : PageModel
    {
        private readonly ILogger<BookLookupModel> _logger;
        private readonly IGatewayApiService _openLibraryService;
        private readonly int _defaultMaxRecordsPerPage = 100;

        [BindProperty]
        [Required]
        public string authorname { get; set; }

        [BindProperty]
        [Required]
        public int pageSelected { get; set; }

        public BookLookupModel(ILogger<BookLookupModel> logger, IGatewayApiService openLibraryService)
        {
            _logger = logger;
            _openLibraryService = openLibraryService;
            authorname = "";
            pageSelected = 1;
        }

        public async Task OnGetAsync(string authorname, int pageSelected)
        {
            if (pageSelected == 0)
            {
                pageSelected = 1;
            }
            ViewData["currentSearch"] = authorname;
            ViewData["requestedPage"] = pageSelected;
            ViewData["pageCount"] = 0;

            try
            {
                var searchResultset = await _openLibraryService.GetBooksByAuthor(authorname, pageSelected);
                ViewData["SearchResults"] = searchResultset ?? new BookSearchResults();
                if (searchResultset != null)
                {
                    ViewData["Docs"] = searchResultset.Docs ?? [];
                    var pageCount = ((searchResultset?.Docs?.Count > 0) ? Math.Ceiling(((double)searchResultset.NumFound + 1) / _defaultMaxRecordsPerPage) : 0);
                    ViewData["pageCount"] = (int) pageCount;
                }
            }
            catch 
            {
                ViewData["SearchResults"] = new BookSearchResults();
                ViewData["Docs"] = new List<BookDoc> ();
            }            
        }
    }
}
