using BE_W06L02.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_W06L02.Controllers
{
    public class SpedizioneController : Controller
    {
        // GET: Spedizione
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NuovaSpedizione()
        {
            // Ottenimento delle liste per i menu a tendina
            var clientiList = GetClientiList(); //metodo alla fine

            var model = new Spedizione
            {
                ClientiItems = clientiList,
            };
            return View(model);
        }

        // Metodo per gestire il submit del form per la creazione di una nuova spedizione
        [HttpPost]
        public ActionResult NuovaSpedizione(Spedizione spedizione)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["W06L01"].ConnectionString;
                SqlConnection conn = new SqlConnection(connectionString);
                try
                {
                    string query = @"INSERT INTO Spedizione (IdCliente, DataSpedizione, Peso, CittaDestinazione, 
                                IndirizzoDestinazione, NominativoDestinatario, SpeseSpedizione, DataConsegnaPrevista) 
                                VALUES (@IdCliente, @DataSpedizione, @Peso, @CittaDestinazione, 
                                @IndirizzoDestinazione, @NominativoDestinatario, @SpeseSpedizione, @DataConsegnaPrevista)";

                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@IdCliente", spedizione.IdCliente);
                    command.Parameters.AddWithValue("@DataSpedizione", spedizione.DataSpedizione ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Peso", spedizione.Peso);
                    command.Parameters.AddWithValue("@CittaDestinazione", spedizione.CittaDestinazione);
                    command.Parameters.AddWithValue("@IndirizzoDestinazione", spedizione.IndirizzoDestinazione);
                    command.Parameters.AddWithValue("@NominativoDestinatario", spedizione.NominativoDestinatario);
                    command.Parameters.AddWithValue("@SpeseSpedizione", spedizione.SpeseSpedizione);
                    command.Parameters.AddWithValue("@DataConsegnaPrevista", spedizione.DataConsegnaPrevista ?? (object)DBNull.Value);

                    conn.Open();
                    command.ExecuteNonQuery();
                    // In alternativa, reindirizza ad un'altra azione o restituisci una vista di conferma
                    return View(spedizione);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Errore: " + ex.Message);
                    ModelState.AddModelError("", "Si è verificato un errore durante il salvataggio della spedizione. Riprova più tardi.");
                }
                finally
                {
                    conn.Close();
                }
            }

            return View(spedizione);
        }

        // GET CLIENTI LIST per riempire il menù a tendina
        private List<SelectListItem> GetClientiList()
        {
            var clientiList = new List<SelectListItem>();

            // Connessione al database e recupero delle opzioni per l'anagrafica
            string connectionString = ConfigurationManager.ConnectionStrings["W06L01"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
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
                    System.Diagnostics.Debug.WriteLine("Errore nella richiesta SQL" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            return clientiList;
        }
    }
}