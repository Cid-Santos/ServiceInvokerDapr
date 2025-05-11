namespace Messaging.Core.Exceptions
{
    // MessageDeliveryException.cs
    public class MessageDeliveryException : Exception
    {
        public MessageDeliveryException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
