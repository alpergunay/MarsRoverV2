using System;
using System.Collections.Generic;
using System.Text;
using MarsRover.Domain.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarsRover.Infrastructure.EntityConfigurations
{
    public class DirectionEntityTypeConfiguration : BaseEntityTypeConfiguration<Direction>
    {
        public override void Configure(EntityTypeBuilder<Direction> builder)
        {
            builder.ToTable("directions", MarsRoverContext.DEFAULT_SCHEMA);
            base.ConfigureForEnum(builder);

        }
    }
}
