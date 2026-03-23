namespace DocuSense.Domain.Errors
{
    public class Error
    {
        public ErrorType Type { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }

        private Error(string code, string message, ErrorType type)
        {
            Code = code;
            Message = message;
            Type = type;
        }

        public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);

        public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);

        public static Error Unauthorized(string code, string message) => new(code, message, ErrorType.Unauthorized);

        public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);

        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
    }
}