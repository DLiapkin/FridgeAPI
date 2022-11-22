using AutoMapper;
using Domain.Models;
using Domain.Contracts;
using Services.Models;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

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

        public async Task<FridgeResponse> Create(FridgeRequest fridgeToCreate)
        {
            Fridge fridge = _mapper.Map<Fridge>(fridgeToCreate);
            await _unitOfWork.Fridge.Create(fridge);
            await _unitOfWork.Save();
            FridgeResponse fridgeToReturn = _mapper.Map<FridgeResponse>(fridge);
            return fridgeToReturn;
        }

        public async Task<IEnumerable<FridgeResponse>> GetAll()
        {
            IEnumerable<Fridge> fridges = await _unitOfWork.Fridge.FindAll(trackChanges: true);
            IEnumerable<FridgeResponse> fridgesDto = _mapper.Map<IEnumerable<FridgeResponse>>(fridges);
            return fridgesDto;
        }

        public async Task<FridgeResponse> GetById(Guid id)
        {
            Fridge fridge = await _unitOfWork.Fridge.FindById(id, trackChanges: false);
            FridgeResponse fridgeDto = _mapper.Map<FridgeResponse>(fridge);
            return fridgeDto;
        }

        public async Task Update(Guid id, FridgeRequest fridgeToUpdate)
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

        public async Task<FridgeProductResponse> CreateProduct(Guid fridgeId, FridgeProductRequest productToCreateDto)
        {
            FridgeProduct product = _mapper.Map<FridgeProduct>(productToCreateDto);
            product.FridgeId = fridgeId;
            await _unitOfWork.FridgeProduct.Create(product);
            await _unitOfWork.Save();
            FridgeProductResponse productToReturn = _mapper.Map<FridgeProductResponse>(product);
            return productToReturn;
        }

        public async Task<IEnumerable<FridgeProductResponse>> GetProducts(Guid fridgeId)
        {
            Fridge fridge = await _unitOfWork.Fridge.FindById(fridgeId, trackChanges: true);
            IEnumerable<FridgeProduct> products = fridge.Products.ToList();
            IEnumerable<FridgeProductResponse> productsDto = _mapper.Map<IEnumerable<FridgeProductResponse>>(products);
            return productsDto;
        }

        public async Task<FridgeProductResponse> GetProductById(Guid productId)
        {
            FridgeProduct product = await _unitOfWork.FridgeProduct.FindById(productId, trackChanges: false);
            FridgeProductResponse productDto = _mapper.Map<FridgeProductResponse>(product);
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
