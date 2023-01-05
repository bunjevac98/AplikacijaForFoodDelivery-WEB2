using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web2.Models;
using static DataLibrary.BusinessLogic.ProizvodiProcessor;
using static DataLibrary.BusinessLogic.KorisnikProcessor;
using static DataLibrary.BusinessLogic.PorudzbineProcessor;
using DataLibrary.Models;
using DataLibrary.BusinessLogic;

namespace Web2.Controllers
{
    public class PotrosacController : Controller
    {

        public static List<Kupovina> kupovine = new List<Kupovina>();
        public static Proizvod ProizvodKupovine = new Proizvod();
        public static List<Porudzbina> listaPorudzbina = new List<Porudzbina>();
        public static int minValue = 0000;
        public static int maxValue = 9999;
        // GET: Potrosac
        public ActionResult Index()
        {
            return View();
        }
        public static int IdPorudzbine = 0;

        public ActionResult Porucivanje()
        {
            /*
            List<Proizvod> proizvodi2 = (List<Proizvod>)Session["proizvodi2"];
            if (proizvodi2 != null)
            {
                ViewBag.proizvodi = proizvodi2;
            }
            else {
            List<Proizvod> proizvodi = (List<Proizvod>)HttpContext.Application["proizvodi"];
                if (proizvodi != null)
                {
                    ViewBag.proizvodi = proizvodi;
                }
            }*/
            #region CITANJE PROIZVODA
            var proizvodiIzBaze = UcitajProizvod();
            List<Proizvod> proizvodi = new List<Proizvod>();
            foreach (var item in proizvodiIzBaze)
            {
                proizvodi.Add(new Proizvod
                {
                    ImeProizvoda = item.ImeProizvoda,
                    IdProizvoda = item.IdProizvoda,
                    Sastojci=item.Sastojci,
                    Cena=item.Cena
                });
            }
            #endregion
            ViewBag.proizvodi = proizvodi;


            return View("~/Views/Potrosac/Porucivanje.cshtml");

        }

        public ActionResult PorucivanjeProizvoda(int idProizvoda)
        {
            //List<Proizvod> proizvodi = (List<Proizvod>)HttpContext.Application["proizvodi"];
            Proizvod p = new Proizvod();
            #region CITANJE PROIZVODA
            var proizvodiIzBaze = UcitajProizvod();
            List<Proizvod> proizvodi = new List<Proizvod>();
            foreach (var item in proizvodiIzBaze)
            {
                proizvodi.Add(new Proizvod
                {
                    ImeProizvoda = item.ImeProizvoda,
                    IdProizvoda = item.IdProizvoda,
                    Sastojci = item.Sastojci,
                    Cena = item.Cena
                });
            }
            #endregion
            
            foreach (var proizvod in proizvodi)
            {
                if (idProizvoda== proizvod.IdProizvoda) {
                    ViewBag.proizvod = proizvod;
                    ProizvodKupovine = proizvod;
                    return View("~/Views/Potrosac/KolicinaProizvoda.cshtml");
                }
            }



            return View("~/Views/Potrosac/NovaPorudzbina.cshtml");
        }

        [HttpPost]
        public ActionResult PorucivanjeProizvoda()
        {
            int ukupnaCena = 0;
            foreach (var item in kupovine)
            {
                ukupnaCena = ukupnaCena + item.ProizvodKupovine.Cena * item.KolicinaKupovine;
            }
            ukupnaCena = ukupnaCena + 50;
            ViewBag.kupovina = kupovine;
            ViewBag.UkupnaCena = ukupnaCena;
            ///racunam kolko ce izaci sve ukupno sa dostavom
            //int skacem da pravim novu porudzbinu
            //tek kad napravim pordudzbinu i sacuvam je u fajl anuliram sve ove liste jebene
            //kupovine = null;



            //vo jos napraviti
            return View("~/Views/Potrosac/NovaPorudzbina.cshtml");
        }

        public ActionResult SacuvajKupovinu(int kolicinaKupovine)
        {
            //List<Proizvod> proizvodi = (List<Proizvod>)HttpContext.Application["proizvodi"];
            #region CITANJE PROIZVODA
            var proizvodiIzBaze = UcitajProizvod();
            List<Proizvod> proizvodi = new List<Proizvod>();
            foreach (var item in proizvodiIzBaze)
            {
                proizvodi.Add(new Proizvod
                {
                    ImeProizvoda = item.ImeProizvoda,
                    IdProizvoda = item.IdProizvoda,
                    Sastojci = item.Sastojci,
                    Cena = item.Cena
                });
            }
            #endregion



            ViewBag.proizvodi = proizvodi;
            Kupovina kupovina = new Kupovina();
            kupovina.KolicinaKupovine = kolicinaKupovine;
            kupovina.ProizvodKupovine = ProizvodKupovine;
            kupovine.Add(kupovina);

            //vo jos napraviti
            return View("~/Views/Potrosac/Porucivanje.cshtml");
        }

        public ActionResult ZavrsenaPorudzbina(Porudzbina p) {
            // List<Korisnik> potrosaci = (List<Korisnik>)HttpContext.Application["potrosaci"];

            #region UCITAJ POTROSACE
            var potrosaciIzBaze = UcitajPotrosaca();
            List<Korisnik> potrosaci = new List<Korisnik>();
            if (potrosaciIzBaze != null)
            {
                foreach (var row in potrosaciIzBaze)
                {
                    potrosaci.Add(new Korisnik
                    {
                        KorisnickoIme = row.KorisnickoIme,
                        Lozinka = row.Lozinka,
                        Ime = row.Ime,
                        Prezime = row.Prezime,
                        Adresa = row.Adresa,
                        Email = row.Email,
                        DatumRodjenja = row.DatumRodjenja,
                        TipKorisnika = (TipKorisnika)row.TipKorisnika,
                        Slika = row.Slika,
                        Status = (Status)row.Status
                    });
                }
            }
            #endregion


            //treba dodati potrosacu u njegovu listu porudzbina id porudzbine
            Korisnik potrosac = (Korisnik)Session["potrosac"];

            Porudzbina porudzbina = new Porudzbina();
            List<Proizvod> proizvodiIzKupovine = new List<Proizvod>();
            List<int> listaKolicine = new List<int>();
            int ukupnaCena = 0;
            int ukupnaKolicina = 0;
            foreach (var item in kupovine)
            {
                proizvodiIzKupovine.Add(item.ProizvodKupovine);
                ukupnaCena = ukupnaCena + item.ProizvodKupovine.Cena * item.KolicinaKupovine;
                ukupnaKolicina = ukupnaKolicina + item.KolicinaKupovine;
                listaKolicine.Add(item.KolicinaKupovine);
            }
            Random random = new Random();
            porudzbina.IdPorudzbine= random.Next(minValue, maxValue);
            ukupnaCena = ukupnaCena + 50;
            porudzbina.Adresa = p.Adresa;
            porudzbina.Komentar = p.Komentar;
            porudzbina.proizvodi = proizvodiIzKupovine;
            porudzbina.UkupnaCena = ukupnaCena;
            porudzbina.StatusPorudzbine = StatusPorudzbine.NEPOTVRDJENA;
            porudzbina.CenaDostave = 50;
            porudzbina.Kolicina = listaKolicine;
            porudzbina.DatumIsporuke = DateTime.Now;

            listaPorudzbina.Add(porudzbina);


            //Podaci.CuvanjePorudzbine(porudzbina);
            DodajPorudzbinu(porudzbina.IdPorudzbine, porudzbina.Kolicina, porudzbina.Adresa, porudzbina.Komentar, porudzbina.CenaDostave, porudzbina.UkupnaCena, (StatusPorudzbineData)porudzbina.StatusPorudzbine, porudzbina.DatumIsporuke);
            foreach (var item in proizvodiIzKupovine)
            {
                DodajPorudzbinuProizvod(porudzbina.IdPorudzbine, item.IdProizvoda);
            }



            Session["porudzbine"] = listaPorudzbina;
            //potrosac.Porudzbine = listaPorudzbina;

            //POVEZATI POTROSAC I PORUDZBINU
            DodajPotrosacaPorudzbinu(porudzbina.IdPorudzbine, potrosac.KorisnickoIme);
            //potrosac.Porudzbine.Add(porudzbina);

            //VEROVATNO NECE BITI POTREBNO STA UPDEJTOVATI
            //Podaci.UpdejtovanjFajla("C:/Users/basic/Desktop/WEB2/Web2/Web2/App_Data/Potrosaci.txt", potrosac);

            //ovo dodali
            kupovine = new List<Kupovina>();

            return View("~/Views/Home/Dashboard.cshtml");
        }










    }
}