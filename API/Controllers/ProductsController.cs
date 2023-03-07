using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        //private readonly IProductRepository _repo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;
        public ProductsController(IGenericRepository<Product> productRepo,
        IGenericRepository<ProductBrand> productBrandRepo 
        , IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productRepo = productRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
           
        }

        // On a ajouté string sort dans la section sorting pagination
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams productParams)
        {
            //var products = await _productRepo.ListAllAsync();
            var spec = new productsWithTypesAndBrandsSpec(productParams);
            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _productRepo.CountAsync(spec);

            var products = await _productRepo.ListAsync(spec);
            //return Ok(products);
            // mapping with our flat result
            // kona najmou naamlouha fel spec toul
            // .tolist houni mamasetch el base , andna el products fel memoire juste mappinaha
            /*return products.Select(product => new ProductToReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                description = product.description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name
            }).ToList();*/
            
            //return Ok(_mapper
            //.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(products));

            var data = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }
        // zedna ProducesResponce type khater fel swagger ui maanech el cas li yebda 404 error
        // w baad zeda 7assana khater fel swagger jabelna el 404 ema fiha des champs mouch mriglin khater yaarach el type
        // donc nzidou el type li amelneh
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new productsWithTypesAndBrandsSpec(id);
            //return await _productRepo.GetByIdAsync(id);
            var product =  await _productRepo.GetEntityWithSpec(spec);
            //return new ProductToReturnDto
            /*{
                Id = product.Id,
                Name = product.Name,
                description = product.description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name          
            };*/
            if(product == null) return NotFound(new ApiResponse(404));
            return _mapper.Map<Product, ProductToReturnDto>(product);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            // zidna Ok khater jetna erreur 9alik conversion lel IReadOnlylist mayaceptihech w 9bel masta3melnehech heka alech jetnech erreur
            //return Ok(await _repo.GetProductBrandAsync());
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            // zidna Ok khater jetna erreur 9alik conversion lel IReadOnlylist mayaceptihech w 9bel masta3melnehech heka alech jetnech erreur
            //return Ok(await _repo.GetProductTypeAsync());
            return Ok(await _productTypeRepo.ListAllAsync());
        }
    }
}