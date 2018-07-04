using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RazorPagesMovies.Pages.Movies
{
  public class IndexModel : PageModel
  {
    private readonly RazorPagesMovie.Models.MovieContext _context;

    public IndexModel(RazorPagesMovie.Models.MovieContext context)
    {
      _context = context;
    }

    public IList<Movie> Movie { get; set; }
    public SelectList Genres { get; set; }
    public string MovieGenre { get; set; }
    public string PriceFilter { get; set; }

    public async Task OnGetAsync(string movieGenre, string searchString, string price, string priceFilter)
    {
      IQueryable<string> genreQuery = from m in _context.Movie
                                      orderby m.Genre
                                      select m.Genre;

      var movies = from m in _context.Movie
                   select m;

      if (!String.IsNullOrEmpty(movieGenre))
        movies = movies.Where(x => x.Genre == movieGenre);

      if (!String.IsNullOrEmpty(searchString))
        movies = movies.Where(s => s.Title.Contains(searchString));

      if (!String.IsNullOrEmpty(price))
        if (priceFilter == "Lower")
        {
          movies = movies.Where(v => v.Price <= decimal.Parse(price));
        }
        else if (priceFilter == "Higher")
        {
          movies = movies.Where(v => v.Price >= decimal.Parse(price));
        }

      Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
      Movie = await movies.ToListAsync();
    }
  }
}
