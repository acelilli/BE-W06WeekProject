using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_W06WeekProject.Models
{
    public class Camera
    {
        public int IDCamera { get; set; }

        [DisplayName("Numero Camera")]
        [Required(ErrorMessage = "Il campo Numero Camera è obbligatorio.")]
        [Remote("CameraEsistente", "Cliente", ErrorMessage = "Cliente già inserito")]
        public int NumeroCamera { get; set; }

        [DisplayName("Descrizione")]
        [Required(ErrorMessage = "Il campo Descrizione è obbligatorio.")]
        public string Descrizione { get; set; }

        [DisplayName("Tipo Camera")]
        [Required(ErrorMessage = "Il campo Tipo Camera è obbligatorio.")]
        public string TipoCamera { get; set; }
    }
}