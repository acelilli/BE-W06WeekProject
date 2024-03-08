using BE_W06WeekProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BE_W06WeekProject.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserGestione user)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Polaris"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM UserGestione WHERE NomeUtente = @NomeUtente AND Password = @Password", conn);
                cmd.Parameters.AddWithValue("@NomeUtente", user.NomeUtente);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    FormsAuthentication.SetAuthCookie(user.NomeUtente, false);
                    conn.Close();
                    reader.Close();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.AuthError = "Autenticazione non riuscita";
                }
            }
            catch (Exception ex)
            {
                ViewBag.AuthError = "Errore durante l'autenticazione: " + ex.Message;
                System.Diagnostics.Debug.WriteLine("Errore: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}