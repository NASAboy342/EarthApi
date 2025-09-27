using EarthApi.Enums;

namespace EarthApi.Models
{
    public class EarthApiResponse<TResponseObject> where TResponseObject : EarthApiResponseBase
    {
        public EarthApiResponse(TResponseObject responseData)
        {
            ResponseData = responseData;
            ErrorCode = responseData.ErrorCode;
            ExtraMessage = responseData.ExtraMessage;
        }
        public TResponseObject ResponseData { get; set; }
        public EnumEarthApiErrorCode ErrorCode { get; set; }
        public string ErrorMessage => ErrorCode.ToString();
        public string ExtraMessage { get; set; } = string.Empty;
    }

    public class EarthApiResponseBase
    {
        public EnumEarthApiErrorCode ErrorCode { get; set; } = EnumEarthApiErrorCode.Success;
        public string ErrorMessage => ErrorCode.ToString();
        public string ExtraMessage { get; set; } = string.Empty;
    }
}
