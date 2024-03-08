using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BE_W06WeekProject.Models;

namespace BE_W06WeekProject.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            List<Cliente> clientiList = new List<Cliente>();

            try
            {
                conn.Open();
                string query = "SELECT * FROM Cliente";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Cliente cliente = new Cliente()
                    {
                        IDCliente = Convert.ToInt32(reader["IDCliente"]),
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString(),
                        CodiceFiscale = reader["CodiceFiscale"].ToString(),
                        Citta = reader["Citta"].ToString(),
                        Provincia = reader["Provincia"].ToString(),
                        Email = reader["Email"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Cellulare = reader["Cellulare"].ToString()
                    };
                    clientiList.Add(cliente);
                }
            }
            catch (Exception ex)
            {
                // Gestione dell'eccezione
                System.Diagnostics.Debug.WriteLine("Errore durante l'elaborazione dei dati del cliente: " + ex.Message);
            }
            finally
            {
                // Chiusura della connessione al database
                conn.Close();
            }

            return View(clientiList);
        }

        // CONTROLLO DEL CF
        // Controllo REMOTE che come paramentro prende la stringa del valore immesso in CodiceFiscale e restituisce un Json
        // Non funziona quindi addio lo lascio solo per far vedere che ci ho provato Mi sta facendo venire un esaurimento.
        /*
        [HttpPost]
        public ActionResult IsCodiceFiscaleAvailable(string codiceFiscale)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Clienti WHERE CodiceFiscale = @CodiceFiscale";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CodiceFiscale", codiceFiscale);
                int count = (int)cmd.ExecuteScalar();

                return Json(count == 0, JsonRequestBehavior.AllowGet);
            }
            catch (SqlException ex)
            {
                // Gestisci l'eccezione SQL
                return Json("Errore di connessione al database: " + ex.Message, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Close();
            }
        }
        */

        // METODO CREATE => NuovoCliente
        // COn la get prendiamo la vista
        // Con la post
        [HttpGet]
        public ActionResult NuovoCliente()
        {
            return View();
        }

        // Bonus: Aggiunta del super metodo debugging
        [HttpPost]
        public ActionResult NuovoCliente(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
                SqlConnection conn = new SqlConnection(connectionString);

                try
                {
                    conn.Open();
                    // Parte dedicata alla verifica del Codice Fiscale
                    SqlCommand com = conn.CreateCommand();

                    com.CommandText = "SELECT COUNT(*) FROM Cliente WHERE CodiceFiscale = @CodiceFiscale";
                    com.Parameters.AddWithValue("@CodiceFiscale", cliente.CodiceFiscale);

                    int count = (int)com.ExecuteScalar();

                    if (count > 0)
                    {
                        // Il codice fiscale esiste già nel database, mostra un messaggio di errore
                        TempData["Message"] = "Il codice fiscale inserito è già presente nel database.";
                        return RedirectToAction("Index", "Home");
                    }

                    // Se il codice fiscale non esiste nel database => l'inserimento del cliente
                    com = conn.CreateCommand();
                    com.CommandText = "INSERT INTO Cliente (Nome, Cognome, CodiceFiscale, Citta, Provincia, Email, Telefono, Cellulare) " +
                                      "VALUES (@Nome, @Cognome, @CodiceFiscale, @Citta, @Provincia, @Email, @Telefono, @Cellulare)";

                    com.Parameters.AddWithValue("@Nome", cliente.Nome);
                    com.Parameters.AddWithValue("@Cognome", cliente.Cognome);
                    com.Parameters.AddWithValue("@CodiceFiscale", cliente.CodiceFiscale);
                    com.Parameters.AddWithValue("@Citta", cliente.Citta);
                    com.Parameters.AddWithValue("@Provincia", cliente.Provincia);
                    com.Parameters.AddWithValue("@Email", cliente.Email);
                    com.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                    com.Parameters.AddWithValue("@Cellulare", cliente.Cellulare);

                    com.ExecuteNonQuery();

                    TempData["Message"] = "Cliente inserito con successo";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Si è verificato un errore durante il salvataggio del cliente. Riprova più tardi.";
                    System.Diagnostics.Debug.WriteLine("Errore:" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                // Log degli errori di ModelState
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList() })
                    .ToList();

                // Registrare nel log di sistema o in un file
                foreach (var entry in errors)
                {
                    foreach (var error in entry.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Errore nel campo {entry.Key}: {error}");
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// ///////////////////////////////////////////////////////////////////
        // Edit cliente 
        // Metodi get e post => Get restituisce la vista, post modifica il cliente
        // Non funge
        // GET: Cliente/Edit/5
        public ActionResult EditCliente(int id)
        {
            SqlConnection conn = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
                conn = new SqlConnection(connectionString);

                conn.Open();
                string query = "SELECT * FROM Cliente WHERE IDCliente = @IDCliente";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@IDCliente", id);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Cliente cliente = new Cliente()
                    {
                        IDCliente = Convert.ToInt32(reader["IDCliente"]),
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString(),
                        CodiceFiscale = reader["CodiceFiscale"].ToString(),
                        Citta = reader["Citta"].ToString(),
                        Provincia = reader["Provincia"].ToString(),
                        Email = reader["Email"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Cellulare = reader["Cellulare"].ToString()
                    };

                    return View(cliente);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                // Gestione dell'eccezione
                System.Diagnostics.Debug.WriteLine("Errore durante il recupero del cliente: " + ex.Message);
                return RedirectToAction("Index", "Home");
            }
            finally
            {
               conn.Close();
            }
        }

        [HttpPost]
        public ActionResult EditCliente(Cliente cliente)
        {
            SqlConnection conn = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
                conn = new SqlConnection(connectionString);
                conn.Open();
                string query = "UPDATE Cliente SET Nome = @Nome, Cognome = @Cognome, CodiceFiscale = @CodiceFiscale, Citta = @Citta, Provincia = @Provincia, Email = @Email, Telefono = @Telefono, Cellulare = @Cellulare WHERE IDCliente = @IDCliente";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Nome", cliente.Nome);
                command.Parameters.AddWithValue("@Cognome", cliente.Cognome);
                command.Parameters.AddWithValue("@CodiceFiscale", cliente.CodiceFiscale);
                command.Parameters.AddWithValue("@Citta", cliente.Citta);
                command.Parameters.AddWithValue("@Provincia", cliente.Provincia);
                command.Parameters.AddWithValue("@Email", cliente.Email);
                command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                command.Parameters.AddWithValue("@Cellulare", cliente.Cellulare);
                command.Parameters.AddWithValue("@IDCliente", cliente.IDCliente);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    TempData["Message"] = "Cliente modificato con successo";
                }
                else
                {
                    TempData["Message"] = "Nessuna modifica effettuata";
                }
                return RedirectToAction("Index", "Cliente");
            }
            catch (Exception ex)
            {
                // Gestione dell'eccezione
                System.Diagnostics.Debug.WriteLine("Errore durante il salvataggio del cliente: " + ex.Message);
                TempData["Message"] = "Si è verificato un errore durante il salvataggio del cliente. Riprova più tardi.";
                return RedirectToAction("Index", "Home");
            }
            finally
            {
               conn.Close();
            }
        }
        /// DELETE
        /// 
        [HttpGet]
        public ActionResult DeleteCliente(int id)
        {
            SqlConnection conn = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
                conn = new SqlConnection(connectionString);

                conn.Open();
                string query = "SELECT * FROM Cliente WHERE IDCliente = @IDCliente";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@IDCliente", id);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Cliente cliente = new Cliente()
                    {
                        IDCliente = Convert.ToInt32(reader["IDCliente"]),
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString(),
                        CodiceFiscale = reader["CodiceFiscale"].ToString(),
                        Citta = reader["Citta"].ToString(),
                        Provincia = reader["Provincia"].ToString(),
                        Email = reader["Email"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Cellulare = reader["Cellulare"].ToString()
                    };

                    return View(cliente);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                // Gestione dell'eccezione
                System.Diagnostics.Debug.WriteLine("Errore durante il recupero del cliente: " + ex.Message);
                return RedirectToAction("Index", "Home");
            }
            finally
            {
                conn.Close();
            }
        }

        [HttpPost, ActionName("DeleteCliente")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SqlConnection conn = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
                conn = new SqlConnection(connectionString);

                conn.Open();
                string query = "DELETE FROM Cliente WHERE IDCliente = @IDCliente";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@IDCliente", id);

                command.ExecuteNonQuery();

                TempData["Message"] = "Cliente eliminato con successo";
                return RedirectToAction("Index", "Cliente");
            }
            catch (Exception ex)
            {
                // Gestione dell'eccezione
                System.Diagnostics.Debug.WriteLine("Errore durante l'eliminazione del cliente: " + ex.Message);
                TempData["Message"] = "Si è verificato un errore durante l'eliminazione del cliente. Riprova più tardi.";
                return RedirectToAction("Index", "Home");
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
