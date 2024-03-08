using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BE_W06WeekProject.Models
{
    public class CheckoutDetails
    {
        [Display(Name ="Nome:")]
        public string NomeCliente { get; set; }
        [Display(Name = "Cognome:")]
        public string CognomeCliente { get; set; }
        [Display(Name = "Codice Fiscale:")]
        public string CodiceFiscale { get; set; }
        [Display(Name = "Città:")]
        public string CittaCliente { get; set; }
        [Display(Name = "Provincia:")]
        public string ProvinciaCliente { get; set; }
        [Display(Name = "Email:")]
        public string EmailCliente { get; set; }
        [Display(Name = "Telefoono:")]
        public string TelefonoCliente { get; set; }
        [Display(Name = "Cellulare:")]
        public string CellulareCliente { get; set; }
        [Display(Name = "Numero stanza prenotata:")]
        public string NumeroCamera { get; set; }
        [Display(Name = "Descrizione stanza prenotata:")]
        public string DescrizioneCamera { get; set; }
        [Display(Name = "Tipologia camera prenotata:")]
        public string TipoCamera { get; set; }
        public int IDPrenotazione { get; set; }
        [Display(Name = "Data della prenotazione:")]
        public DateTime DataPrenotazione { get; set; }
        [Display(Name = "Orario Check In:")]
        public DateTime CheckIn { get; set; }
        [Display(Name = "Orario Check Out:")]
        public DateTime CheckOut { get; set; }
        [Display(Name = "Anticipo")]
        public decimal Anticipo { get; set; }
        [Display(Name = "Saldo:")]
        public decimal TotaleSaldo { get; set; }
        [Display(Name = "Servizio aggiuntivo:")]
        public string NomeServizio { get; set; }
        [Display(Name = "Descrizione servizio aggiuntivo:")]
        public string DescrizioneServizio { get; set; }
        [Display(Name = "Prezzo del servizio aggiuntivo:")]
        public decimal PrezzoServizio { get; set; }
    }
}