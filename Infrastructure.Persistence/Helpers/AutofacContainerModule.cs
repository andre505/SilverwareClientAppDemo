using Application.Services.Interfaces;
using Autofac;
using Infrastructure.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Helpers
{
    public class AutofacContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterAssemblyTypes(typeof(IAutoDependencyService).Assembly)
            .AssignableTo<IAutoDependencyService>()
            .As<IAutoDependencyService>()
            .AsImplementedInterfaces().InstancePerLifetimeScope();

        }
    }
}
