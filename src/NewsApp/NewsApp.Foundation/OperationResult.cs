using System;
using System.Collections.Generic;
using System.Linq;

namespace NewsApp.Foundation
{
    public class OperationResult<TError> where TError : Enum
    {
        public bool IsSuccessful { get; }

        public IReadOnlyCollection<TError> Errors { get; }

        public static OperationResult<TError> Success { get; }


        static OperationResult()
        {
            Success = new OperationResult<TError>(true);
        }


        private OperationResult(bool isSuccessful, IReadOnlyCollection<TError> errors = null)
        {
            IsSuccessful = isSuccessful;
            Errors = errors ?? Array.Empty<TError>();
        }


        public static OperationResult<TError> Failed(TError error, params TError[] errors)
        {
            return Failed(errors.Append(error).ToList());
        }

        public static OperationResult<TError> Failed(IReadOnlyCollection<TError> errors)
        {
            if (errors == null || errors.Count == 0)
            {
                throw new ArgumentException("At least 1 error is required", nameof(errors));
            }

            var result = new OperationResult<TError>(false, errors);

            return result;
        }

        public override string ToString()
        {
            return IsSuccessful ?
                   "Succeeded" : $"Failed : {string.Join(",", Errors.ToList())}";
        }
    }
}