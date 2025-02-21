using Microsoft.EntityFrameworkCore;
using MyBookShelf.Models;
using MyBookShelf.Repositories.BookRroviders;
using MyBookShelf.Repositories.ShelfProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookShelf.Services
{

    public class Creator : ICreator
    {
        public readonly IShelfProviders _shelfProviders;
        public readonly IBookProviders _bookProviders;
        public Creator(IShelfProviders shelfProviders , IBookProviders bookProviders)
        {
            _shelfProviders = shelfProviders;
            _bookProviders = bookProviders;
        }

        public async Task<Shelf> CreateShelfAsync(string nameShelf, string description)
        {
            var shelf = new Shelf()
            {
                Name = nameShelf,
                Description = description
            };
            await _shelfProviders.AddAsync(shelf);
            return shelf;
        }

        public async Task<Book> CreateBookAsync(string title, int countPages,  int shelfId ,string author = "", string description = "", string pathImg = "", int rating = 0)
        {
            var book = new Book
            {
                Title = title,
                Author = author,
                Description = description,
                CountPages = countPages,
                PublicationDate = DateTime.Now,
                PathImg = pathImg,
                Rating = rating,
                IdShelf = shelfId, 
            };

           
            await _bookProviders.AddAsync(book);

            return book;
        }

    }
}
