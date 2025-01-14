namespace Application.Models
{
    public class ResponseCore<T>
    {
        public int StatusCode { get; set; }
        public object[] ErrorMessage { get; set; }
        public T Result { get; set; }
    }
}
