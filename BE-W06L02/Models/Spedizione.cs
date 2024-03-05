using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_W06L02.Models
{
    public class Spedizione
    {
        public int IdSpedizione { get; set; }

        [Required(ErrorMessage = "Il campo Cliente è obbligatorio.")]
        [Display(Name = "Cliente")]
        public int IdCliente { get; set; }
        public List<SelectListItem> ClientiItems { get; set; } // Lista delle opzioni per i clienti registrati

        [Display(Name = "Data Spedizione")]
        [DataType(DataType.Date)]
        public DateTime? DataSpedizione { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Il Peso deve essere maggiore di zero.")]
        public decimal Peso { get; set; }

        [Required(ErrorMessage = "Il campo Città Destinazione è obbligatorio.")]
        [Display(Name = "Città Destinazione")]
        public string CittaDestinazione { get; set; }

        [Required(ErrorMessage = "Il campo Indirizzo Destinazione è obbligatorio.")]
        [Display(Name = "Indirizzo Destinazione")]
        public string IndirizzoDestinazione { get; set; }

        [Required(ErrorMessage = "Il campo Nominativo Destinatario è obbligatorio.")]
        [Display(Name = "Nominativo Destinatario")]
        public string NominativoDestinatario { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Le Spese di Spedizione devono essere maggiori di zero.")]
        [Display(Name = "Spese di Spedizione")]
        public decimal SpeseSpedizione { get; set; }

        [Display(Name = "Data Consegna Prevista")]
        [DataType(DataType.Date)]
        public DateTime? DataConsegnaPrevista { get; set; }

        [Display(Name ="Stato della consegna")]
        [Required(ErrorMessage = "La spedizione deve avere uno stato.")]
        public string Status {  get; set; }
        public List<SelectListItem> StatusItems { get; set; } // Lista delle opzioni per i clienti registrati


        public Spedizione()
        {
            ClientiItems = new List<SelectListItem>();
            StatusItems = new List<SelectListItem>();
        }
        public Spedizione(int idSpedizione, int idCliente, DateTime? dataSpedizione, decimal peso, string cittaDestinazione, string indirizzoDestinazione, string nominativoDestinatario, decimal speseSpedizione, DateTime? dataConsegnaPrevista)
        {
            IdSpedizione = idSpedizione;
            IdCliente = idCliente;
            DataSpedizione = dataSpedizione;
            Peso = peso;
            CittaDestinazione = cittaDestinazione;
            IndirizzoDestinazione = indirizzoDestinazione;
            NominativoDestinatario = nominativoDestinatario;
            SpeseSpedizione = speseSpedizione;
            DataConsegnaPrevista = dataConsegnaPrevista;

            // Inizializza le liste delle opzioni
            ClientiItems = new List<SelectListItem>();
            StatusItems = new List<SelectListItem>();
        }
    }
}