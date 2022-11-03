using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Services;
using Entities.Models;
using Entities.DataTransferObjects;
using AutoMapper;
using Contracts.Repositries;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto> Create(ProductToCreateDto productToCreate)
        {
            Product product = _mapper.Map<Product>(productToCreate);
            await _unitOfWork.Product.Create(product);
            await _unitOfWork.Save();
            ProductDto productToReturn = _mapper.Map<ProductDto>(product);
            return productToReturn;
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            IEnumerable<Product> products = await _unitOfWork.Product.FindAll(trackChanges: true);
            IEnumerable<ProductDto> productDto = _mapper.Map<IEnumerable<ProductDto>>(products);
            return productDto;
        }

        public async Task<ProductDto> GetById(Guid id)
        {
            Product product = await _unitOfWork.Product.FindById(id, trackChanges: false);
            ProductDto productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }

        public async Task Update(Guid id, ProductToUpdateDto productToUpdate)
        {
            Product productEntity = await _unitOfWork.Product.FindById(id, trackChanges: false);
            _mapper.Map(productToUpdate, productEntity);
            _unitOfWork.Product.Update(productEntity);
            await _unitOfWork.Save();
        }

        public async Task Delete(Guid id)
        {
            Product product = await _unitOfWork.Product.FindById(id, trackChanges: false);
            _unitOfWork.Product.Delete(product);
            await _unitOfWork.Save();
        }
    }
}
