using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_W06WeekProject.Models
{
    public class Cliente
    {

        public int IDCliente { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Il campo Nome è obbligatorio.")]
        public string Nome { get; set; }

        [DisplayName("Cognome")]
        [Required(ErrorMessage = "Il campo Cognome è obbligatorio.")]
        public string Cognome { get; set; }

        [DisplayName("Codice Fiscale")]
        [Required(ErrorMessage = "Il campo Codice Fiscale è obbligatorio.")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Il Codice Fiscale deve essere lungo 16 caratteri.")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Il Codice Fiscale può contenere solo lettere e numeri.")]
        /* [Remote("IsCodiceFiscaleAvailable", "Cliente", ErrorMessage = "Cliente già inserito")] */
        public string CodiceFiscale { get; set; }

        [DisplayName("Città")]
        [Required(ErrorMessage = "Il campo Città è obbligatorio.")]
        public string Citta { get; set; }

        [DisplayName("Provincia")]
        [Required(ErrorMessage = "Il campo Provincia è obbligatorio.")]
        public string Provincia { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Il campo Email è obbligatorio.")]
        [EmailAddress(ErrorMessage = "Il campo Email deve essere un indirizzo email valido.")]
        public string Email { get; set; }

        [DisplayName("Telefono")]
        [Required(ErrorMessage = "Il campo Telefono è obbligatorio.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Il campo Telefono deve contenere solo numeri.")]
        public string Telefono { get; set; }

        [DisplayName("Cellulare")]
        [Required(ErrorMessage = "Il campo Cellulare è obbligatorio.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Il campo Cellulare deve contenere solo numeri.")]
        public string Cellulare { get; set; }
    }
}