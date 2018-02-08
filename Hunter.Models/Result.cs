using System;

namespace Hunter.Models
{
    public class Result
    {
        public static DataResult<T> CreateDataResult<T>(T t)
        {
            return new DataResult<T>() { Data = t };
        }

        public static InvalidResult<T> CreateInvalidResult<T>(T t)
        {
            return new InvalidResult<T>() { Code = Code.Fail, Data = t };
        }

        public static Result Create(Code code = Code.Success, string message = null)
        {
            return new Result(code, message);
        }

        public static string TranslateCode(Code code)
        {
            return code.ToString();
        }

        public Result() : this(Code.Success, null) { }

        public Result(Code code) : this(code, null) { }

        public Result(string message) : this(Code.Success, message) { }

        public Result(Code code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        public bool Success
        {
            get
            {
                return (int)this.Code >= 0;
            }
        }

        public Code Code { get; set; }

        private string _message = null;

        public string Message
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this._message))
                    return this.Code.ToString();
                return this._message;
            }
            set
            {
                this._message = value;
            }
        }

    }
}
