namespace ConceptArchitect.BookManagement
{
    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }

        //[ForeignKey(To=typeof(Author))]
        public string AuthorId { get; set; }
        public Author Author { get; set; }
        public List<string> Isbn { get; set; }=new List<string>();
        public double Price { get; set; }

        public double Rating { get;  }

        public string Cover { get; set; }

        public string Description { get; set; }

        public List<string> Tags { get; set; }


    }
}