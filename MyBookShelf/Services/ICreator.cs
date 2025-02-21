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
    }
}
