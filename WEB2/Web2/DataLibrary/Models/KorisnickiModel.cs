using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class KorisnickiModel
    {
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Adresa { get; set; }
        public string Email { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public TipKorisnikaData TipKorisnika { get; set; }
        public string Slika { get; set; }
        public string DrugaLozinka { get; set; }
        public StatusData Status { get; set; }
        public List<PorudzbinaModel> Porudzbine { get; set; }
         


    }
}
