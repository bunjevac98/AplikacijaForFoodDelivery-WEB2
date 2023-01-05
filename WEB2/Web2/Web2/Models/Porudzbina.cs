using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web2.Models
{
    public class Porudzbina
    {
        public int IdPorudzbine { get; set; }
        public List<Proizvod> proizvodi { get; set; }
        public List<int> Kolicina { get; set; }
        public string Adresa { get; set; }
        public string Komentar { get; set; }
        public int CenaDostave { get; set; }
        public int UkupnaCena { get; set; }
        public StatusPorudzbine StatusPorudzbine { get; set; }
        public DateTime DatumIsporuke { get; set; }

        public Porudzbina()
        {
        }

        public Porudzbina(int idPorudzbine, List<Proizvod> proizvodi, List<int> kolicina, string adresa, string komentar, int cenaDostave, int ukupnaCena, StatusPorudzbine statusPorudzbine, DateTime datumIsporuke)
        {
            IdPorudzbine = idPorudzbine;
            this.proizvodi = proizvodi;
            Kolicina = kolicina;
            Adresa = adresa;
            Komentar = komentar;
            CenaDostave = cenaDostave;
            UkupnaCena = ukupnaCena;
            StatusPorudzbine = statusPorudzbine;
            DatumIsporuke = datumIsporuke;
        }
    }
}