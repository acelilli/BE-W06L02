using BE_W06L02.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;

namespace BE_W06L02.Controllers
{
    public class SpedizioneController : Controller
    {
        [Authorize(Roles = "admin, employee")]
        // GET: Spedizione
        public ActionResult Index()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["W06L01"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            List<Spedizione> spedizioniList = new List<Spedizione>();

            try
            {
                conn.Open();
                string query = "SELECT * FROM Spedizione";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Spedizione spedizione = new Spedizione()
                    {
                        IdSpedizione = Convert.ToInt32(reader["IdSpedizione"]),
                        IdCliente = Convert.ToInt32(reader["IdCliente"]),
                        DataSpedizione = reader["DataSpedizione"] as DateTime?,
                        Peso = Convert.ToDecimal(reader["Peso"]),
                        CittaDestinazione = reader["CittaDestinazione"].ToString(),
                        IndirizzoDestinazione = reader["IndirizzoDestinazione"].ToString(),
                        NominativoDestinatario = reader["NominativoDestinatario"].ToString(),
                        SpeseSpedizione = Convert.ToDecimal(reader["SpeseSpedizione"]),
                        DataConsegnaPrevista = reader["DataConsegnaPrevista"] as DateTime?,
                        Status = reader["Status"].ToString()
                    };
                    spedizioniList.Add(spedizione);
                }
            }
            catch (Exception ex)
            {
                // Gestione dell'eccezione
                System.Diagnostics.Debug.WriteLine("Errore: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return View(spedizioniList);
        }

        [Authorize(Roles = "admin, employee")]
        [HttpGet]
        public ActionResult NuovaSpedizione()
        {
            // Ottenimento delle liste per i menu a tendina
            var clientiList = GetClientiList();
            var statusList = GetStatiConsegnaList();

            var model = new Spedizione
            {
                ClientiItems = clientiList,
                StatusItems = statusList,
            };
            return View(model);
        }

        // Metodo per gestire il submit del form per la creazione di una nuova spedizione
        [HttpPost]
        public ActionResult NuovaSpedizione(Spedizione spedizione)
        {
            if (ModelState.IsValid)
            {
                SqlConnection conn = null;
                try
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["W06L01"].ConnectionString;
                    conn = new SqlConnection(connectionString);
                    string query = @"INSERT INTO Spedizione (IdCliente, DataSpedizione, Peso, CittaDestinazione, 
                                IndirizzoDestinazione, NominativoDestinatario, SpeseSpedizione, DataConsegnaPrevista, Status) 
                                VALUES (@IdCliente, @DataSpedizione, @Peso, @CittaDestinazione, 
                                @IndirizzoDestinazione, @NominativoDestinatario, @SpeseSpedizione, @DataConsegnaPrevista, @Status)";

                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@IdCliente", spedizione.IdCliente);
                    command.Parameters.AddWithValue("@DataSpedizione", spedizione.DataSpedizione ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Peso", spedizione.Peso);
                    command.Parameters.AddWithValue("@CittaDestinazione", spedizione.CittaDestinazione);
                    command.Parameters.AddWithValue("@IndirizzoDestinazione", spedizione.IndirizzoDestinazione);
                    command.Parameters.AddWithValue("@NominativoDestinatario", spedizione.NominativoDestinatario);
                    command.Parameters.AddWithValue("@SpeseSpedizione", spedizione.SpeseSpedizione);
                    command.Parameters.AddWithValue("@DataConsegnaPrevista", spedizione.DataConsegnaPrevista ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Status", spedizione.Status);

                    conn.Open();
                    command.ExecuteNonQuery();
                    return RedirectToAction("Index");
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

            // Se il modello non è valido, torna alla vista con il modello per correggere gli errori
            return View(spedizione);
        }

        // GET CLIENTI LIST per riempire il menù a tendina
        private List<SelectListItem> GetClientiList()
        {
            var clientiList = new List<SelectListItem>();

            // Connessione al database e recupero delle opzioni per l'anagrafica
            string connectionString = ConfigurationManager.ConnectionStrings["W06L01"].ConnectionString;
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

        // GET PER GLI STATUS per il menù a tendina
        private List<SelectListItem> GetStatiConsegnaList()
        {
            var statiConsegnaList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Registrato", Text = "Registrato" },
                new SelectListItem { Value = "Pronto", Text = "Pronto per la spedizione" },
                new SelectListItem { Value = "Spedito", Text = "Spedito dal magazzino" },
                new SelectListItem { Value = "Transito", Text = "In transito" },
                new SelectListItem { Value = "Dogana", Text = "Arrivato alla dogana" },
                new SelectListItem { Value = "RicevutoLocale", Text = "Ricevuto dalla società di consegna locale" },
                new SelectListItem { Value = "InConsegna", Text = "In consegna" },
                new SelectListItem { Value = "Consegnato", Text = "Pacco consegnato" }
            };
            return statiConsegnaList;
        }

        // GET: Seleziona i dati nel DB per visualizzare tramite ID
        [Authorize(Roles = "admin, employee")]
        [HttpGet]
        public ActionResult EditSpedizione(int id)
        {
            Spedizione spedizione = null;
            string connectionString = ConfigurationManager.ConnectionStrings["W06L01"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                string query = "SELECT * FROM Spedizione WHERE IdSpedizione = @IdSpedizione";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdSpedizione", id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    spedizione = new Spedizione
                    {
                        IdSpedizione = Convert.ToInt32(reader["IdSpedizione"]),
                        IdCliente = Convert.ToInt32(reader["IdCliente"]),
                        DataSpedizione = (DateTime)reader["DataSpedizione"],
                        Peso = (decimal)reader["Peso"],
                        CittaDestinazione = reader["CittaDestinazione"].ToString(),
                        IndirizzoDestinazione = reader["IndirizzoDestinazione"].ToString(),
                        NominativoDestinatario = reader["NominativoDestinatario"].ToString(),
                        SpeseSpedizione = (decimal)reader["SpeseSpedizione"],
                        DataConsegnaPrevista = (DateTime)reader["DataConsegnaPrevista"],
                        Status = reader["Status"].ToString()
                    };
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Errore nella richiesta SQL: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

            ViewData["StatusList"] = GetStatiConsegnaList();

            return View(spedizione);
        }

        [HttpPost]
        public ActionResult EditSpedizione(Spedizione spedizione)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["W06L01"].ConnectionString;
                SqlConnection conn = new SqlConnection(connectionString);

                try
                {
                    conn.Open();

                    string sqlQuery = "UPDATE Spedizione SET Status = @Status WHERE IdSpedizione = @IdSpedizione";
                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                    cmd.Parameters.AddWithValue("@IdSpedizione", spedizione.IdSpedizione);
                    cmd.Parameters.AddWithValue("@Status", spedizione.Status);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Errore nella richiesta SQL: " + ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }

                return RedirectToAction("Index", "Home");
            }

            return View(spedizione);
        }


    }
}