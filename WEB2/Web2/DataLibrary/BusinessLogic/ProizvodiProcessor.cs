using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class ProizvodiProcessor
    {
        public static int DodajProizvod(string imeProizvoda, int idProizvoda, int cena, string sastojci) {

            ProizvodData data = new ProizvodData
            {
                ImeProizvoda = imeProizvoda,
                IdProizvoda= idProizvoda,
                Cena=cena,
                Sastojci=sastojci
            };
            string sql = "";
            sql = @"insert into dbo.Proizvodi(ImeProizvoda, IdProizvoda, Cena, Sastojci)
                   values (@ImeProizvoda, @IdProizvoda, @Cena, @Sastojci)";

            return SqlDataAccess.SacuvajPodatak(sql, data);
        }

        public static List<ProizvodData> UcitajProizvod() {
            string sql = "SELECT ImeProizvoda, IdProizvoda, Cena, Sastojci from dbo.Proizvodi";

            return SqlDataAccess.UcitajPodatak<ProizvodData>(sql);
        }







    }





}
