using System;
using System.Net;

namespace librato4net
{
    public interface IWebClient : IDisposable
    {
        ICredentials Credentials { get; set; }
        WebHeaderCollection Headers { get; }
        void UploadString(string url, string data);
    }
}