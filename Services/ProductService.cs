using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Models;
using Services.Contracts;
using Domain.Models;
using Domain.Contracts;
using AutoMapper;

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

        public async Task<ProductResponse> Create(ProductRequest productToCreate)
        {
            Product product = _mapper.Map<Product>(productToCreate);
            await _unitOfWork.Product.Create(product);
            await _unitOfWork.Save();
            ProductResponse productToReturn = _mapper.Map<ProductResponse>(product);
            return productToReturn;
        }

        public async Task<IEnumerable<ProductResponse>> GetAll()
        {
            IEnumerable<Product> products = await _unitOfWork.Product.FindAll(trackChanges: true);
            IEnumerable<ProductResponse> productDto = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return productDto;
        }

        public async Task<ProductResponse> GetById(Guid id)
        {
            Product product = await _unitOfWork.Product.FindById(id, trackChanges: false);
            ProductResponse productDto = _mapper.Map<ProductResponse>(product);
            return productDto;
        }

        public async Task Update(Guid id, ProductRequest productToUpdate)
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
