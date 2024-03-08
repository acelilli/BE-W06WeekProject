using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BE_W06WeekProject.Models
{
    public class Servizio
    {
        public int IDServizio { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Il campo Nome è obbligatorio.")]
        public string Nome { get; set; }

        [DisplayName("Descrizione")]
        [Required(ErrorMessage = "Il campo Descrizione è obbligatorio.")]
        public string Descrizione { get; set; }

        [DisplayName("Prezzo")]
        [Required(ErrorMessage = "Il campo Prezzo è obbligatorio.")]
        public decimal Prezzo { get; set; }
    }
}