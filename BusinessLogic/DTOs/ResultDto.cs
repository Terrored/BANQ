namespace BusinessLogic.DTOs
{
    public class ResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class ResultDto<T> : ResultDto
    {
        public T Data { get; set; }
    }
}
