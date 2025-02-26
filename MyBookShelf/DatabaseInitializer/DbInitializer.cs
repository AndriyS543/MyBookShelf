using MyBookShelf.Models;
using MyBookShelf.Repositories.GenreRroviders;

namespace MyBookShelf.DatabaseInitializer
{
    public static class DbInitializer
    {
        public static async Task InitializeGenres(IGenreProviders genreProviders)
        {
            // Перевірка наявності жанрів в базі даних
            var existingGenres = await genreProviders.GetAllAsync();

            // Якщо жанри ще не додані (список порожній)
            if (!existingGenres.Any())
            {
                // Список жанрів, які потрібно додати
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


                // Додавання жанрів у базу даних
                foreach (var genre in genresToAdd)
                {
                    await genreProviders.AddAsync(genre);
                }
            }
        }
    }

}
