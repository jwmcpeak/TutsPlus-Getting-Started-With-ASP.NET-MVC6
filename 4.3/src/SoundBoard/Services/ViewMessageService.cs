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

        internal async Task<IEnumerable<MessageViewModel>> GetAllMessagesAsync(string username)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == username);

            if (user == null)
            {
                return null;
            }

            var messages = _messages.GetByUserId(user.Id);

            return messages.Select(m => new MessageViewModel
            {
                Id = m.Id,
                UserId = m.UserId,
                MessageContent = m.MessageContent,
                MessageTitle = m.MessageTitle,
                Created = m.Created,
                Username = user.Email
            }).ToArray();
        }

        internal async Task<MessageViewModel> GetMessageAsync(int id)
        {
            var m = _messages.GetBy(id);

            if (m == null)
            {
                return null;
            }

            var user = await _db.Users.SingleAsync(u => u.Id == m.UserId);

            return new MessageViewModel
            {
                Id = m.Id,
                UserId = m.UserId,
                MessageContent = m.MessageContent,
                MessageTitle = m.MessageTitle,
                Created = m.Created,
                Username = user.Email
            };

        }
    }
}
