using Common.Exception;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace eCommerceApi
{
    internal class CustomExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var vex = context.Exception as ValidationException;

            if (vex != null)
                context.Result = new ValidationErrorResult(context.Request, vex.Message);

            base.Handle(context);
        }

        private class ValidationErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { get; set; }

            public string Content { get; set; }

            public ValidationErrorResult(HttpRequestMessage req, string msg)
            {
                Request = req;
                Content = msg;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response =
                                 new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent(Content);
                response.RequestMessage = Request;
                return Task.FromResult(response);
            }
        }
    }
}