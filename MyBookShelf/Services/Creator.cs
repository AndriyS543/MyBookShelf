using MyBookShelf.Models;
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
        public Creator(IShelfProviders shelfProviders)
        {
            _shelfProviders = shelfProviders;
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
    }
}
