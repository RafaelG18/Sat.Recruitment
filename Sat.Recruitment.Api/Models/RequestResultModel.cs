namespace Sat.Recruitment.Api.Models
{
    /// <summary>
    /// Represents a request result model for api request
    /// </summary>
    public class RequestResultModel<T>
    {
        public T Result { get; set; }

        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }
    }
}