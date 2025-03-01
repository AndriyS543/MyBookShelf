using MyBookShelf.Models;
using MyBookShelf.Repositories.GenreRroviders;

namespace MyBookShelf.DatabaseInitializer
{
    /// <summary>
    /// Provides methods to initialize the database with predefined genres.
    /// </summary>
    public static class DbInitializer
    {
        public static async Task InitializeGenres(IGenreProviders genreProviders)
        {
            /// <summary>
            /// Initializes the database with a predefined list of genres if none exist.
            /// </summary>
            var existingGenres = await genreProviders.GetAllAsync();

            // Check if genres already exist in the database
            if (!existingGenres.Any())
            {
                // List of genres to be added
                var genresToAdd = new List<Genre>
                {
                    new Genre { Name = "Fantasy" },
                    new Genre { Name = "Science" },
                    new Genre { Name = "Mystery" },
                    new Genre { Name = "Romance" },
                    new Genre { Name = "Horror" },
                    new Genre { Name = "Historical" },
                    new Genre { Name = "Thriller" },
                    new Genre { Name = "Adventure" },
                    new Genre { Name = "Drama" },
                    new Genre { Name = "Comedy" },
                    new Genre { Name = "Dystopian" },
                    new Genre { Name = "Cyberpunk" },
                    new Genre { Name = "Steampunk" },
                    new Genre { Name = "Crime" },
                    new Genre { Name = "Detective" },
                    new Genre { Name = "Poetry" },
                    new Genre { Name = "Self-help" },
                    new Genre { Name = "Biography" },
                    new Genre { Name = "Autobiography" },
                    new Genre { Name = "Psychology" },
                    new Genre { Name = "Philosophy" },
                    new Genre { Name = "War" }
                };

                // Add genres to the database
                foreach (var genre in genresToAdd)
                {
                    await genreProviders.AddAsync(genre);
                }
            }
        }
    }

}
