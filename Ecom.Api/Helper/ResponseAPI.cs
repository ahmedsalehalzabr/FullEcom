namespace Ecom.Api.Helper
{
    public class ResponseAPI
    {
        public ResponseAPI(int statusCode, string message= null)
        {
            StatusCode = statusCode;
            Message = message?? GetMessageFormStatusCode(StatusCode);
        }
        private string GetMessageFormStatusCode(int statuscode)
        {
            return statuscode switch
            {
                200 => "Done",
                400 => "Bad Request",
                401 => "Un Authorized",
                404 => "Not Found Res",
                500 => "Server Error",
                _ => null,
            };
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
//namespace Ecom.Api.Helper
//{
//    public class ResponseAPI
//    {
//        public int StatusCode { get; set; }
//        public string? Message { get; set; }
//        public string? Detailes { get; set; } // ✅ إضافة الخاصية

//        // Constructor الأساسي
//        public ResponseAPI(int statusCode, string? message = null)
//        {
//            StatusCode = statusCode;
//            Message = message ?? GetMessageFormStatusCode(StatusCode);
//        }
//        public ResponseAPI()
//        {
//            StatusCode = 200;
//            Message = "Done";
//        }


//        // ✅ Constructor ثاني يسمح بإضافة تفاصيل الخطأ
//        public ResponseAPI(int statusCode, string? message, string? detailes)
//        {
//            StatusCode = statusCode;
//            Message = message ?? GetMessageFormStatusCode(StatusCode);
//            Detailes = detailes;
//        }

//        private string GetMessageFormStatusCode(int statuscode)
//        {
//            return statuscode switch
//            {
//                200 => "Done",
//                400 => "Bad Request",
//                401 => "Un Authorized",
//                404 => "Not Found Res",
//                500 => "Server Error",
//                _ => "Unexpected Error"
//            };
//        }
//    }
//}
