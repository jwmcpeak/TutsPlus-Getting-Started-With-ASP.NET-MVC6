using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoundBoard.Models
{
    public class Message
    {
        [Required(ErrorMessage ="Please supply a title.")]
        [Display(Name = "Title")]
        public string MessageTitle { get; set; }

        [Required(ErrorMessage = "Please supply content.")]
        [Display(Name = "Content")]
        public string MessageContent { get; set; }

    }
}
