using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregation;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext Dbcontext)
        {
            //Brands Seeding
            if (!Dbcontext.ProductBrands.Any())
            {
                var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                if (Brands?.Count > 0)
                {
                    foreach (var Brand in Brands)
                    {
                        await Dbcontext.Set<ProductBrand>().AddAsync(Brand);
                    }
                    await Dbcontext.SaveChangesAsync();
                }
            }
            // Types Seeding
            if (!Dbcontext.ProductTypes.Any())
            {
                var TypesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                if (Types?.Count > 0)
                {
                    foreach (var Type in Types)
                    {
                        await Dbcontext.Set<ProductType>().AddAsync(Type);
                    }
                    await Dbcontext.SaveChangesAsync();
                }
            }
            // Product Seeding
            if (!Dbcontext.Products.Any())
            {
                var ProductData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);
                if (Products?.Count > 0)
                {
                    foreach (var Product in Products)
                    {
                        await Dbcontext.Set<Product>().AddAsync(Product);
                    }
                    await Dbcontext.SaveChangesAsync();
                }
            }
			if (!Dbcontext.DeliveryMethods.Any())
			{
				var DeliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
				var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
				if (DeliveryMethods?.Count > 0)
				{
					foreach (var Method in DeliveryMethods)
					{
						await Dbcontext.Set<DeliveryMethod>().AddAsync(Method);
					}
					await Dbcontext.SaveChangesAsync();
				}
			}
		}
    }
}
