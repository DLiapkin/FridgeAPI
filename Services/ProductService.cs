using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Contracts.Services;
using Entities.Models;
using Entities.DataTransferObjects;
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

        public ProductDto Create(ProductToCreateDto productToCreate)
        {
            Product product = _mapper.Map<Product>(productToCreate);
            _unitOfWork.Product.Create(product);
            _unitOfWork.Save();
            ProductDto productToReturn = _mapper.Map<ProductDto>(product);
            return productToReturn;
        }

        public IEnumerable<ProductDto> GetAll()
        {
            IEnumerable<Product> products = _unitOfWork.Product.FindAll(trackChanges: true);
            IEnumerable<ProductDto> productDto = _mapper.Map<IEnumerable<ProductDto>>(products);
            return productDto;
        }

        public ProductDto GetById(Guid id)
        {
            Product product = _unitOfWork.Product.FindById(id, trackChanges: false);
            ProductDto productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }

        public void Update(Guid id, ProductToUpdateDto productToUpdate)
        {
            Product productEntity = _unitOfWork.Product.FindById(id, trackChanges: false);
            _mapper.Map(productToUpdate, productEntity);
            _unitOfWork.Product.Update(productEntity);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            Product product = _unitOfWork.Product.FindById(id, trackChanges: false);
            _unitOfWork.Product.Delete(product);
            _unitOfWork.Save();
        }
    }
}
