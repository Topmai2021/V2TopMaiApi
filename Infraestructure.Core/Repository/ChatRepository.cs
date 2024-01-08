using Infraestructure.Core.Data;
using Infraestructure.Core.Repository.Interface;
using Infraestructure.Entity.Entities.Chats;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Core.Repository
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        #region Attributes
        private readonly DataContext _context;
        #endregion

        #region Builder
        public ChatRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        #endregion

        public IEnumerable<Chat> GetAllChats(Guid profileSender, int page)
        {
            int take = 20;
            var chat = _context.Chats.Include(c => c.ChatConfigurations.Where(cf => cf.ChatDeleted == false && cf.ProfileId == profileSender))
                                     .Include(m => m.Messages.OrderByDescending(d => d.DateHour).Take(1))
                                     .Include(pR => pR.IdProfileReceiverNavigation).ThenInclude(i => i.Image)
                                     .Include(pS => pS.IdProfileSenderNavigation).ThenInclude(i => i.Image)
                                     .Where(x => (x.IdProfileSender == profileSender || x.IdProfileReceiver == profileSender))
                                     .OrderByDescending(x => x.Messages.Max(d => d.DateHour))
                                     .Skip((page - 1) * take).Take(take)
                                     .AsNoTracking().AsEnumerable();
            return chat;
        }
    }
}
