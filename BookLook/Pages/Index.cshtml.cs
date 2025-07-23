using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookLook.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        ViewData["Title"] = "Book Search";

    }
    public async Task<ActionResult> OnPost()
    {
        if (!Request.HasFormContentType)
            return new StatusCodeResult(415);
        var form = await Request.ReadFormAsync();
        if (!form.Any())
             return new StatusCodeResult(415);
        string author = Convert.ToString( form["authorname"]);
        return RedirectToPage("/book/Booklookup", new { authorname = author, pageSelected = "1" });
    }
}
