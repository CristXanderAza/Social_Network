using Social_Network.Core.Domain.Entities.Battleship.Attacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Interfaces.Repositories.Battleship.Attacks
{
    public interface IAttackRepository : IRepositoryBase<Attack, Guid>
    {
    }
}
