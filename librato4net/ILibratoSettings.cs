using System;


namespace librato4net
{
    public interface ILibratoSettings
    {
        string Username { get; }

        string ApiKey { get; }

        Uri ApiEndpoint { get; }

        TimeSpan SendInterval { get; }
    }
}
