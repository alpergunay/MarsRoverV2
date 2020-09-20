using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using MarsRover.Domain.DomainModels;
using MarsRover.Infrastructure.Idempotency;
using MarsRover.Infrastructure.Repositories;

namespace MarsRover.Api.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        public string QueriesConnectionString { get; }

        public ApplicationModule(string qconstr)
        {
            QueriesConnectionString = qconstr;
        }

        protected override void Load(ContainerBuilder builder)
        {
            #region Repositories
            builder.RegisterType<RequestManager>()
                .As<IRequestManager>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PlateauRepository>()
                .As<IPlateauRepository>()
                .InstancePerLifetimeScope();

            #endregion

            #region Validations



            #endregion

        }
    }
}
