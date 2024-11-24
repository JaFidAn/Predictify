namespace Prediction.Application.Exceptions
{
    public class AlreadyExistsException : InternalServerException
    {
        public AlreadyExistsException(string message) : base(message)
        {
        }
    }
}
