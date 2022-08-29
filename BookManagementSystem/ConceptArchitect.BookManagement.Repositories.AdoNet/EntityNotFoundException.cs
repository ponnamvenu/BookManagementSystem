using System.Runtime.Serialization;

namespace ConceptArchitect.BookManagement.Repositories.AdoNet
{
    [Serializable]
    public class EntityNotFoundException<T> : Exception
    {
        public T Id { get; private set; }

        
        public EntityNotFoundException(T id, string message):base(message)
        {
            this.Id = id;
            
        }

        public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}