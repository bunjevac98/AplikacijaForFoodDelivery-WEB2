using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web2.Models
{
    public class Korisnik
    {

        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Adresa { get; set; }
        public string Email { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public TipKorisnika TipKorisnika { get; set; }
        public string Slika{ get; set; }
        public string DrugaLozinka { get; set; }
        public Status Status { get; set; }
        public List<Porudzbina> Porudzbine { get; set; }

        /// <summary>
        /// /
        /// </summary>
        public bool Preuzeo { get; set; }

        public Korisnik()
        {
        }

        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, string adresa, string email, DateTime datumRodjenja, TipKorisnika tipKorisnika, string slika, Status status, List<Porudzbina> porudzbine, bool preuzeo)
        {
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            Ime = ime;
            Prezime = prezime;
            Adresa = adresa;
            Email = email;
            DatumRodjenja = datumRodjenja;
            TipKorisnika = tipKorisnika;
            Slika = slika;
            Status = status;
            Porudzbine = porudzbine;
            Preuzeo = preuzeo;
        }
        
    }
}