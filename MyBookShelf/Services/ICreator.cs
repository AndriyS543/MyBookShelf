using MyBookShelf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookShelf.Services
{
    public interface ICreator
    {
        Task<Shelf> CreateShelfAsync(string nameShelf, string description);
        Task<Book> CreateBookAsync(string title, int countPages, int shelfId,string author, string description, string pathImg , int rating);
    }
}
