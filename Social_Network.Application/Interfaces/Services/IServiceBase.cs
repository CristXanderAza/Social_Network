using Social_Network.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Interfaces.Services
{
    public interface IServiceBase<TEntity, TID, TDTO, TInsertDTO, TUpdateDTO>
    {
        Task<IEnumerable<TDTO>> GetAll();
        Task<Result<TDTO>> GetById(TID id);
        Task<Result<Unit>> CreateAsync(TInsertDTO entity);
        Task<Result<Unit>> UpdateAsync(TUpdateDTO entity);
        Task<Result<Unit>> DeleteAsync(TID ResourceID, string UserId);
    }
}
