using BE_W06L02.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;

namespace BE_W06L02.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            return View();
        }
        // Verifico che i valoro siano unici
        public JsonResult IsValueUnique(string columnName, string value)
        {           
            bool isUnique; // Variabile per memorizzare se il valore è unico nel database
            string connectionString = ConfigurationManager.ConnectionStrings["W06L01"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                // Query SQL dove il valore nella colonna specificata corrisponde al valore fornito
                string query = $"SELECT * FROM Cliente WHERE {columnName} = @Value";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Value", value);
                SqlDataReader reader = command.ExecuteReader();

                // Verifica se il lettore dati ha delle righe => se non ci sono righe, significa che il valore è unico nel database
                isUnique = !reader.HasRows;
                // Restituisce il risultato come JSON => questo perché un remote da un json come output !
                return Json(isUnique, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Errore:" + ex.Message);
                // Restituisci false come JSON in caso di errore
                return Json(false, JsonRequestBehavior.AllowGet);
                
            }
            finally
            {
                conn.Close();
            }
        }

        //Create del form da compilare per la registrazione
        [HttpGet]
        public ActionResult NuovoCliente() {
            return View();
                }

        [HttpPost]
        public ActionResult NuovoCliente(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["W06Spedizioni"].ConnectionString;
                SqlConnection conn = new SqlConnection(connectionString);
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Cliente (AziendaPrivato, Nome, Cognome, CodiceFiscale, PartitaIva, Indirizzo, Citta) " +
                           "VALUES (@AziendaPrivato, @Nome, @Cognome, @CodiceFiscale, @PartitaIva, @Indirizzo, @Citta)";

                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@AziendaPrivato", cliente.AziendaPrivato);
                    command.Parameters.AddWithValue("@Nome", cliente.Nome);
                    command.Parameters.AddWithValue("@Cognome", cliente.Cognome);
                    command.Parameters.AddWithValue("@CodiceFiscale", cliente.CodiceFiscale);
                    command.Parameters.AddWithValue("@PartitaIva", cliente.PartitaIva);
                    command.Parameters.AddWithValue("@Indirizzo", cliente.Indirizzo);
                    command.Parameters.AddWithValue("@Citta", cliente.Citta);

                    command.ExecuteNonQuery();

                    // Restituisci una vista di conferma o reindirizza ad un'altra azione dopo il salvataggio del cliente
                    return RedirectToAction("ClienteRegistrato");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Errore:" + ex.Message);
                    ModelState.AddModelError("", "Si è verificato un errore durante il salvataggio del cliente. Riprova più tardi.");
                }
                finally
                {
                    conn.Close();
                }
            }

            // Se il modello non è valido o si è verificato un errore, restituisci la vista con il modello
            return View(cliente);
        }

    }
}