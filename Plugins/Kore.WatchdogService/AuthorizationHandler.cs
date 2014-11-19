using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Kore.WatchdogService
{
    public class AuthorizationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!ValidateCredentials(request))
            {
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                var tcs = new TaskCompletionSource<HttpResponseMessage>();
                tcs.SetResult(response);
                return tcs.Task;
            }
            return base.SendAsync(request, cancellationToken);
        }

        private bool ValidateCredentials(HttpRequestMessage message)
        {
            var query = message.RequestUri.ParseQueryString();
            string password = query["password"];
            return (password == Global.APIPassword);
        }
    }
}