// The MIT License
// 
// Copyright (c) 2012-2015 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using iSynaptic.Commons.Reflection;

namespace iSynaptic.Commons
{
    public abstract class MessageHandler
    {
        private static readonly Task _completedTask;

        private static readonly TypeHierarchyComparer _typeHierarchyComparer
            = new TypeHierarchyComparer();

        private static readonly ConcurrentDictionary<Type, Func<MessageHandler, object, Task>> _dispatchers
            = new ConcurrentDictionary<Type, Func<MessageHandler, object, Task>>();

        static MessageHandler()
        {
            _completedTask = Task.FromResult(true);
        }

        protected virtual Task OnHandle(Object message)
        {
            Guard.NotNull(message, "message");

            if (!ShouldHandle(message))
                return _completedTask;

            var dispatcher = GetDispatcher(GetType(), "On", _dispatchers);
            return dispatcher(this, message);
        }

        protected virtual bool ShouldHandle(Object message)
        {
            return true;
        }

        private static Func<MessageHandler, object, Task> GetDispatcher(Type handlerType, String methodName, ConcurrentDictionary<Type, Func<MessageHandler, object, Task>> dictionary)
        {
            var baseHandlerType = typeof(MessageHandler);

            return dictionary.GetOrAdd(handlerType, t =>
            {
                var returnLabel = Expression.Label(typeof (Task));

                var paramType = typeof(Object);
                var baseDispatcher = baseHandlerType.IsAssignableFrom(t.BaseType)
                                         ? GetDispatcher(t.BaseType, methodName, dictionary)
                                         : null;

                var handlerParam = Expression.Parameter(baseHandlerType);
                var inputParam = Expression.Parameter(paramType);
                var handlerVariable = Expression.Variable(t);

                var assignHandlerVariable = Expression.Assign(handlerVariable, Expression.Convert(handlerParam, t));

                var dispatchers = t
                    .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(x => x.DeclaringType == t)
                    .Select(m => new { Method = m, Parameters = m.GetParameters() })
                    .Where(x => x.Method.Name == methodName && x.Parameters.Length == 1)
                    .Where(x => x.Method.ReturnType == typeof(void) || x.Method.ReturnType == typeof(Task))
                    .Select(x => new { x.Method, x.Parameters[0].ParameterType, ReturnsTask = x.Method.ReturnType == typeof(Task) })
                    .Where(x => paramType.IsAssignableFrom(x.ParameterType))
                    .OrderByDescending(x => x.ParameterType, _typeHierarchyComparer)
                    .Select(x =>
                    {
                        var call = Expression.Call(
                            handlerVariable,
                            x.Method,
                            Expression.Convert(inputParam, x.ParameterType)
                        );

                        var applicatorBody = x.ReturnsTask
                            ? (Expression)Expression.Return(returnLabel, call, typeof(Task))
                            : Expression.Block(call, Expression.Return(returnLabel, Expression.Constant(_completedTask), typeof(Task)));

                        return Expression.IfThen(Expression.TypeIs(inputParam, x.ParameterType), applicatorBody);
                    })
                    .OfType<Expression>()
                    .ToArray();

                if (dispatchers.Length <= 0)
                    return baseDispatcher ?? (Func<MessageHandler, object, Task>)((h, m) => h.HandleUnexpectedMessage(m));

                if (baseDispatcher != null)
                    dispatchers = dispatchers.Concat(new[] { Expression.Return(returnLabel, Expression.Invoke(Expression.Constant(baseDispatcher), handlerParam, inputParam), typeof(Task)) }).ToArray();

                var body = new[] { assignHandlerVariable }.Concat(dispatchers);

                return Expression.Lambda<Func<MessageHandler, object, Task>>(
                    Expression.Label(returnLabel, Expression.Block(typeof(Task), new[] { handlerVariable }, body)),
                                     handlerParam, inputParam)
                    .Compile();
            });
        }

        protected virtual Task HandleUnexpectedMessage(Object message)
        {
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetException(new InvalidOperationException(String.Format("Unable to handle message of type '{0}'.", message.GetType().FullName)));
            return tcs.Task;
        }
    }
}
