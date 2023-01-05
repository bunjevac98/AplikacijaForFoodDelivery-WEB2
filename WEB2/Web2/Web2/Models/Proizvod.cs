using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web2.Models
{
    public class Proizvod
    {

        public string ImeProizvoda { get; set; }
        public int IdProizvoda { get; set; }
        public int Cena { get; set; }
        public string Sastojci { get; set; }

        public Proizvod(string imeProizvoda, int idProizvoda, int cena, string sastojci)
        {
            ImeProizvoda = imeProizvoda;
            IdProizvoda = idProizvoda;
            Cena = cena;
            Sastojci = sastojci;
        }

        public Proizvod()
        {
        }



    }
}