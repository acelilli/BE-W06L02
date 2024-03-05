using BE_W06L02.Models;
using Newtonsoft.Json.Linq;
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
        // Verifico che i valori siano unici
       /* Fallimenti cya
        * public JsonResult IsValueUnique(string columnName, string value)
        {
            bool isUnique;
            string connectionString = ConfigurationManager.ConnectionStrings["W06L01"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = $"SELECT COUNT(*) FROM Cliente WHERE {columnName} = @Value";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@Value", value);
                    int count = (int)command.ExecuteScalar();
                    isUnique = count == 0;

                    return Json(isUnique, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Errore:" + ex.Message);
                    return Json(false, JsonRequestBehavior.AllowGet);
                } */
   
  

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
                string connectionString = ConfigurationManager.ConnectionStrings["W06L01"].ConnectionString;
                SqlConnection conn = new SqlConnection(connectionString);
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Cliente (AziendaPrivato, Nome, Cognome, CodiceFiscale, PartitaIva, Indirizzo, Citta) " +
                           "VALUES (@AziendaPrivato, @Nome, @Cognome, @CodiceFiscale, @PartitaIva, @Indirizzo, @Citta)";

                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@AziendaPrivato", cliente.AziendaPrivato);
                    command.Parameters.AddWithValue("@Nome", cliente.Nome);
                    command.Parameters.AddWithValue("@Cognome", (object)cliente.Cognome ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CodiceFiscale", (object)cliente.CodiceFiscale ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PartitaIva", (object)cliente.PartitaIva ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Indirizzo", cliente.Indirizzo);
                    command.Parameters.AddWithValue("@Citta", cliente.Citta);

                    command.ExecuteNonQuery();

                    // Restituisci una vista di conferma o reindirizza ad un'altra azione dopo il salvataggio del cliente
                    
                    return View(cliente);
                    
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