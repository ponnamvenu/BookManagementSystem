using System.Runtime.Serialization;

namespace ConceptArchitect.BookManagement.Repositories.AdoNet
{
    [Serializable]
    public class  DuplicateEntitityException<T> : Exception
    {
        public T Id { get; private set; }


        public DuplicateEntitityException(T id, string message, Exception innerException = null) : base(message, innerException)
        {
            Id = id;    
        }

     
    }
}