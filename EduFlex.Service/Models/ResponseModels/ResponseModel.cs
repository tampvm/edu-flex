using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ResponseModels
{
    public class ResponseModel
    {
        [JsonPropertyOrder(1)]
        public bool Success { get; set; }

        [JsonPropertyOrder(2)]
        public string Message { get; set; }
    }

    public class SuccessResponseModel<T> : ResponseModel
    {
        [JsonPropertyOrder(3)]
        public T Data { get; set; }

        //public SuccessResponseModel(string message, T data)
        //{
        //    Success = true;
        //    Message = message;
        //    Data = data;
        //}
    }

    public class ErrorResponseModel<T> : ResponseModel
    {
        [JsonPropertyOrder(3)]
        public List<string> Errors { get; set; }

        //public ErrorResponseModel(string message, List<string> errors = null)
        //{
        //    Success = false;
        //    Message = message;
        //    Errors = errors ?? new List<string>();
        //}
    }
}
