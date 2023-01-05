using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class KorisnikProcessor
    {
        //VRV DODAJE U BAZU
        public static int DodajKorisnika(string korisnickoIme, string lozinka, string ime, string Prezime, string adresa, string email, DateTime datumRodjenja, TipKorisnikaData tipKorisnika, string slika, StatusData status) {

            KorisnickiModel data = new KorisnickiModel
            {

                KorisnickoIme = korisnickoIme,
                Lozinka = lozinka,
                Ime = ime,
                Prezime = Prezime,
                Adresa = adresa,
                Email = email,
                DatumRodjenja = datumRodjenja,
                TipKorisnika = tipKorisnika,
                Slika = slika,
                Status = status
            };

            string sql = "";
            if (data.TipKorisnika == TipKorisnikaData.DOSTAVLJAC) {
                sql = @"insert into dbo.Dostavljaci (KorisnickoIme,Lozinka ,Ime ,Prezime ,Adresa ,Email ,DatumRodjenja ,TipKorisnika ,Slika ,Status )
                   values (@KorisnickoIme, @Lozinka , @Ime , @Prezime , @Adresa , @Email , @DatumRodjenja , @TipKorisnika , @Slika , @Status)";
            }
            if (data.TipKorisnika == TipKorisnikaData.POTROSAC)
            {
                sql = @"insert into dbo.Potrosaci (KorisnickoIme,Lozinka ,Ime ,Prezime ,Adresa ,Email ,DatumRodjenja ,TipKorisnika ,Slika ,Status) values ( @KorisnickoIme, @Lozinka , @Ime , @Prezime , @Adresa , @Email , @DatumRodjenja , @TipKorisnika , @Slika , @Status)";
            }

            return SqlDataAccess.SacuvajPodatak(sql, data);

        }

        public static int IzmeniStatusDostavljaca(KorisnickiModel korisnik) {

            string sql = "";
            sql = "UPDATE dbo.Dostavljaci SET Status= '" + korisnik.Status + "' where KorisnickoIme= '" + korisnik.KorisnickoIme + "';";

            return SqlDataAccess.IzmeniPodatak(sql, korisnik);

        }

        public static int IzmeniPodatkeKorisnika(KorisnickiModel korisnik)
        {
            string sql = "";
            if (korisnik.TipKorisnika == TipKorisnikaData.ADMINISTRATOR) {
                sql = "UPDATE dbo.Administratori SET Lozinka='" + korisnik.Lozinka + " ' ,Ime ='" + korisnik.Ime + "',Prezime='" + korisnik.Prezime + "',Adresa='" +       korisnik.Adresa + "',Email='" + korisnik.Email + "',DatumRodjenja='"+ korisnik.DatumRodjenja + "',TipKorisnika='" + korisnik.TipKorisnika+"',Slika='" + korisnik.Slika  + "',Status='" + korisnik.Status + "' where KorisnickoIme= '" + korisnik.KorisnickoIme + "';";
            }
            if (korisnik.TipKorisnika == TipKorisnikaData.DOSTAVLJAC) {
                sql = "UPDATE dbo.Dostavljaci SET Lozinka='" + korisnik.Lozinka + " ' ,Ime ='" + korisnik.Ime + "',Prezime='" + korisnik.Prezime + "',Adresa='" + korisnik.Adresa + "',Email='" + korisnik.Email + "',DatumRodjenja='" + korisnik.DatumRodjenja + "',TipKorisnika='" + korisnik.TipKorisnika + "',Slika='" + korisnik.Slika + "',Status='" + korisnik.Status + "' where KorisnickoIme= '" + korisnik.KorisnickoIme + "';";
            }
            if (korisnik.TipKorisnika == TipKorisnikaData.POTROSAC) {
                sql = "UPDATE dbo.Potrosaci SET Lozinka='" + korisnik.Lozinka + " ' ,Ime ='" + korisnik.Ime + "',Prezime='" + korisnik.Prezime + "',Adresa='" + korisnik.Adresa + "',Email='" + korisnik.Email + "',DatumRodjenja='" + korisnik.DatumRodjenja + "',TipKorisnika='" + korisnik.TipKorisnika + "',Slika='" + korisnik.Slika + "',Status='" + korisnik.Status + "' where KorisnickoIme= '" + korisnik.KorisnickoIme + "';";
            }

            return SqlDataAccess.IzmeniPodatak(sql, korisnik);

        }


        public static int DodajPotrosacaPorudzbinu(int IdPorudzbine, string KorisnickoIme) {

            PorPotrosacModel data = new PorPotrosacModel
            {
                KorisnickoIme = KorisnickoIme,
                IdPorudzbine = IdPorudzbine
            };

            string sql = "Insert into dbo.PotrosacPorudzbina (KorisnickoIme, IdPorudzbine)" +
                "values (@KorisnickoIme, @IdPorudzbine)";


            return SqlDataAccess.SacuvajPodatak(sql, data);
        }
        public static List<PorPotrosacModel> UcitajPotrosacePorudzbine() {
            string sql = "select KorisnickoIme, IdPorudzbine from dbo.PotrosacPorudzbina";

            return SqlDataAccess.UcitajPodatak<PorPotrosacModel>(sql);
        }


        public static int DodajDostavljacaPorudzbinu(int IdPorudzbine, string KorisnickoIme)
        {

            PorDosModel data = new PorDosModel
            {
                KorisnickoIme = KorisnickoIme,
                IdPorudzbine = IdPorudzbine
            };

            string sql = "Insert into dbo.DostavljacPorudzbina (KorisnickoIme, IdPorudzbine)" +
                "values (@KorisnickoIme, @IdPorudzbine)";


            return SqlDataAccess.SacuvajPodatak(sql, data);
        }
        public static List<PorDosModel> UcitajDostavljacaPorudzbine()
        {
            string sql = "select KorisnickoIme, IdPorudzbine from dbo.DostavljacPorudzbina";

            return SqlDataAccess.UcitajPodatak<PorDosModel>(sql);
        }

        
        public static List<KorisnickiModel> UcitajPotrosaca() {
            string sql = "select KorisnickoIme,Lozinka ,Ime ,Prezime ,Adresa ,Email ,DatumRodjenja ,TipKorisnika ,Slika ,Status" +
                        " from dbo.Potrosaci";

            return SqlDataAccess.UcitajPodatak<KorisnickiModel>(sql);
        }
        public static List<KorisnickiModel> UcitajDostavljaca()
        {
            string sql = "select KorisnickoIme,Lozinka ,Ime ,Prezime ,Adresa ,Email ,DatumRodjenja ,TipKorisnika ,Slika ,Status" +
                        " from dbo.Dostavljaci";

            return SqlDataAccess.UcitajPodatak<KorisnickiModel>(sql);
        }
        public static List<KorisnickiModel> UcitajAdministratora() {
            string sql = "select KorisnickoIme, Lozinka, Ime, Prezime, Adresa, Email, DatumRodjenja, TipKorisnika, Slika, Status" +
                        " from dbo.Administratori";

            return SqlDataAccess.UcitajPodatak<KorisnickiModel>(sql);
        }




    }
}
