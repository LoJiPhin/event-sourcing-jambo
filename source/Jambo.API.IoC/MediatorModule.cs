﻿using Autofac;
using Autofac.Core;
using Jambo.Application.Commands;
using MediatR;
using System.Linq;
using System.Reflection;

namespace Jambo.API.IoC
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            // Register all the Command classes (they implement IAsyncRequestHandler) in assembly holding the Commands
            builder.RegisterAssemblyTypes(typeof(CriarBlogCommand).GetTypeInfo().Assembly)
                .As(o => o.GetInterfaces()
                    .Where(i => i.IsClosedTypeOf(typeof(IAsyncRequestHandler<,>)))
                    .Select(i => new KeyedService("IAsyncRequestHandler", i)));

            builder.RegisterAssemblyTypes(typeof(CriarBlogCommand).GetTypeInfo().Assembly)
                .As(o => o.GetInterfaces()
                    .Where(i => i.IsClosedTypeOf(typeof(IRequestHandler<>)))
                    .Select(i => new KeyedService("IAsyncRequestHandler", i)));
        }
    }
}
