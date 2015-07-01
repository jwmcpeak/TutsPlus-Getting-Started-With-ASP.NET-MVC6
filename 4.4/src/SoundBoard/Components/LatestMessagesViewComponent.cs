using Microsoft.AspNet.Mvc;
using SoundBoard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoundBoard.Components
{
    public class LatestMessagesViewComponent : ViewComponent
    {
        private readonly MessageRepository _messages;

        public LatestMessagesViewComponent(MessageRepository messages)
        {
            _messages = messages;
        }

        public IViewComponentResult Invoke(int count)
        {
            var messages = _messages.GetAll()
                .OrderByDescending(m => m.Created)
                .Take(count);

            return View(messages);
        }
    }
}
