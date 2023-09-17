namespace Newsletter.Reporting.Shared
{
    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public IEnumerable<string>? ErrorList { get; set; } = default!;
    }
}
