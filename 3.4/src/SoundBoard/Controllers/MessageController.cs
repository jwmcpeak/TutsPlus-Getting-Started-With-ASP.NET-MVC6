using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SoundBoard.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SoundBoard.Controllers
{
    public class MessageController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // /messsage/create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Message model)
        {
            // todo: validate
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // todo: store

            return RedirectToAction("index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // todo: retrieve message
            var message = new Message
            {
                MessageTitle = "A sample message",
                MessageContent = "This the content"
            };

            if (message == null)
            {
                return HttpNotFound();
            }

            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Message model)
        {
            // todo: retrieve message
            var message = new Message();

            if (message == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // todo: update and store
            message.MessageTitle = model.MessageTitle;
            message.MessageContent = model.MessageContent;


            return RedirectToAction("index");
        }
    }
}
