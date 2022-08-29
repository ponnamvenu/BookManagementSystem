using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptArchitect.BookManagement
{
    public interface IReviewManager
    {
        public List<Review> GetAllReviewsByUser(string email);

        public List<Review> GetAllReviewsForBook(string bookId);    

        public List<Review> GetAllReviewsForAuthor(String authorId);

        public Review GetReviewById(int id);

        public Review AddReview(Review review);

        public void UpdateReview(int id,Review review);
        public void RemoveReview(int id);
    }
}
