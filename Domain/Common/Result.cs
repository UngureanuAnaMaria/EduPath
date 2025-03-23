namespace Domain.Common
{
    public class Result<T>
    {
        public T Data { get; }
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        protected Result(bool isSuccess, T data, string errorMessage)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
        }
        public static Result<T> Success(T data) => new(true, data, null!);
        public static Result<T> Failure(string errorMessage) => new(false, default!, errorMessage);
    }
}
