using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Domain.Base
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public string Error { get; }

        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, T value, string error)
        {
            if (isSuccess && error != string.Empty)
                throw new InvalidOperationException("No puede tener éxito y un mensaje de error");
            if (!isSuccess && error == string.Empty)
                throw new InvalidOperationException("Los fallos deben incluir un mensaje de error");

            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static Result<T> Ok(T value) => new Result<T>(true, value, string.Empty);
        public static Result<T> Fail(string error) => new Result<T>(false, default(T), error);

        public static implicit operator Result<T>(T value) => Ok(value);

        public Result<TResult> Bind<TResult>(Func<T, Result<TResult>> func)
        {
            return IsSuccess
                ? func(Value)
                : Result<TResult>.Fail(Error);
        }

        public Result<TResult> Map<TResult>(Func<T, TResult> func)
        {
            return IsSuccess
                ? Result<TResult>.Ok(func(Value))
                : Result<TResult>.Fail(Error);
        }

        public void Match(Action<T> onSuccess, Action<string> onFailure)
        {
            if (IsSuccess)
                onSuccess(Value);
            else
                onFailure(Error);
        }

        public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<string, TResult> onFailure)
        {
            return IsSuccess
                ? onSuccess(Value)
                : onFailure(Error);
        }


        public async Task<Result<TResult>> BindAsync<TResult>(Func<T, Task<Result<TResult>>> asyncFunc)
        {
            return IsSuccess
                ? await asyncFunc(Value).ConfigureAwait(false)
                : Result<TResult>.Fail(Error);
        }

        // Map asíncrono (para Func<T, Task<TResult>>)
        public async Task<Result<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> asyncFunc)
        {
            if (!IsSuccess)
                return Result<TResult>.Fail(Error);

            var result = await asyncFunc(Value).ConfigureAwait(false);
            return Result<TResult>.Ok(result);
        }

        // Match asíncrono
        public async Task MatchAsync(
            Func<T, Task> onSuccessAsync,
            Func<string, Task> onFailureAsync)
        {
            if (IsSuccess)
                await onSuccessAsync(Value).ConfigureAwait(false);
            else
                await onFailureAsync(Error).ConfigureAwait(false);
        }

        // Match asíncrono con retorno
        public async Task<TResult> MatchAsync<TResult>(
            Func<T, Task<TResult>> onSuccessAsync,
            Func<string, Task<TResult>> onFailureAsync)
        {
            return IsSuccess
                ? await onSuccessAsync(Value).ConfigureAwait(false)
                : await onFailureAsync(Error).ConfigureAwait(false);
        }
    }
}
