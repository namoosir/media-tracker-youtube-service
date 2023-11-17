namespace MediaTrackerYoutubeService.Models
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;

        public static ServiceResponse<T> Build(T? data, bool? success, string? message)
        {
            var serviceResponse = new ServiceResponse<T>
            {
                Data = data,
                Success = success ?? true,
                Message = message ?? string.Empty
            };
            return serviceResponse;
        }
    }
}
