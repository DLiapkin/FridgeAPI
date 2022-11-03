using AutoMapper;
using Contracts.Services;
using Entities.Models;
using Entities.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Repositries;

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

        public async Task<FridgeModelDto> Create(FridgeModelToCreateDto fridgeModelToCreate)
        {
            FridgeModel fridgeModel = _mapper.Map<FridgeModel>(fridgeModelToCreate);
            await _unitOfWork.FridgeModel.Create(fridgeModel);
            await _unitOfWork.Save();
            FridgeModelDto fridgeModelToReturn = _mapper.Map<FridgeModelDto>(fridgeModel);
            return fridgeModelToReturn;
        }

        public async Task<IEnumerable<FridgeModelDto>> GetAll()
        {
            IEnumerable<FridgeModel> fridgeModels = await _unitOfWork.FridgeModel.FindAll(trackChanges: true);
            IEnumerable<FridgeModelDto> fridgeModelsDto = _mapper.Map<IEnumerable<FridgeModelDto>>(fridgeModels);
            return fridgeModelsDto;
        }

        public async Task<FridgeModelDto> GetById(Guid id)
        {
            FridgeModel fridgeModel = await _unitOfWork.FridgeModel.FindById(id, trackChanges: false);
            FridgeModelDto fridgeModelDto = _mapper.Map<FridgeModelDto>(fridgeModel);
            return fridgeModelDto;
        }

        public async Task Update(Guid id, FridgeModelToUpdateDto fridgeModelToUpdate)
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
