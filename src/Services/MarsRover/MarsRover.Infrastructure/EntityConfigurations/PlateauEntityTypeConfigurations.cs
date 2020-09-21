using System;
using System.Collections.Generic;
using System.Text;
using MarsRover.Domain.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarsRover.Infrastructure.EntityConfigurations
{
    public class PlateauEntityTypeConfigurations : BaseEntityTypeConfiguration<Plateau>
    {
        public override void Configure(EntityTypeBuilder<Plateau> builder)
        {
            builder.ToTable("plateaus", MarsRoverContext.DEFAULT_SCHEMA);
            base.ConfigureForEntity(builder);

            builder.Ignore(p => p.Coordinate);
            builder
                //.Property("_xCoordinate")
                .Property(p=>p.XCoordinate)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("XCoordinate")
                .IsRequired();

            builder
                //.Property("_yCoordinate")
                .Property(p => p.YCoordinate)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("YCoordinate")
                .IsRequired();
        }
    }
}
