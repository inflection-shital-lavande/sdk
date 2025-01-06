namespace Common
{
    public class APIError : Exception { }

    public class AuthenticationError : APIError { }

    public class NotFoundError : APIError { }

    public class ValidationError : APIError { }
}
