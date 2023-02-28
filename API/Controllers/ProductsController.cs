using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
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

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            //var products = await _productRepo.ListAllAsync();
            var spec = new productsWithTypesAndBrandsSpec();
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
            return Ok(_mapper
            .Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(products));
        }
        [HttpGet("{id}")]
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