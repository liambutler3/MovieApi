using Microsoft.AspNetCore.Mvc;
using MovieApi.Models;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {

        private readonly ILogger<MoviesController> _logger;

        public MoviesController(ILogger<MoviesController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Get")]
        public IEnumerable<Movie> GetMoviesByTitle(string title, int? limit = null, int? page = null, string? genre = "")
        {
            if (title == null) throw new ArgumentNullException(nameof(title));

            // get movies by title
            using (MovieContext db = new MovieContext())
            {
                var movies = db.Movies.Where(m => m.Title.Contains(title)).ToList();

                if (!string.IsNullOrWhiteSpace(genre))
                {
                    genre = genre.Trim().ToLower();
                    var movieMatchesGenre = false;

                    if (movies.Any())
                    {
                        var moviesThatMatchGenre = new List<Movie>();
                        foreach (var movie in movies)
                        {
                            var movieGenres = movie.Genre?.Split(',');

                            // trim whitespace from each genre
                            if (movieGenres != null)
                                for (int i = 0; i < movieGenres.Length; i++)
                                {
                                    movieGenres[i] = movieGenres[i].Trim().ToLower();
                                }

                            foreach (var movieGenre in movieGenres)
                            {
                                // genre already matched to the movie
                                if (movieMatchesGenre) break;

                                if (movieGenre == genre)
                                {
                                    movieMatchesGenre = true;
                                    break;
                                }
                            }

                            if (movieMatchesGenre)
                            {
                                if (!moviesThatMatchGenre.Contains(movie))
                                {
                                    moviesThatMatchGenre.Add(movie);
                                }
                                movieMatchesGenre = false;
                            }
                            else
                            {
                                // reset movieMatchesGenre for next movie
                                movieMatchesGenre = false;
                            }
                        }

                        movies = moviesThatMatchGenre;
                    }
                }

                if (page != null)
                {
                    page = page - 1;

                    if (limit != null)
                    {
                        movies = movies.Skip((int)page * (int)limit).Take((int)limit).ToList();
                    }
                }

                if (limit != null)
                {
                    if (movies.Count() > limit)
                    {
                        movies = movies.Take((int)limit).ToList();
                    }
                }

                return movies;
            }
        }
    }
}