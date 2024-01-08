﻿namespace Common.Utils.Exceptions
{
    public class HttpResponseException : Exception
    {
        public int Status { get; set; }
        public string Error {get;set;}
        public object Value { get; set; }
    }
}
