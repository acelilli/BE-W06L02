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
    public class HomeController : Controller
    {
        [Authorize(Roles = "admin, employee")]
        public ActionResult Index()
        {
            DateTime today = DateTime.Today;
            List<Spedizione> spedizioniInConsegnaOggi = GetSpedizioniInConsegna(today);
            int totalDeliveries = GetTotalPendingDeliveries();
            Dictionary<string, int> deliveriesByCity = GetDeliveriesByCity();

            ViewBag.SpedizioniInConsegna = spedizioniInConsegnaOggi;
            ViewBag.TotalDeliveries = totalDeliveries;
            ViewBag.DeliveriesByCity = deliveriesByCity;

            return View();
        }
        public ActionResult DeliveryToday()
        {
            DateTime today = DateTime.Today;
            List<Spedizione> spedizioniInConsegnaOggi = GetSpedizioniInConsegna(today);
            return View(spedizioniInConsegnaOggi);
        }

        private List<Spedizione> GetSpedizioniInConsegna(DateTime today)
        {
            List<Spedizione> spedizioni = new List<Spedizione>();
            string connectionString = ConfigurationManager.ConnectionStrings["W06L01"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Spedizione WHERE DataConsegnaPrevista = @Today AND Status = 'InConsegna'";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Today", today);

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Spedizione spedizione = new Spedizione
                    {
                        IdSpedizione = Convert.ToInt32(reader["IdSpedizione"]),
                        // Popolare altre proprietà necessarie
                    };
                    spedizioni.Add(spedizione);
                }
            }

            return spedizioni;
        }

        [Authorize(Roles = "admin, employee")]
        public ActionResult TotalPendingDeliveries()
        {
            int totalDeliveries = GetTotalPendingDeliveries();
            return Content($"Il numero totale di spedizioni in attesa di consegna è: {totalDeliveries}");
        }

        private int GetTotalPendingDeliveries()
        {
            int totalDeliveries = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["W06L01"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Spedizione WHERE Status = 'InConsegna'";
                SqlCommand command = new SqlCommand(query, conn);

                conn.Open();
                totalDeliveries = (int)command.ExecuteScalar();
            }

            return totalDeliveries;
        }

        [Authorize(Roles = "admin, employee")]
        public ActionResult DeliveriesForCities()
        {
            Dictionary<string, int> deliveriesByCity = GetDeliveriesByCity();
            return View(deliveriesByCity);
        }

        private Dictionary<string, int> GetDeliveriesByCity()
        {
            Dictionary<string, int> deliveriesByCity = new Dictionary<string, int>();
            string connectionString = ConfigurationManager.ConnectionStrings["W06L01"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CittaDestinazione, COUNT(*) AS Total FROM Spedizione GROUP BY CittaDestinazione";
                SqlCommand command = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string city = reader["CittaDestinazione"].ToString();
                    int total = Convert.ToInt32(reader["Total"]);
                    deliveriesByCity.Add(city, total);
                }
            }

            return deliveriesByCity;
        }
    }
}