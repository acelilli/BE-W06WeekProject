using BE_W06WeekProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BE_W06WeekProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        ////////////////// METODI ASYNC /////////////////////////////////////////
        ///// Ci proviamo e vediamo se fungono /////////////////////////////
 
        // Azione asincrona per ricercare le prenotazioni effettuate da un cliente in base al codice fiscale
        [HttpPost]
        public async Task<JsonResult> RicercaPrenotazioniCliente(string codiceFiscale)
        {
            List<Prenotazione> prenotazioniCliente = new List<Prenotazione>();

            string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            string query = @"
        SELECT p.* 
        FROM Prenotazione p 
        INNER JOIN Cliente c ON p.IDCliente = c.IDCliente 
        WHERE c.CodiceFiscale = @CodiceFiscale";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CodiceFiscale", codiceFiscale);

            SqlDataReader reader = null;

            try
            {
                await conn.OpenAsync();
                reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    // Popolare l'oggetto Prenotazione dai dati del reader
                    Prenotazione prenotazione = new Prenotazione
                    {
                        IDCliente = Convert.ToInt32(reader["IDCliente"]),
                        IDCamera = Convert.ToInt32(reader["IDCamera"]),
                        IDServizio = Convert.ToInt32(reader["IDServizio"]),
                        DataPrenotazione = Convert.ToDateTime(reader["DataPrenotazione"]),
                        CheckIn = Convert.ToDateTime(reader["CheckIn"]),
                        CheckOut = Convert.ToDateTime(reader["CheckOut"]),
                        Anticipo = Convert.ToDecimal(reader["Anticipo"]),
                        TotaleSaldo = Convert.ToDecimal(reader["TotaleSaldo"]),
                        // Assicurati di popolare tutti gli altri attributi della prenotazione necessari
                    };

                    prenotazioniCliente.Add(prenotazione);
                }
            }
            catch (Exception ex)
            {
                // Gestisci l'eccezione
                ViewBag.Error = "Si è verificato un errore durante la ricerca delle prenotazioni del cliente: " + ex.Message;
            }
            finally
            {
                // Chiudi il reader e la connessione
                if (reader != null)
                    reader.Close();

                conn.Close();
            }

            return Json(prenotazioniCliente, JsonRequestBehavior.AllowGet);
        }


        // Azione asincrona per ricercare il numero totale di prenotazioni per i soggiorni di tipo “pensione completa”
        // Non ho la tabella tipo i pensione, gg

    }
}