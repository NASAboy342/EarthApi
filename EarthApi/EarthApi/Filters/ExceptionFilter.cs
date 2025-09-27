using EarthApi.Enums;
using EarthApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EarthApi.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var response = new EarthApiResponse<EarthApiResponseBase>(new EarthApiResponseBase
            {
                ErrorCode = EnumEarthApiErrorCode.Exception,
                ExtraMessage = context.Exception.Message
            });

            context.Result = new JsonResult(response)
            {
                StatusCode = 200
            };
            context.ExceptionHandled = true;
        }
    }
}
