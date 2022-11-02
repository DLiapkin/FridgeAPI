using AutoMapper;
using Contracts;
using Contracts.Services;
using Entities.Models;
using Entities.DataTransferObjects;
using System;
using System.Collections.Generic;

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

        public FridgeModelDto Create(FridgeModelToCreateDto fridgeModelToCreate)
        {
            FridgeModel fridgeModel = _mapper.Map<FridgeModel>(fridgeModelToCreate);
            _unitOfWork.FridgeModel.Create(fridgeModel);
            _unitOfWork.Save();
            FridgeModelDto fridgeModelToReturn = _mapper.Map<FridgeModelDto>(fridgeModel);
            return fridgeModelToReturn;
        }

        public IEnumerable<FridgeModelDto> GetAll()
        {
            IEnumerable<FridgeModel> fridgeModels = _unitOfWork.FridgeModel.FindAll(trackChanges: true);
            IEnumerable<FridgeModelDto> fridgeModelsDto = _mapper.Map<IEnumerable<FridgeModelDto>>(fridgeModels);
            return fridgeModelsDto;
        }

        public FridgeModelDto GetById(Guid id)
        {
            FridgeModel fridgeModel = _unitOfWork.FridgeModel.FindById(id, trackChanges: false);
            FridgeModelDto fridgeModelDto = _mapper.Map<FridgeModelDto>(fridgeModel);
            return fridgeModelDto;
        }

        public void Update(Guid id, FridgeModelToUpdateDto fridgeModelToUpdate)
        {
            FridgeModel fridgeModelEntity = _unitOfWork.FridgeModel.FindById(id, trackChanges: false);
            _mapper.Map(fridgeModelToUpdate, fridgeModelEntity);
            _unitOfWork.FridgeModel.Update(fridgeModelEntity);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            FridgeModel fridgeModel = _unitOfWork.FridgeModel.FindById(id, trackChanges: false);
            _unitOfWork.FridgeModel.Delete(fridgeModel);
            _unitOfWork.Save();
        }
    }
}
