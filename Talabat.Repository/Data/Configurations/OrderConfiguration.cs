using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregation;

namespace Talabat.Repository.Data.Configurations
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.Property(O => O.Status)
				   .HasConversion(OStatus => OStatus.ToString(), OStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus));

			builder.Property(O => O.Subtotal)
				   .HasColumnType("decimal(18,2)");

			builder.OwnsOne(O => O.ShipingAdress, X => X.WithOwner());

			builder.HasOne(O => O.DeliveryMethod)
				   .WithMany()
				   .OnDelete(DeleteBehavior.NoAction);
		}
	}
}
