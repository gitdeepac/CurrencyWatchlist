using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Helpers
{
	public class ApiResponse<T>
	{
		public String Status { get; set; } = string.Empty;
		public int StatusCode { get; set; }
		public String Message { get; set; } = string.Empty;
		public T? Data { get; set; }

		public static ApiResponse<T> Success(T data, string message = "Success", int statusCode = 200)
		{
			return new ApiResponse<T>
			{
				Status = "Success",
				StatusCode = statusCode,
				Message = message,
				Data = data
			};
		}

		public static ApiResponse<T> Error(T Data, string message, int statusCode)
		{

			return new ApiResponse<T>
			{
				Status = "Error",
				StatusCode = statusCode,
				Message = message,
				Data = default
			};
		}
		public static ApiResponse<T> NotFound(string message = "Resource not found.")
		{
			return new ApiResponse<T>
			{
				Status = "Error",
				StatusCode = 404,
				Message = message,
				Data = default
			};
		}
		public static ApiResponse<T> ServerError(string message = "An unexpected error occurred.")
		{
			return new ApiResponse<T>
			{
				Status = "Error",
				StatusCode = 500,
				Message = message,
				Data = default
			};
		}
	}
}