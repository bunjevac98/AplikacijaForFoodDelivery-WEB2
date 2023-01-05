using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class PorudzbinaModel
    {
        public int IdPorudzbine { get; set; }
        public List<ProizvodData> proizvodi { get; set; }
        public string Kolicina { get; set; }
        public string Adresa { get; set; }
        public string Komentar { get; set; }
        public int CenaDostave { get; set; }
        public int UkupnaCena { get; set; }
        public StatusPorudzbineData StatusPorudzbine { get; set; }
        public DateTime DatumIsporuke { get; set; }


    }
}
