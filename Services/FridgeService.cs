using AutoMapper;
using Contracts;
using Contracts.Services;
using Entities.Models;
using Entities.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Services
{
    public class FridgeService : IFridgeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly string _storedProc;

        public FridgeService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storedProc = configuration.GetSection("StoredProcedures:refreshProducts").Value;
        }

        public FridgeDto Create(FridgeToCreateDto fridgeToCreate)
        {
            Fridge fridge = _mapper.Map<Fridge>(fridgeToCreate);
            _unitOfWork.Fridge.Create(fridge);
            _unitOfWork.Save();
            FridgeDto fridgeToReturn = _mapper.Map<FridgeDto>(fridge);
            return fridgeToReturn;
        }

        public IEnumerable<FridgeDto> GetAll()
        {
            IEnumerable<Fridge> fridges = _unitOfWork.Fridge.FindAll(trackChanges: true);
            IEnumerable<FridgeDto> fridgesDto = _mapper.Map<IEnumerable<FridgeDto>>(fridges);
            return fridgesDto;
        }

        public FridgeDto GetById(Guid id)
        {
            Fridge fridge = _unitOfWork.Fridge.FindById(id, trackChanges: false);
            FridgeDto fridgeDto = _mapper.Map<FridgeDto>(fridge);
            return fridgeDto;
        }

        public void Update(Guid id, FridgeToUpdateDto fridgeToUpdate)
        {
            Fridge fridgeEntity = _unitOfWork.Fridge.FindById(id, trackChanges: false);
            _mapper.Map(fridgeToUpdate, fridgeEntity);
            _unitOfWork.Fridge.Update(fridgeEntity);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            Fridge fridge = _unitOfWork.Fridge.FindById(id, trackChanges: false);
            _unitOfWork.Fridge.Delete(fridge);
            _unitOfWork.Save();
        }

        public FridgeProductDto CreateProduct(Guid fridgeId, FridgeProductToCreateDto productToCreateDto)
        {
            FridgeProduct product = _mapper.Map<FridgeProduct>(productToCreateDto);
            product.FridgeId = fridgeId;
            _unitOfWork.FridgeProduct.Create(product);
            _unitOfWork.Save();
            FridgeProductDto productToReturn = _mapper.Map<FridgeProductDto>(product);
            return productToReturn;
        }

        public IEnumerable<FridgeProductDto> GetProducts(Guid fridgeId)
        {
            Fridge fridge = _unitOfWork.Fridge.FindById(fridgeId, trackChanges: true);
            IEnumerable<FridgeProduct> products = fridge.Products.ToList();
            IEnumerable<FridgeProductDto> productsDto = _mapper.Map<IEnumerable<FridgeProductDto>>(products);
            return productsDto;
        }

        public FridgeProductDto GetProductById(Guid productId)
        {
            FridgeProduct product = _unitOfWork.FridgeProduct.FindById(productId, trackChanges: false);
            FridgeProductDto productDto = _mapper.Map<FridgeProductDto>(product);
            return productDto;
        }

        public void DeleteProduct(Guid fridgeProductId)
        {
            FridgeProduct product = _unitOfWork.FridgeProduct.FindById(fridgeProductId, trackChanges: false);
            _unitOfWork.FridgeProduct.Delete(product);
            _unitOfWork.Save();
        }

        public void RefreshProduct()
        {
            _unitOfWork.FridgeProduct.ExcecuteProcedure(_storedProc);
        }
    }
}
