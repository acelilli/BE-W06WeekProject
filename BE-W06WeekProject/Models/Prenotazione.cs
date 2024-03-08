using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BE_W06WeekProject.Models
{
    public class Prenotazione
    {
        public int IDPrenotazione { get; set; }

        [DisplayName("Cliente")]
        [Required(ErrorMessage = "Il campo Cliente è obbligatorio.")]
        public int IDCliente { get; set; }
        public List<SelectListItem> ClientList { get; set; }

        [DisplayName("Camera")]
        [Required(ErrorMessage = "Il campo Camera è obbligatorio.")]
        public int IDCamera { get; set; }
        public List<SelectListItem> RoomList { get; set; } // Lista delle camere

        [DisplayName("Servizio")]
        [Required(ErrorMessage = "Il campo Servizio è obbligatorio.")]
        public int IDServizio { get; set; }
        public List<SelectListItem> ServicesList { get; set; } // Lista dei servizi aggiuntivi

        [DisplayName("Data Prenotazione")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Il campo Data Prenotazione è obbligatorio.")]

        public DateTime DataPrenotazione { get; set; }

        [DisplayName("Check-In")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Il campo Check-In è obbligatorio.")]
        public DateTime CheckIn { get; set; }

        [DisplayName("Check-Out")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Il campo Check-Out è obbligatorio.")]
        public DateTime CheckOut { get; set; }

        [DisplayName("Anticipo")]
        [Required(ErrorMessage = "Il campo Anticipo è obbligatorio.")]
        public decimal Anticipo { get; set; }

        [DisplayName("Totale Saldo")]
        [Required(ErrorMessage = "Il campo Totale Saldo è obbligatorio.")]
        public decimal TotaleSaldo { get; set; }

        public Prenotazione()
        {
            DataPrenotazione = DateTime.Now;
            ClientList = new List<SelectListItem>();
            RoomList = new List<SelectListItem>();
            ServicesList = new List<SelectListItem>();
        }
    }
}
