using System;
using System.Net;

namespace librato4net
{
    public class WebClientAdapter : IWebClient
    {
        private readonly WebClient _webClient;
        private bool _disposed;

        public WebClientAdapter()
        {
            _webClient = new WebClient();
        }

        public ICredentials Credentials
        {
            get
            {
                return _webClient.Credentials;
            }
            set
            {
                _webClient.Credentials = value;
            }
        }

        public WebHeaderCollection Headers
        {
            get
            {
                return _webClient.Headers;
            }
        }

        public void UploadString(string url, string data)
        {
            _webClient.UploadString(url, data);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~WebClientAdapter()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _webClient.Dispose();
            }

            _disposed = true;
        }
    }
}