using Infraestructure.Core.Data;
using Infraestructure.Core.Repository.Interface;
using Infraestructure.Entity.Entities.Chats;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Core.Repository
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        #region Attributes
        private readonly DataContext _context;
        #endregion

        #region Builder
        public MessageRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        #endregion

        public IEnumerable<Message> GetAllByChat(Guid chatId, Guid userId, int page)
        {
            int take = 20;
            IEnumerable<Message> messages = _context.Messages.Include(f => f.From).ThenInclude(i => i.Image)
                                                              .OrderByDescending(d => d.DateHour)
                                                              .Where(x => x.ChatId == chatId && (x.Chat.IdProfileReceiver == userId || x.Chat.IdProfileSender == userId))
                                                              .Skip((page - 1) * take).Take(take)
                                                              .AsNoTracking().AsEnumerable();
            return messages;
        }
    }
}
