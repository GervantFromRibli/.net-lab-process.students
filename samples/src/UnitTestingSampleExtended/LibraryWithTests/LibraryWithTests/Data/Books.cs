﻿using LibraryWithTests.Domain;
using System.Collections.Generic;

namespace LibraryWithTests.Data
{
    public static class Books
    {
        public static List<Book> GetCurrentBooks = new List<Book>
        {
            new Book { Id = 1, Name = "The Lord of the Rings" },
            new Book { Id = 2, Name = "Le Petit Prince" },
            new Book { Id = 3, Name = "Harry Potter and the Philosopher's Stone" },
        };
    }
}
