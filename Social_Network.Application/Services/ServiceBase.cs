using AutoMapper;
using Social_Network.Core.Application.DTOs;
using Social_Network.Core.Application.Interfaces.Repositories;
using Social_Network.Core.Application.Interfaces.Services;
using Social_Network.Core.Domain.Base;

namespace Social_Network.Core.Application.Services
{
    public abstract class ServiceBase<TEntity, TID, TDTO, TInsertDTO, TUpdateDTO> : IServiceBase<TEntity, TID, TDTO, TInsertDTO, TUpdateDTO> 
        where TEntity : BaseEntity<TID>
        where TUpdateDTO : BaseResourceDTO<TID>
    {
        private readonly IRepositoryBase<TEntity, TID> _repositoryBase;
        protected readonly IMapper _mapper;

        private Func<TInsertDTO, Task<Result<TInsertDTO>>>? _insertPreCondition;
        private Func<TUpdateDTO, TEntity, Result<TUpdateDTO>>? _updatePreCondition;
        private Func<TID, string, TEntity, Result<Unit>>? _deletePreCondition;
        private Func<TDTO, Task<TDTO>>? _getByIdPostMappingProccessing;
        private Func<IEnumerable<TDTO>, Task<IEnumerable<TDTO>>>? _getAllPostMappingProccessing;

        protected ServiceBase(IRepositoryBase<TEntity, TID> repositoryBase, IMapper mapper)
        {
            _repositoryBase = repositoryBase;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> CreateAsync(TInsertDTO dTO)
        {
            if(_insertPreCondition != null)
            {
                var isValid = await _insertPreCondition(dTO);
                if(!isValid.IsSuccess)
                    return Result<Unit>.Fail(isValid.Error);
            }
            var entity = _mapper.Map<TEntity>(dTO);
            var result = await _repositoryBase.AddAsync(entity);
            return result.IsSuccess? Unit.Value : Result<Unit>.Fail(result.Error);
        }

        public async Task<Result<Unit>> DeleteAsync(TID ResourceID, string UserId)
        {
            var entityRes = await _repositoryBase.GetById(ResourceID);
            if (entityRes == null)
                return Result<Unit>.Fail("Resource to Delete not found");
            if (_deletePreCondition != null)
            {
                var isValid = _deletePreCondition(ResourceID, UserId, entityRes.Value);
                if (!isValid.IsSuccess)
                    return isValid;
            }
            var res = await _repositoryBase.DeleteAsync(entityRes.Value);
            return res.IsSuccess ? Unit.Value : res;
        }

        public async Task<IEnumerable<TDTO>> GetAll()
        {
            var ents = await _repositoryBase.GetAllAsync();
            IEnumerable<TDTO> dtos = ents.Select(e => _mapper.Map<TDTO>(e)).ToList();
            if(_getAllPostMappingProccessing != null)
                dtos = await _getAllPostMappingProccessing(dtos);
            return dtos;
        }

        public async Task<Result<TDTO>> GetById(TID id)
        {
            var ent = await _repositoryBase.GetById(id);
            if (ent.IsFailure)
                return Result<TDTO>.Fail("Resource not found");
            TDTO dTO = _mapper.Map<TDTO>(ent.Value);
            if(_getByIdPostMappingProccessing != null)
                dTO = await _getByIdPostMappingProccessing(dTO);
            return dTO;
        }

        public async Task<Result<Unit>> UpdateAsync(TUpdateDTO dTO)
        {

            var entityRes = await _repositoryBase.GetById(dTO.Id);
            if (entityRes == null)
                return Result<Unit>.Fail("Resource to Update not found");
            var entity = entityRes.Value;
            if (_updatePreCondition != null)
            {
                var isValid = _updatePreCondition(dTO, entity);
                if (!isValid.IsSuccess)
                    return Result<Unit>.Fail(isValid.Error);
            }
            _mapper.Map(dTO, entity);
            var res = await _repositoryBase.UpdateAsync(entity);
            if (!res.IsSuccess)
                return Result<Unit>.Fail(res.Error);
            return Unit.Value;
        }

        protected void ConfigureInsertPreCondition(Func<TInsertDTO, Task<Result<TInsertDTO>>>? insertPreCondition)
            => _insertPreCondition = insertPreCondition;
        protected void ConfigureUpdatePreCondition(Func<TUpdateDTO, TEntity, Result<TUpdateDTO>>? updatePreCondition)
            => _updatePreCondition = updatePreCondition;

        protected void ConfigureDeletePreCondition(Func<TID, string, TEntity, Result<Unit>>? deletePreCondition)
            => _deletePreCondition = deletePreCondition;

        protected void ConfigureGetByIdPostMapping(Func<TDTO, Task<TDTO>>? getByIdPostMapping)
            => _getByIdPostMappingProccessing = getByIdPostMapping;

        protected void ConfigureGetAllPostMapping(Func<IEnumerable<TDTO>, Task<IEnumerable<TDTO>>>? getAllPostMapping)
            => _getAllPostMappingProccessing = getAllPostMapping;


    }
}
