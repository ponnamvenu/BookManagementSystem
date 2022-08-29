using ConceptArchitect.Utils;

namespace ConceptArchitect.BookManagement
{
    [Table(Name ="authors")]
    public class Author
    {
        [Column(Name="Id", DataType ="varchar(50)")]
        public string Id { get; set; }

        
        public string Name { get; set; }
        public string Photo { get; set; }
        
        [Column(DataType ="varchar(2000)")]
        public string Biography { get; set; }

        [Column(DataType ="varchar(100)")]
        public List<string> Tags { get; set; }

        [Column(Exclude =true)]
        public List<Book> Books { get; set; } = new List<Book>();

        [Column(Exclude =true)]
        public double Rating { get;  }

    }
}