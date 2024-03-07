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
    [Authorize(Roles = "admin, employee")]
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["W06L01"].ConnectionString;
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
                        IdCliente = Convert.ToInt32(reader["IdCliente"]),
                        AziendaPrivato = Convert.ToBoolean(reader["AziendaPrivato"]),
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString(),
                        CodiceFiscale = reader["CodiceFiscale"].ToString(),
                        PartitaIva = reader["PartitaIva"].ToString(),
                        Indirizzo = reader["Indirizzo"].ToString(),
                        Citta = reader["Citta"].ToString()
                    };
                    clientiList.Add(cliente);
                }
            }
            catch { }
            finally
            {
                conn.Close(); 
            }
            return View(clientiList); 
        }

        [Authorize(Roles = "admin, employee")]
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


                    TempData["Message"] = "Cliente inserito con successo";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Errore:" + ex.Message);
                    TempData["Message"] = "Si è verificato un errore durante il salvataggio del cliente. Riprova più tardi.";
                }
                finally
                {
                    conn.Close();
                }
            }
            return RedirectToAction("Index", "Home");
        }

    }
}