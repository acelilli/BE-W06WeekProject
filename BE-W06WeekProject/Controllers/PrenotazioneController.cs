using BE_W06WeekProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_W06WeekProject.Controllers
{
    public class PrenotazioneController : Controller
    {
        // GET: Prenotazione
        [Authorize(Roles = "PolarisHead, PolarisStaff")]
        public ActionResult Index()
        {
            List<Prenotazione> prenotazioniList = new List<Prenotazione>();
            SqlConnection conn = null;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
                conn = new SqlConnection(connectionString);
                string query = "SELECT * FROM Prenotazione";
                SqlCommand command = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Prenotazione prenotazione = new Prenotazione
                    {
                        IDPrenotazione = (int)reader["IDPrenotazione"],
                        IDCliente = (int)reader["IDCliente"],
                        IDCamera = (int)reader["IDCamera"],
                        IDServizio = (int)reader["IDServizio"],
                        DataPrenotazione = (DateTime)reader["DataPrenotazione"],
                        CheckIn = (DateTime)reader["CheckIn"],
                        CheckOut = (DateTime)reader["CheckOut"],
                        Anticipo = (decimal)reader["Anticipo"],
                        TotaleSaldo = (decimal)reader["TotaleSaldo"]
                    };

                    prenotazioniList.Add(prenotazione);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Gestisci l'eccezione o registra eventuali errori
                System.Diagnostics.Debug.WriteLine("Errore: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return View(prenotazioniList);
        }


        [HttpGet]
        public ActionResult NuovaPrenotazione()
        {
            // Ottenere le liste di clienti, camere e servizi per popolare i menu a tendina
            var clientList = GetClientList();
            var roomList = GetRoomList();
            var servicesList = GetServicesList();

            // Creare un modello di Prenotazione e popolarlo con le liste ottenute
            var model = new Prenotazione
            {
                ClientList = clientList,
                RoomList = roomList,
                ServicesList = servicesList,
                DataPrenotazione = DateTime.Now,
            };

            // Restituire la vista con il modello
            return View(model);
        }

        // Metodo per gestire il submit del form per la creazione di una nuova prenotazione
        [HttpPost]
        public ActionResult NuovaPrenotazione(Prenotazione prenotazione)
        {
            if (ModelState.IsValid)
            {
                SqlConnection conn = null;
                try
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
                    conn = new SqlConnection(connectionString);
                    string query = @"INSERT INTO Prenotazione (IDCliente, IDCamera, IDServizio, DataPrenotazione, CheckIn, CheckOut, Anticipo, TotaleSaldo ) 
                                    VALUES (@IDCliente, @IDCamera, @IDServizio, @DataPrenotazione, @CheckIn, @CheckOut, @Anticipo, @TotaleSaldo )";

                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@IDCliente", prenotazione.IDCliente);
                    command.Parameters.AddWithValue("@IDCamera", prenotazione.IDCamera);
                    command.Parameters.AddWithValue("@IDServizio", prenotazione.IDServizio);
                    command.Parameters.AddWithValue("@DataPrenotazione", prenotazione.DataPrenotazione);
                    command.Parameters.AddWithValue("@CheckIn", prenotazione.CheckIn);
                    command.Parameters.AddWithValue("@CheckOut", prenotazione.CheckOut);
                    command.Parameters.AddWithValue("@Anticipo", prenotazione.Anticipo);
                    command.Parameters.AddWithValue("@TotaleSaldo", prenotazione.TotaleSaldo);


                    conn.Open();
                    command.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "ERRORE:" + ex.Message + ".");
                }
                finally
                {
                    conn.Close();
                }
            }
            return View(prenotazione);
        }

        //// METODI DELLE LISTE -> Ottenere i dati dal DB per generare le liste da visualizzare nel menù a tendina durante la creazione della prenotazione.
        ///
        // Metodo per ottenere la lista dei clienti
        private List<SelectListItem> GetClientList()
        {
            var clientiList = new List<SelectListItem>();

            // Connessione al database per ottenere la lista dei clienti
            string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = "SELECT IdCliente, Nome, Cognome FROM Cliente";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var item = new SelectListItem
                    {
                        Value = reader["IdCliente"].ToString(),
                        Text = $"{reader["Nome"]} {reader["Cognome"]}"
                    };
                    clientiList.Add(item);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Errore nel recupero dati clienti" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return clientiList;
        }

        // Metodo per ottenere la lista delle camere
        private List<SelectListItem> GetRoomList()
        {
            var roomList = new List<SelectListItem>();

            // Connessione al database per ottenere la lista delle camere
            string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = "SELECT IDCamera, NumeroCamera FROM Camera";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var item = new SelectListItem
                    {
                        Value = reader["IDCamera"].ToString(),
                        Text = $"N. {reader["NumeroCamera"]}"
                    };
                    roomList.Add(item);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Errore nel recupero dati camere" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return roomList;
        }

        // Metodo per ottenere la lista dei servizi
        private List<SelectListItem> GetServicesList()
        {
            var servicesList = new List<SelectListItem>();

            // Connessione al database per ottenere la lista dei servizi
            string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = "SELECT IDServizio, Nome FROM Servizio";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var item = new SelectListItem
                    {
                        Value = reader["IDServizio"].ToString(),
                        Text = reader["Nome"].ToString()
                    };
                    servicesList.Add(item);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Errore nel recupero dati servizi" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return servicesList;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// FINE METODI DEL CREATE /////////////////////////////////////////////////
        ////////  INIZIO GRUPPO EDIT ////////////////////////////////////////////////
        /// EDIT NON FUNZIONANTE /////
        ///// Metodo per recuperare una prenotazione dal database tramite l'id
        private Prenotazione GetPrenotazioneById(int id)
        {
            Prenotazione prenotazione = null;
            SqlConnection conn = null;
            string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
            try
            {
                conn = new SqlConnection(connectionString);
                string query = "SELECT * FROM Prenotazione WHERE IDPrenotazione = @ID";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@ID", id);

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    // Costruisci l'oggetto prenotazione utilizzando i dati recuperati dal database
                    prenotazione = new Prenotazione
                    {
                        IDPrenotazione = (int)reader["IDPrenotazione"],
                        IDCliente = (int)reader["IDCliente"],
                        IDCamera = (int)reader["IDCamera"],
                        IDServizio = (int)reader["IDServizio"],
                        DataPrenotazione = (DateTime)reader["DataPrenotazione"],
                        CheckIn = (DateTime)reader["CheckIn"],
                        CheckOut = (DateTime)reader["CheckOut"],
                        Anticipo = (decimal)reader["Anticipo"],
                        TotaleSaldo = (decimal)reader["TotaleSaldo"]
                    };
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                // Gestisci l'eccezione o registra eventuali errori
                System.Diagnostics.Debug.WriteLine("Errore: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return prenotazione;
        }

        // GET: Prende, tramite l'id (parametro + metodo associato) la prenotazione
        [HttpGet]
        public ActionResult EditPrenotazione(int id)
        {
            // Recupera la prenotazione dal database tramite l'id
            Prenotazione prenotazione = GetPrenotazioneById(id);

            // Se la prenotazione non esiste, ritorna HttpNotFound
            if (prenotazione == null)
            {
                return HttpNotFound();
            }

            // Popola le liste necessarie
            prenotazione.ClientList = GetClientList();
            prenotazione.RoomList = GetRoomList();
            prenotazione.ServicesList = GetServicesList();

            return View(prenotazione);
        }

        // POST: Tramite il metodo, usando l'oggetto prenotazione legato al relativo modello, modifico i valori.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPrenotazione(Prenotazione prenotazione)
        {
            if (ModelState.IsValid)
            {
                SqlConnection conn = null;
                try
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
                    conn = new SqlConnection(connectionString);

                    // Query per aggiornare la prenotazione nel database
                    string query = @"UPDATE Prenotazione 
                             SET IDCliente = @IDCliente, 
                                 IDCamera = @IDCamera, 
                                 IDServizio = @IDServizio, 
                                 DataPrenotazione = @DataPrenotazione, 
                                 CheckIn = @CheckIn, 
                                 CheckOut = @CheckOut, 
                                 Anticipo = @Anticipo, 
                                 TotaleSaldo = @TotaleSaldo 
                             WHERE IDPrenotazione = @IDPrenotazione";

                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@IDCliente", prenotazione.IDCliente);
                    command.Parameters.AddWithValue("@IDCamera", prenotazione.IDCamera);
                    command.Parameters.AddWithValue("@IDServizio", prenotazione.IDServizio);
                    command.Parameters.AddWithValue("@DataPrenotazione", prenotazione.DataPrenotazione);
                    command.Parameters.AddWithValue("@CheckIn", prenotazione.CheckIn);
                    command.Parameters.AddWithValue("@CheckOut", prenotazione.CheckOut);
                    command.Parameters.AddWithValue("@Anticipo", prenotazione.Anticipo);
                    command.Parameters.AddWithValue("@TotaleSaldo", prenotazione.TotaleSaldo);
                    command.Parameters.AddWithValue("@IDPrenotazione", prenotazione.IDPrenotazione);

                    conn.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        TempData["Message"] = "Prenotazione modificata con successo";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Message"] = "Nessuna modifica effettuata";
                    }
                }
                catch (Exception ex)
                {
                    // Gestisci l'eccezione o registra eventuali errori
                    System.Diagnostics.Debug.WriteLine("Errore: " + ex.Message);
                    TempData["Message"] = "Si è verificato un errore durante la modifica della prenotazione. Riprova più tardi.";
                }
                finally
                {
                    conn.Close();
                }
            }
            // Se il modello non è valido o si è verificato un errore durante l'aggiornamento,
            // ritorna alla vista di modifica con il modello e le liste aggiornate
            prenotazione.ClientList = GetClientList();
            prenotazione.RoomList = GetRoomList();
            prenotazione.ServicesList = GetServicesList();

            return View(prenotazione);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////// METODI DELETE ////////////////////////////////////////////////////////////////////
        ///
        // GET: -> Utilizzando GetPrenotazioneById recupera i dati della prenotazione
        public ActionResult DeletePrenotazione(int id)
        {
            Prenotazione prenotazione = GetPrenotazioneById(id);
            if (prenotazione == null)
            {
                return HttpNotFound();
            }

            return View(prenotazione);
        }

        // POST: La vera delete dopo aver confermato. Cancella dal database il record.
        [HttpPost, ActionName("DeletePrenotazione")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SqlConnection conn = null;
            try
            {
                Prenotazione prenotazione = GetPrenotazioneById(id);
                if (prenotazione == null)
                {
                    return HttpNotFound();
                }

                string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                command.CommandText = "DELETE FROM Prenotazione WHERE IDPrenotazione = @IDPrenotazione";
                command.Parameters.AddWithValue("@IDPrenotazione", id);
                command.ExecuteNonQuery();

                TempData["Message"] = "Prenotazione eliminata con successo";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Errore: " + ex.Message);
                TempData["ErrorMessage"] = "Si è verificato un errore durante l'eliminazione della prenotazione.";
                return RedirectToAction("Index");
            }
            finally
            {
                conn.Close();
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////// FINE METODI DELETE //////////////////////////////////////////////////
        //////////////////// INIZIO METODI DETTAGLI ///////////////////////////////////
        public ActionResult DettagliCheckout(int id)
        {
            CheckoutDetails checkoutDetails = new CheckoutDetails();

            string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            string query = @"
        SELECT 
            c.Nome AS NomeCliente, c.Cognome AS CognomeCliente, c.CodiceFiscale,
            c.Citta AS CittaCliente, c.Provincia AS ProvinciaCliente,
            c.Email, c.Telefono, c.Cellulare,
            cam.NumeroCamera, cam.Descrizione AS DescrizioneCamera, cam.TipoCamera,
            p.IDPrenotazione, p.DataPrenotazione, p.CheckIn, p.CheckOut, p.Anticipo, p.TotaleSaldo,
            s.Nome AS NomeServizio, s.Descrizione AS DescrizioneServizio, s.Prezzo
        FROM Prenotazione p
        INNER JOIN Cliente c ON p.IDCliente = c.IDCliente
        INNER JOIN Camera cam ON p.IDCamera = cam.IDCamera
        INNER JOIN Servizio s ON p.IDServizio = s.IDServizio
        WHERE p.IDPrenotazione = @IDPrenotazione";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@IDPrenotazione", id);

            SqlDataReader reader = null;

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    checkoutDetails.NomeCliente = reader["NomeCliente"].ToString();
                    checkoutDetails.CognomeCliente = reader["CognomeCliente"].ToString();
                    checkoutDetails.CodiceFiscale = reader["CodiceFiscale"].ToString();
                    checkoutDetails.CittaCliente = reader["CittaCliente"].ToString();
                    checkoutDetails.ProvinciaCliente = reader["ProvinciaCliente"].ToString();
                    checkoutDetails.EmailCliente = reader["Email"].ToString();
                    checkoutDetails.TelefonoCliente = reader["Telefono"].ToString();
                    checkoutDetails.CellulareCliente = reader["Cellulare"].ToString();
                    checkoutDetails.NumeroCamera = reader["NumeroCamera"].ToString();
                    checkoutDetails.DescrizioneCamera = reader["DescrizioneCamera"].ToString();
                    checkoutDetails.TipoCamera = reader["TipoCamera"].ToString();
                    checkoutDetails.IDPrenotazione = Convert.ToInt32(reader["IDPrenotazione"]);
                    checkoutDetails.DataPrenotazione = Convert.ToDateTime(reader["DataPrenotazione"]);
                    checkoutDetails.CheckIn = Convert.ToDateTime(reader["CheckIn"]);
                    checkoutDetails.CheckOut = Convert.ToDateTime(reader["CheckOut"]);
                    checkoutDetails.Anticipo = Convert.ToDecimal(reader["Anticipo"]);
                    checkoutDetails.TotaleSaldo = Convert.ToDecimal(reader["TotaleSaldo"]);
                    checkoutDetails.NomeServizio = reader["NomeServizio"].ToString();
                    checkoutDetails.DescrizioneServizio = reader["DescrizioneServizio"].ToString();
                    checkoutDetails.PrezzoServizio = Convert.ToDecimal(reader["Prezzo"]);
                }
            }
            catch (Exception ex)
            {
                // Gestisci l'eccezione
                ViewBag.Error = "Si è verificato un errore durante il recupero dei dettagli della prenotazione: " + ex.Message;
            }
            finally
            {
                // Chiudi il reader e la connessione
                if (reader != null)
                    reader.Close();

                conn.Close();
            }

            return View(checkoutDetails);
        }

    }
}