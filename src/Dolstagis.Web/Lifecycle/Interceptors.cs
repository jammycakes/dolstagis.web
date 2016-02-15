﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Lifecycle
{
    public class Interceptors
    {
        private IList<IInterceptor> _interceptors;

        public Interceptors()
        {
            _interceptors = new List<IInterceptor>();
        }

        public Interceptors(IEnumerable<IInterceptor> interceptors)
        {
            _interceptors = interceptors.ToList();
        }

        public async Task<IRequestContext> BeginRequest(IRequestContext context)
        {
            foreach (var interceptor in _interceptors) {
                context = await interceptor.BeginRequest(context);
            }
            return context;
        }

        public async Task<object> ControllerFound(IRequestContext context, object controller)
        {
            foreach (var interceptor in _interceptors) {
                controller = await interceptor.ControllerFound(context, controller);
            }
            return controller;
        }

        public async Task<object[]> ModelBound(IRequestContext context, object[] model, MethodInfo method)
        {
            foreach (var interceptor in _interceptors) {
                model = await interceptor.ModelBound(context, model, method);
            }
            return model;
        }


        public async Task<Exception> HandleException(IRequestContext context, Exception ex)
        {
            foreach (var interceptor in _interceptors) {
                ex = await interceptor.HandleException(context, ex);
            }
            return ex;
        }
    }
}
