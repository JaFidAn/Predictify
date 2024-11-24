namespace Prediction.Application.Exceptions
{
    public class ObjectNotFoundException : NotFoundException
    {
        public ObjectNotFoundException(Guid id, string name = null) : base(name is null
               ? $"Object with ID '{id}' was not found."
               : $"{name} with ID '{id}' was not found.")
        { }
    }
}
