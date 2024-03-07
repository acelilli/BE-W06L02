using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;
using BE_W06L02.Models;

namespace MVC_Autenticazione.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Utente utente)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["W06L01"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Utente WHERE Username = @Username AND Password = @Password", conn);
                cmd.Parameters.AddWithValue("@Username", utente.Username);
                cmd.Parameters.AddWithValue("@Password", utente.Password);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    FormsAuthentication.SetAuthCookie(utente.Username, false);
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
            return View();
        }
    }
}
