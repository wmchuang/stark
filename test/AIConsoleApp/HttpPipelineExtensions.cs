using Azure;
using Azure.Core;
using Azure.Core.Pipeline;

namespace AIModule;

public static class HttpPipelineExtensions
{
    public static async ValueTask<Response> ProcessMessageAsync(this HttpPipeline pipeline, HttpMessage message, RequestContext? requestContext,
        CancellationToken cancellationToken = default)
    {
        var (userCt, statusOption) = ApplyRequestContext(requestContext);
        if (!userCt.CanBeCanceled || !cancellationToken.CanBeCanceled)
        {
            await pipeline.SendAsync(message, cancellationToken.CanBeCanceled ? cancellationToken : userCt).ConfigureAwait(false);
        }
        else
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(userCt, cancellationToken);
            await pipeline.SendAsync(message, cts.Token).ConfigureAwait(false);
        }

        if (!message.Response.IsError || statusOption == ErrorOptions.NoThrow)
        {
            return message.Response;
        }

        throw new RequestFailedException(message.Response);
    }

    public static Response ProcessMessage(this HttpPipeline pipeline, HttpMessage message, RequestContext? requestContext, CancellationToken cancellationToken = default)
    {
        var (userCt, statusOption) = ApplyRequestContext(requestContext);
        if (!userCt.CanBeCanceled || !cancellationToken.CanBeCanceled)
        {
            pipeline.Send(message, cancellationToken.CanBeCanceled ? cancellationToken : userCt);
        }
        else
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(userCt, cancellationToken);
            pipeline.Send(message, cts.Token);
        }

        if (!message.Response.IsError || statusOption == ErrorOptions.NoThrow)
        {
            return message.Response;
        }

        throw new RequestFailedException(message.Response);
    }

    private static (CancellationToken CancellationToken, ErrorOptions ErrorOptions) ApplyRequestContext(RequestContext? requestContext)
    {
        if (requestContext == null)
        {
            return (CancellationToken.None, ErrorOptions.Default);
        }

        return (requestContext.CancellationToken, requestContext.ErrorOptions);
    }

    internal class ErrorResponse<T> : Response<T>
    {
        private readonly Response _response;
        private readonly RequestFailedException _exception;

        public ErrorResponse(Response response, RequestFailedException exception)
        {
            _response = response;
            _exception = exception;
        }

        public override T Value
        {
            get => throw _exception;
        }

        public override Response GetRawResponse() => _response;
    }
}