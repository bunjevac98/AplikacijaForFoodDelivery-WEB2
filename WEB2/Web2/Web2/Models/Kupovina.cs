using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web2.Models
{
    public class Kupovina
    {

        public Proizvod ProizvodKupovine { get; set; }
        public int KolicinaKupovine { get; set; }

        public Kupovina(Proizvod proizvodKupovine, int kolicinaKupovine)
        {
            ProizvodKupovine = proizvodKupovine;
            KolicinaKupovine = kolicinaKupovine;
        }

        public Kupovina()
        {
        }


    }
}