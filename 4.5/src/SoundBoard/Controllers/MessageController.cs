using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SoundBoard.Models;
using SoundBoard.Data;
using System.Security.Claims;
using Microsoft.AspNet.Authorization;
using SoundBoard.Services;
using SoundBoard.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SoundBoard.Controllers
{
    [Authorize]
    [Route("messages")]
    public class MessageController : Controller
    {
        private readonly MessageRepository _messages;
        private readonly ViewMessageService _service;

        public MessageController(MessageRepository messages, ApplicationDbContext dbContext)
        {
            _messages = messages;
            _service = new ViewMessageService(messages, dbContext);
        }
        
        // GET: /<controller>/
        [AllowAnonymous]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var messages = await _service.GetAllMessagesAsync();

            ViewBag.Heading = "All Messages";

            return View("DisplayList", messages);
        }

        [AllowAnonymous]
        [Route("~/api/messages")]
        public async Task<IEnumerable<MessageViewModel>> GetMessagesAsync()
        {
            var messages = await _service.GetAllMessagesAsync();

            return messages;
        }

        [AllowAnonymous]
        [Route("{username}")]
        public async Task<IActionResult> DisplayByUser(string username)
        {
            var messages = await _service.GetAllMessagesAsync(username);

            if (messages == null)
            {
                return HttpNotFound();
            }

            ViewBag.Heading = "Messages by " + username;


            return View("DisplayList", messages);
        }

        [AllowAnonymous]
        [Route("~/api/messages/{username}")]
        public async Task<IActionResult> GetMessagesByUserAsync(string username)
        {
            var messages = await _service.GetAllMessagesAsync(username);

            if (messages == null)
            {
                return HttpNotFound();
            }

            return new ObjectResult(messages);
        }

        [AllowAnonymous]
        [Route("{id:int}")]
        public async Task<IActionResult> DisplayById(int id)
        {
            var message = await _service.GetMessageAsync(id);

            if (message == null)
            {
                return HttpNotFound();
            }

            return View("DisplaySingle", message);
        }

        [AllowAnonymous]
        [Route("~/api/messages/{id:int}")]
        public async Task<IActionResult> GetMessageByIdAsync(int id)
        {
            var message = await _service.GetMessageAsync(id);

            if (message == null)
            {
                return HttpNotFound();
            }

            return new ObjectResult(message);
        }

        // /messsage/create
        [HttpGet("[action]")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Message model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Created = DateTime.Now;
            model.UserId = User.GetUserId();

            _messages.Add(model);

            return RedirectToAction("index");
        }

        [HttpGet("{id}/[action]")]
        public IActionResult Edit(int id)
        {
            var message = _messages.GetBy(id);

            if (message == null)
            {
                return HttpNotFound();
            }

            if (message.UserId != User.GetUserId())
            {
                return HttpUnauthorized();
            }

            return View(message);
        }

        [HttpPost("{id}/[action]")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Message model)
        {
            var message = _messages.GetBy(id);

            if (message == null)
            {
                return HttpNotFound();
            }

            if (message.UserId != User.GetUserId())
            {
                return HttpUnauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            message.MessageTitle = model.MessageTitle;
            message.MessageContent = model.MessageContent;

            _messages.Update(message);


            return RedirectToAction("index");
        }
    }
}
