using AOPExample.Application.Attributes;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOPExample.Application.Interceptors
{
    public class InterceptorBaseAsync<TAttribute> : IAsyncInterceptor where TAttribute : AttributeBase
    {
        protected virtual void OnBefore(IInvocation invocation, TAttribute attribute) { }
        protected virtual void OnAfter(IInvocation invocation, TAttribute attribute) { }
        protected virtual void OnException(IInvocation invocation, Exception ex, TAttribute attribute) { }
        protected virtual void OnSuccess(IInvocation invocation, TAttribute attribute) { }

        void IAsyncInterceptor.InterceptAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptAsynchronous(invocation);
        }

        void IAsyncInterceptor.InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
        }

        void IAsyncInterceptor.InterceptSynchronous(IInvocation invocation)
        {
            var attribute = GetAttribute(invocation.MethodInvocationTarget, invocation.TargetType);
            if (attribute is null) invocation.Proceed();
            else
            {
                var succeeded = true;
                OnBefore(invocation, attribute);
                try
                {
                    invocation.Proceed();

                }
                catch (Exception ex)
                {
                    succeeded = false;
                    OnException(invocation, ex, attribute);
                    throw;
                }
                finally
                {
                    if (succeeded) OnSuccess(invocation, attribute);
                }
                OnAfter(invocation, attribute);
            }
        }



        private async Task InternalInterceptAsynchronous(IInvocation invocation)
        {
            var attribute = GetAttribute(invocation.MethodInvocationTarget, invocation.TargetType);
            if (attribute is null)
            {
                invocation.Proceed();
                var task = (Task)invocation.ReturnValue;
                await task;
            }
            else
            {
                var succeeded = true;
                OnBefore(invocation, attribute);
                try
                {
                    invocation.Proceed();
                    var task = (Task)invocation.ReturnValue;
                    await task;
                }
                catch (Exception ex)
                {
                    succeeded = false;
                    OnException(invocation, ex, attribute);
                    throw;
                }
                finally
                {
                    if (succeeded) OnSuccess(invocation, attribute);
                }
                OnAfter(invocation, attribute);
            }

        }
        private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            var attribute = GetAttribute(invocation.MethodInvocationTarget, invocation.TargetType);
            TResult result;
            if (attribute is null)
            {
                invocation.Proceed();
                var task = (Task<TResult>)invocation.ReturnValue;
                result = await task;
            }
            else
            {
                var succeeded = true;

                OnBefore(invocation, attribute);
                try
                {
                    invocation.Proceed();
                    var task = (Task<TResult>)invocation.ReturnValue;
                    result = await task;

                }
                catch (Exception ex)
                {
                    succeeded = false;
                    OnException(invocation, ex, attribute);
                    throw;
                }
                finally
                {
                    if (succeeded) OnSuccess(invocation, attribute);
                }
                OnAfter(invocation, attribute);

            }
            return result;
        }
        private TAttribute? GetAttribute(MethodInfo methodInfo, Type type)
        {
            var attribute = methodInfo.GetCustomAttribute<TAttribute>(true);//true kalıtım aldığı sınıflarıda dikkate alır
            if (attribute is not null) return attribute;
            return type.GetTypeInfo().GetCustomAttribute<TAttribute>(true);
        }

    }
}
