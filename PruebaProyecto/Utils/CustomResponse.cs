namespace PruebaProyecto.Utils
{
    public class CustomResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T? Content { get; set; }
    }
}
