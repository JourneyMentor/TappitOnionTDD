using Newtonsoft.Json;

namespace GlobalResultPattern.ExceptionResult
{
    public class ExceptionModel : ErrorStatusCode
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class ErrorStatusCode
    {
        public int StatusCode { get; set; }
    }
}
