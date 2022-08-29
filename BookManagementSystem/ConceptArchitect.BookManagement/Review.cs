namespace ConceptArchitect.BookManagement
{
    public class Review
    {
        public int Id { get; set; }
        public User Reviewer { get; set; }
        public Book Book { get; set; }
        public int Rating { get; set; }

        public string ReviewText { get; set; }

        public string ReviewTitle { get; set; }

        public string ReviewerEmail { get; set; }

        public string BookId { get; set; }

    }
}