using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace OmegaLulChat.Auth;

//Todo: Denna gör samma som CookeHandler fast för SignalR, den hämtar vår authentication cookie och skickar med den när vi ansluter till Hubben.
public class IncludeRequestCredentialsMessageHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        return base.SendAsync(request, cancellationToken);
    }
}