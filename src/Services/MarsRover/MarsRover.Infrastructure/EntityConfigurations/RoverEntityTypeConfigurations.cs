using System;
using System.Collections.Generic;
using System.Text;
using MarsRover.Domain.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarsRover.Infrastructure.EntityConfigurations
{
    public class RoverEntityTypeConfigurations : BaseEntityTypeConfiguration<Rover>
    {
        public override void Configure(EntityTypeBuilder<Rover> builder)
        {
            builder.ToTable("rovers", MarsRoverContext.DEFAULT_SCHEMA);
            base.ConfigureForEntity(builder);

            builder.Ignore(x => x.CurrentCoordinate);
            builder.Ignore(x => x.CurrentDirection);

            builder
                .Property("_plateauId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("PlateauId")
                .IsRequired();
            builder
                .Property("_xCoordinate")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("XCoordinate")
                .IsRequired();
            builder
                .Property("_yCoordinate")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("YCoordinate")
                .IsRequired();
            builder
                .Property("_directionId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("DirectionId")
                .IsRequired();

            builder.HasOne(p => p.Plateau)
                .WithMany()
                .HasForeignKey("_plateauId");

            builder.HasOne(p => p.CurrentDirection)
                .WithMany()
                .HasForeignKey("_directionId");

        }
    }
}
