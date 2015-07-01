using SoundBoard.Data;
using SoundBoard.Models;
using SoundBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoundBoard.Services
{
    public class ViewMessageService
    {
        private readonly MessageRepository _messages;
        private readonly ApplicationDbContext _db;

        public ViewMessageService(MessageRepository messages, ApplicationDbContext dbContext)
        {
            _messages = messages;
            _db = dbContext;
        }

        public async Task<IEnumerable<MessageViewModel>> GetAllMessagesAsync()
        {
            var messages = _messages.GetAll();
            var users = await _db.Users.ToDictionaryAsync(u => u.Id,
                u => u.Email);

            return messages.Select(m => new MessageViewModel
            {
                Id = m.Id,
                UserId = m.UserId,
                MessageContent = m.MessageContent,
                MessageTitle = m.MessageTitle,
                Created = m.Created,
                Username = users[m.UserId]
            }).ToArray();
        }
    }
}
