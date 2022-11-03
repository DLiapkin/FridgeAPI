using AutoMapper;
using Contracts.Services;
using Entities.Models;
using Entities.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Contracts.Repositries;

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

        public async Task<FridgeDto> Create(FridgeToCreateDto fridgeToCreate)
        {
            Fridge fridge = _mapper.Map<Fridge>(fridgeToCreate);
            await _unitOfWork.Fridge.Create(fridge);
            await _unitOfWork.Save();
            FridgeDto fridgeToReturn = _mapper.Map<FridgeDto>(fridge);
            return fridgeToReturn;
        }

        public async Task<IEnumerable<FridgeDto>> GetAll()
        {
            IEnumerable<Fridge> fridges = await _unitOfWork.Fridge.FindAll(trackChanges: true);
            IEnumerable<FridgeDto> fridgesDto = _mapper.Map<IEnumerable<FridgeDto>>(fridges);
            return fridgesDto;
        }

        public async Task<FridgeDto> GetById(Guid id)
        {
            Fridge fridge = await _unitOfWork.Fridge.FindById(id, trackChanges: false);
            FridgeDto fridgeDto = _mapper.Map<FridgeDto>(fridge);
            return fridgeDto;
        }

        public async Task Update(Guid id, FridgeToUpdateDto fridgeToUpdate)
        {
            Fridge fridgeEntity = await _unitOfWork.Fridge.FindById(id, trackChanges: false);
            _mapper.Map(fridgeToUpdate, fridgeEntity);
            _unitOfWork.Fridge.Update(fridgeEntity);
            await _unitOfWork.Save();
        }

        public async Task Delete(Guid id)
        {
            Fridge fridge = await _unitOfWork.Fridge.FindById(id, trackChanges: false);
            _unitOfWork.Fridge.Delete(fridge);
            await _unitOfWork.Save();
        }

        public async Task<FridgeProductDto> CreateProduct(Guid fridgeId, FridgeProductToCreateDto productToCreateDto)
        {
            FridgeProduct product = _mapper.Map<FridgeProduct>(productToCreateDto);
            product.FridgeId = fridgeId;
            await _unitOfWork.FridgeProduct.Create(product);
            await _unitOfWork.Save();
            FridgeProductDto productToReturn = _mapper.Map<FridgeProductDto>(product);
            return productToReturn;
        }

        public async Task<IEnumerable<FridgeProductDto>> GetProducts(Guid fridgeId)
        {
            Fridge fridge = await _unitOfWork.Fridge.FindById(fridgeId, trackChanges: true);
            IEnumerable<FridgeProduct> products = fridge.Products.ToList();
            IEnumerable<FridgeProductDto> productsDto = _mapper.Map<IEnumerable<FridgeProductDto>>(products);
            return productsDto;
        }

        public async Task<FridgeProductDto> GetProductById(Guid productId)
        {
            FridgeProduct product = await _unitOfWork.FridgeProduct.FindById(productId, trackChanges: false);
            FridgeProductDto productDto = _mapper.Map<FridgeProductDto>(product);
            return productDto;
        }

        public async Task DeleteProduct(Guid fridgeProductId)
        {
            FridgeProduct product = await _unitOfWork.FridgeProduct.FindById(fridgeProductId, trackChanges: false);
            _unitOfWork.FridgeProduct.Delete(product);
            await _unitOfWork.Save();
        }

        public async Task RefreshProduct()
        {
            await _unitOfWork.FridgeProduct.ExcecuteProcedure(_storedProc);
        }
    }
}
