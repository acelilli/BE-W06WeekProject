using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BE_W06WeekProject.Models
{
    public class UserGestione
    {
        public int IDUserGestione { get; set; }

        [DisplayName("Nome Utente")]
        [Required(ErrorMessage = "Il campo Nome Utente è obbligatorio.")]
        public string NomeUtente { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "Il campo Password è obbligatorio.")]
        public string Password { get; set; }
    }
}