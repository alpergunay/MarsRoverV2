using System;
using System.Collections.Generic;
using System.Text;
using MarsRover.Domain.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarsRover.Infrastructure.EntityConfigurations
{
    public class CommandEntityTypeConfiguration : BaseEntityTypeConfiguration<Command>
    {
        public override void Configure(EntityTypeBuilder<Command> builder)
        {
            builder.ToTable("commands", MarsRoverContext.DEFAULT_SCHEMA);
            base.ConfigureForEnum(builder);

        }
    }
}
