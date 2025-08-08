using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Domain.Base
{
    public static class ResultExtensions
    {
        public static async Task<Result<TResult>> BindAsync<T, TResult>(
            this Task<Result<T>> taskResult,
            Func<T, Task<Result<TResult>>> asyncFunc)
        {
            var result = await taskResult.ConfigureAwait(false);
            return await result.BindAsync(asyncFunc).ConfigureAwait(false);
        }

        public static async Task<Result<TResult>> MapAsync<T, TResult>(
            this Task<Result<T>> taskResult,
            Func<T, Task<TResult>> asyncFunc)
        {
            var result = await taskResult.ConfigureAwait(false);
            return await result.MapAsync(asyncFunc).ConfigureAwait(false);
        }

        public static async Task<Result<T>> TapAsync<T>(
            this Result<T> result,
            Func<T, Task> asyncAction)
        {
            if (result.IsSuccess)
            {
                await asyncAction(result.Value).ConfigureAwait(false);
            }
            return result;
        }
    }
}
