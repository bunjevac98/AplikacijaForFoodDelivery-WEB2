using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class PorudzbineProcessor
    {

        public static int DodajPorudzbinu(int idPorudzbine, List<int>kolicina, string adresa, string komentar, int cenaDostave, int ukupnaCena, StatusPorudzbineData statusPorudzbineData, DateTime datumIsporuke) {
            //verovatno cu unostiti kao string kolicinu
            string kolicinaString="";

            foreach (var item in kolicina)
            {
                kolicinaString = kolicinaString + item.ToString()+",";
            }

            PorudzbinaModel data = new PorudzbinaModel
            {
                IdPorudzbine = idPorudzbine,
                Kolicina = kolicinaString,
                Adresa = adresa,
                Komentar = komentar,
                CenaDostave = cenaDostave,
                UkupnaCena = ukupnaCena,
                StatusPorudzbine = statusPorudzbineData,
                DatumIsporuke = datumIsporuke
            };

            string sql = "";
            sql = @"insert into dbo.Porudzbine (IdPorudzbine, Kolicina, Adresa, Komentar, CenaDostave ,UkupnaCena, StatusPorudzbine, DatumIsporuke)
              values (@IdPorudzbine, @Kolicina, @Adresa, @Komentar, @CenaDostave, @UkupnaCena, @StatusPorudzbine, @DatumIsporuke)";



            return SqlDataAccess.SacuvajPodatak(sql, data);

        }

        public static List<PorudzbinaModel> UcitajPorudzbinu() {
            string sql = "SELECT IdPorudzbine, Kolicina, Adresa, Komentar, CenaDostave ,UkupnaCena, StatusPorudzbine, DatumIsporuke from dbo.Porudzbine";
            
            return SqlDataAccess.UcitajPodatak<PorudzbinaModel>(sql);
        }

        public static int IzmeniStatusPorudzbine(PorudzbinaModel porudzbina)
        {

            string sql = "";
            sql = "UPDATE dbo.Porudzbine SET StatusPorudzbine= '" + porudzbina.StatusPorudzbine + "' where IdPorudzbine= '" + porudzbina.IdPorudzbine + "';";
            return SqlDataAccess.IzmeniPodatak(sql, porudzbina);

        }
        public static int IzmeniDatumIsporuke(PorudzbinaModel porudzbina) {

            string sql = "";
            sql = "UPDATE dbo.Porudzbine SET DatumIsporuke= '" + porudzbina.DatumIsporuke + "' where IdPorudzbine= '" + porudzbina.IdPorudzbine + "';";
            return SqlDataAccess.IzmeniPodatak(sql, porudzbina);

        }

        public static int DodajPorudzbinuProizvod(int IdPorudzbine2, int IdProizvod2) {

            //string data = "";
            
            PorProModel data = new PorProModel
            {
                IdPorudzbine = IdPorudzbine2,
                IdProizvoda = IdProizvod2
            };
            string sql = "Insert into dbo.PorudzbinaProizvod (IdPorudzbine, IdProizvoda)" +
                "values (@IdPorudzbine, @IdProizvoda)";
            
            return SqlDataAccess.SacuvajPodatak(sql,data);
        }

        public static List<PorProModel> UcitajPorudzbineProizvod() {
            string sql = "SELECT IdPorudzbine, IdProizvoda from dbo.PorudzbinaProizvod";

            return SqlDataAccess.UcitajPodatak<PorProModel>(sql);

        }




    }
}
