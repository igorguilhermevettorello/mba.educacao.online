namespace MBA.Educacao.Online.Core.Application.Models
{
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }

        public Result()
        {
            Errors = new List<string>();
        }

        public Result(bool success, string message)
        {
            Success = success;
            Message = message;
            Errors = new List<string>();
        }

        public static Result Ok(string message = "Operação realizada com sucesso")
        {
            return new Result(true, message);
        }

        public static Result Fail(string message, List<string> errors = null)
        {
            return new Result(false, message)
            {
                Errors = errors ?? new List<string>()
            };
        }

        public static Result<T> Ok<T>(T data, string message = "Operação realizada com sucesso")
        {
            return new Result<T>(true, message, data);
        }

        public static Result<T> Fail<T>(string message, List<string> errors = null)
        {
            return new Result<T>(false, message, default)
            {
                Errors = errors ?? new List<string>()
            };
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }

        public Result() : base()
        {
        }

        public Result(bool success, string message, T data) : base(success, message)
        {
            Data = data;
        }
    }
}

