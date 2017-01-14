namespace Examples.Validation.Api.Core
{
    public class ValidationError
    {
        #region Constants
        public static class Type
        {
            public static string Input = "input";
            public static string Domain = "domain";
        }
        #endregion

        #region Properties
        public string ErrorType {get; set;}
        public string ErrorCode {get; set;}
        public object Source {get; set;}
        public string ErrorMessage {get; set;}
        #endregion

        #region Constructor
        public ValidationError()
        {
        }

        public ValidationError(string type, string code, object source, string message)
        {
            ErrorType = type;
            ErrorCode = code;
            Source = source;
            ErrorMessage = message;
        }
        #endregion
    }
}
