using EarthApi.Enums;
using EarthApi.Models;
using EarthApi.Servicies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EarthApi.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILoggerService _loggerService;
        public ExceptionFilter(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }
        public void OnException(ExceptionContext context)
        {
            var response = new EarthApiResponse<EarthApiResponseBase>(new EarthApiResponseBase
            {
                ErrorCode = EnumEarthApiErrorCode.Exception,
                ExtraMessage = context.Exception.Message
            });

            _loggerService.Error($"Exception caught in ExceptionFilter: {context.Exception.Message}\n{context.Exception.StackTrace}");

            context.Result = new JsonResult(response)
            {
                StatusCode = 200
            };
            context.ExceptionHandled = true;
        }
    }
}
