using ConceptArchitect.BookManagement;
using ConceptArchitect.Utils;

namespace BooksWebApp.ViewModels
{
    public class AuthorEditVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public string Photo { get; set; }

        public string Tags { get; set; }
    }

    public static class AuthorVMExtensions
    {
        public static Author ToAuthor(this AuthorEditVM vm)
        {
            return new Author().Copy(vm, (m, vm) => m.Tags = vm.Tags.Split(',').ToList());
        }

        public static AuthorEditVM ToAuthorVM(this Author author)
        {
            return new AuthorEditVM().Copy(author, (vm, m) => vm.Tags = String.Join(",", m.Tags));
        }

    }
}