using AutoMapper;
using Domain.Models;
using Domain.Contracts;
using Services.Models;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class FridgeModelService : IFridgeModelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FridgeModelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FridgeModelResponse> Create(FridgeModelRequest fridgeModelToCreate)
        {
            FridgeModel fridgeModel = _mapper.Map<FridgeModel>(fridgeModelToCreate);
            await _unitOfWork.FridgeModel.Create(fridgeModel);
            await _unitOfWork.Save();
            FridgeModelResponse fridgeModelToReturn = _mapper.Map<FridgeModelResponse>(fridgeModel);
            return fridgeModelToReturn;
        }

        public async Task<IEnumerable<FridgeModelResponse>> GetAll()
        {
            IEnumerable<FridgeModel> fridgeModels = await _unitOfWork.FridgeModel.FindAll(trackChanges: true);
            IEnumerable<FridgeModelResponse> fridgeModelsDto = _mapper.Map<IEnumerable<FridgeModelResponse>>(fridgeModels);
            return fridgeModelsDto;
        }

        public async Task<FridgeModelResponse> GetById(Guid id)
        {
            FridgeModel fridgeModel = await _unitOfWork.FridgeModel.FindById(id, trackChanges: false);
            FridgeModelResponse fridgeModelDto = _mapper.Map<FridgeModelResponse>(fridgeModel);
            return fridgeModelDto;
        }

        public async Task Update(Guid id, FridgeModelRequest fridgeModelToUpdate)
        {
            FridgeModel fridgeModelEntity = await _unitOfWork.FridgeModel.FindById(id, trackChanges: false);
            _mapper.Map(fridgeModelToUpdate, fridgeModelEntity);
            _unitOfWork.FridgeModel.Update(fridgeModelEntity);
            await _unitOfWork.Save();
        }

        public async Task Delete(Guid id)
        {
            FridgeModel fridgeModel = await _unitOfWork.FridgeModel.FindById(id, trackChanges: false);
            _unitOfWork.FridgeModel.Delete(fridgeModel);
            await _unitOfWork.Save();
        }
    }
}
