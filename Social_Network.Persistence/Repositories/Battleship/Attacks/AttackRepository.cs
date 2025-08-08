using Social_Network.Core.Application.Interfaces.Repositories.Battleship.Attacks;
using Social_Network.Core.Domain.Entities.Battleship.Attacks;
using Social_Network.Infraestructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Infraestructure.Persistence.Repositories.Battleship.Attacks
{
    public class AttackRepository : BaseRepository<Attack, Guid>, IAttackRepository
    {
        public AttackRepository(SocialNetworkContext context) : base(context)
        {
        }
    }
}
