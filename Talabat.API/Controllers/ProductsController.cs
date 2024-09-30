using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTOs;
using Talabat.API.Errors;
using Talabat.API.Helpers;
using Talabat.Core;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.API.Controllers
{
	public class ProductsController : ApiBaseController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ProductsController(IUnitOfWork unitOfWork 
			                      ,IMapper mapper)
        {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		[Authorize]
		[HttpGet]
		public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams Params)
		{
			var Spec = new ProductWithBrandAndTypeSpecifications(Params);
			var Products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(Spec);
			var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);
			var CountSpec = new ProductWithFilterationForCountAsync(Params);
			var Count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);
			var ReturnResponse = new Pagination<ProductToReturnDto>()
			{
				PageIndex = Params.PageIndex,
				PageSize = Params.PageSize,
				Data = MappedProducts,
				Count = Count
			};
			return Ok(ReturnResponse);
		}
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ProductToReturnDto) , 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var Spec = new ProductWithBrandAndTypeSpecifications(id);
			var Product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(Spec);
			if (Product is null)
			{
				return NotFound(new ApiResponse(404));
			}
			var MappedProduct = _mapper.Map<Product , ProductToReturnDto>(Product);
			return Ok(MappedProduct);
		}
		// GET All Types
		[HttpGet("Types")]
		public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
		{
			var Types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
			return Ok(Types);
		}
		[HttpGet("Brands")]
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
		{
			var Brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
			return Ok(Brands);
		}

    }
}
