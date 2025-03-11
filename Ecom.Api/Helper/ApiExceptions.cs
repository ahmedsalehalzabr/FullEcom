namespace Ecom.Api.Helper
{
    public class ApiExceptions : ResponseAPI
    {
        public ApiExceptions(int statusCode, string message = null, string detailes = null) : base(statusCode, message)
        {
            Detailes = detailes;
        }
        public string Detailes { get; set; }
    }
}
