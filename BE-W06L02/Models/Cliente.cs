using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_W06L02.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }

        [Display(Name = "Azienda?")]
        public bool AziendaPrivato { get; set; }

        [Required(ErrorMessage = "Il campo Nome è obbligatorio.")]
        public string Nome { get; set; }

        [Display(Name = "Cognome")]
        public string Cognome { get; set; }

        [Display(Name = "Codice Fiscale")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Il Codice Fiscale deve essere lungo 16 caratteri.")]
        /* [Remote("IsValueUnique", "Cliente", ErrorMessage = "Il Codice Fiscale inserito è già presente.")] */
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Il Codice Fiscale può contenere solo lettere e numeri.")]
        public string CodiceFiscale { get; set; }

        [Display(Name = "Partita Iva")]
        [StringLength(16, MinimumLength = 11, ErrorMessage = "La Partita IVA deve essere lunga tra 11 e 16 caratteri.")]
        /* [Remote("IsValueUnique", "Cliente", ErrorMessage = "La Partita IVA inserita è già presente.")] */
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "La Partita IVA può contenere solo numeri.")]
        public string PartitaIva { get; set; }

        [Required(ErrorMessage = "Il campo Indirizzo è obbligatorio.")]
        public string Indirizzo { get; set; }

        [Display(Name = "Città")]
        [Required(ErrorMessage = "Il campo Città è obbligatorio.")]
        public string Citta { get; set; }

        public Cliente() { }
        public Cliente(int idCliente, bool aziendaPrivato, string nome, string cognome, string codiceFiscale, string partitaIva, string indirizzo, string citta)
        {
            IdCliente = idCliente;
            AziendaPrivato = aziendaPrivato;
            Nome = nome;
            Cognome = cognome;
            CodiceFiscale = codiceFiscale;
            PartitaIva = partitaIva;
            Indirizzo = indirizzo;
            Citta = citta;
        }
    }
}