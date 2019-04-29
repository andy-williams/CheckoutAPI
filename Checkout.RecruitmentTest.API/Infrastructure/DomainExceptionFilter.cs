using System.Net;
using System.Text;
using Checkout.RecruitmentTest.API.DomainExceptions;
using Checkout.RecruitmentTest.API.Handlers.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Checkout.RecruitmentTest.API.Infrastructure
{
    public class DomainExceptionFilter : IExceptionFilter
    {
        public async void OnException(ExceptionContext context)
        {
            if (!(context.Exception is DomainException))
                return;

            if (!context.ExceptionHandled)
            {
                if (context.Exception is DuplicateBasketItemException)
                {
                    var response = context.HttpContext.Response;
                    response.ContentType = "application/json";
                    response.StatusCode = (int) HttpStatusCode.BadRequest;
                    await response.WriteAsync(string.Empty, Encoding.UTF8);
                }

                if (context.Exception is BasketUnavailableException || context.Exception is BasketItemUnavailableException)
                {
                    var response = context.HttpContext.Response;
                    response.ContentType = "application/json";
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    await response.WriteAsync(string.Empty, Encoding.UTF8);
                }

                context.ExceptionHandled = true;
            }
        }
    }
}
