using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web2.Models;
using DataLibrary;
using static DataLibrary.BusinessLogic.KorisnikProcessor;
using static DataLibrary.BusinessLogic.PorudzbineProcessor;
using static DataLibrary.BusinessLogic.ProizvodiProcessor;
using DataLibrary.Models;
using DataLibrary.BusinessLogic;

namespace Web2.Controllers
{
    public class AutentifikacijaController : Controller
    {
        // GET: Autentifikacija
        public ActionResult Index()
        {
            return View("~/Views/Home/Prijava.cshtml");
        }
        
        [HttpPost]
        public ActionResult Prijavljivanje(string KorisnickoIme, string lozinka)
        {

            #region CITANJE TABELA ZA SJEDINJAVANJE
            var porudzbineProizvodi = UcitajPorudzbineProizvod();
            var porudzbineDostavljaci = UcitajDostavljacaPorudzbine();
            var porudzbinePotrosaci = UcitajPotrosacePorudzbine();
            List<PorProModel> porudzbineProizvodiLista = new List<PorProModel>();
            List<PorDosModel> porudzbineDostavljaciLista = new List<PorDosModel>();
            List<PorPotrosacModel> porudzbinePotrosaciLista = new List<PorPotrosacModel>();
            foreach (var item in porudzbineProizvodi)
            {
                porudzbineProizvodiLista.Add(item);
            }
            foreach (var item2 in porudzbineDostavljaci)
            {
                porudzbineDostavljaciLista.Add(item2);
            }
            foreach (var item3 in porudzbinePotrosaci)
            {
                porudzbinePotrosaciLista.Add(item3);
            }

            #endregion

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

            #region CITANJE PORUDZBINA
            var porudzbineIzBaze = UcitajPorudzbinu();

            List<Porudzbina> porudzbine = new List<Porudzbina>();
            if (porudzbineIzBaze != null)
            {
                foreach (var row in porudzbineIzBaze)
                {
                    Porudzbina porudzbina2 = new Porudzbina();
                    List<int> kolicinaProizvoda = new List<int>();
                    string[] tokens = row.Kolicina.Split(',');
                    foreach (var item in tokens)
                    {
                        if (item != "")
                        {
                            kolicinaProizvoda.Add(Int32.Parse(item));
                        }
                    }
                    porudzbina2.IdPorudzbine = row.IdPorudzbine;
                    porudzbina2.Kolicina = kolicinaProizvoda;
                    porudzbina2.Adresa = row.Adresa;
                    porudzbina2.Komentar = row.Komentar;
                    porudzbina2.CenaDostave = row.CenaDostave;
                    porudzbina2.UkupnaCena = row.UkupnaCena;
                    porudzbina2.StatusPorudzbine = (StatusPorudzbine)row.StatusPorudzbine;
                    porudzbina2.DatumIsporuke = row.DatumIsporuke;

                    List<Proizvod> proizvodiZaPorudzbinu = new List<Proizvod>();
                    foreach (var item2 in porudzbineProizvodiLista)
                    {
                        if (item2.IdPorudzbine == row.IdPorudzbine)
                        {
                            foreach (var proizvod in proizvodi)
                            {
                                if (proizvod.IdProizvoda == item2.IdProizvoda)
                                {
                                    proizvodiZaPorudzbinu.Add(proizvod);
                                }
                            }
                        }
                    }
                    porudzbina2.proizvodi = proizvodiZaPorudzbinu; ;
                    porudzbine.Add(porudzbina2);
                }

            }

            #endregion
            

            #region CITANJE IZ BAZE
            var administratoriIzBaze = UcitajAdministratora();
            var dostavljaciIzBaze = UcitajDostavljaca();
            var potrosaciIzBaze = UcitajPotrosaca();

            List<Korisnik> administratori = new List<Korisnik>();
            List<Korisnik> dostavljaci = new List<Korisnik>();
            List<Korisnik> potrosaci = new List<Korisnik>();
            if (administratoriIzBaze != null)
            {
                foreach (var row in administratoriIzBaze)
                {
                    administratori.Add(new Korisnik
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
            if (dostavljaciIzBaze != null)
            {
                foreach (var row in dostavljaciIzBaze)
                {
                    List<Porudzbina> porudzbinaZaDostavljaca = new List<Porudzbina>();
                    foreach (var item2 in porudzbineDostavljaciLista)
                    {
                        if (item2.KorisnickoIme == row.KorisnickoIme)
                        {
                            foreach (var por in porudzbine)
                            {
                                if (por.IdPorudzbine == item2.IdPorudzbine)
                                {
                                    porudzbinaZaDostavljaca.Add(por);
                                }
                            }
                        }
                    }
                    dostavljaci.Add(new Korisnik
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
                        Status = (Status)row.Status,
                        Porudzbine = porudzbinaZaDostavljaca

                    });
                }
            }
            if (potrosaciIzBaze != null)
            {
                foreach (var row in potrosaciIzBaze)
                {
                    List<Porudzbina> porudzbinaZaPotrosaca = new List<Porudzbina>();
                    foreach (var item2 in porudzbinePotrosaciLista)
                    {
                        if (item2.KorisnickoIme == row.KorisnickoIme)
                        {
                            foreach (var por in porudzbine)
                            {
                                if (por.IdPorudzbine == item2.IdPorudzbine)
                                {
                                    porudzbinaZaPotrosaca.Add(por);
                                }
                            }
                        }
                    }
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
                        Status = (Status)row.Status,
                        Porudzbine = porudzbinaZaPotrosaca

                    });
                }
            }
            #endregion

            if (administratori == null)
            {
                ViewBag.Message = $"Korisnik sa datim imenom ne postoji!";
                return View("~/Views/Home/Prijava.cshtml");
            }
            foreach (var adm in administratori)
            {
                if (adm.KorisnickoIme == KorisnickoIme)
                {
                    string lozinkaKorisnika = adm.Lozinka.Trim();
                    if (lozinka==lozinkaKorisnika)
                    {
                        Korisnik k = adm;                   
                        Session["administrator"] = k;
                        return View("~/Views/Home/Dashboard.cshtml");
                    }
                }
            }
            ViewBag.Message = $"Pogresno ste uneli lozinku ili korisnicko ime!";
            
            if (dostavljaci == null)
            {
                ViewBag.Message = $"Korisnik ne postoji!";
                return View("~/Views/Home/Prijava.cshtml");
            }
            foreach (var kup in dostavljaci)
            {
                if (kup.KorisnickoIme == KorisnickoIme)
                {
                    if (kup.Status == Status.NEAKTIVAN)
                    {
                        ViewBag.Message = "Vas zahtev nije odobren";
                        return View("~/Views/Home/Prijava.cshtml");
                    }
                    if (kup.Status == Status.ODBIJEN)
                    {
                        ViewBag.Message = "Vas zahtev je odbijen";
                        return View("~/Views/Home/Prijava.cshtml");
                    }
                }
                if (kup.KorisnickoIme == KorisnickoIme)
                {
                    string lozinkaKorisnika = kup.Lozinka.Trim();
                    if (lozinka == lozinkaKorisnika) { 
                        Korisnik k = kup;
                        Session["dostavljac"] = k;
                        //DASHBOARD
                        return View("~/Views/Home/Dashboard.cshtml");
                    }
                }
            }
            if (potrosaci == null) {
                ViewBag.Message = $"Korisnik ne postoji!";
                return View("~/Views/Home/Prijava.cshtml");
            }
            foreach (var kup in potrosaci)
            {
                if (kup.KorisnickoIme == KorisnickoIme)
                {
                    string lozinkaKorisnika = kup.Lozinka.Trim();
                    if (lozinka == lozinkaKorisnika) { 
                        Korisnik k = kup;
                        Session["potrosac"] = k;
                        return View("~/Views/Home/Dashboard.cshtml");
                    }
                }
            }
            ViewBag.Message = $"Pogresno ste uneli lozinku ili korisnicko ime!";
            
            return View("~/Views/Home/Prijava.cshtml");
        }


        public ActionResult Registracija()
        {

            Korisnik k = new Korisnik();
            Session["k"] = k;
            return View(k);
        }

        [HttpPost]
        public ActionResult Registracija(Korisnik korisnik)
        {

            #region CITANJE IZ BAZE
            var administratoriIzBaze = UcitajAdministratora();
            var dostavljaciIzBaze = UcitajDostavljaca();
            var potrosaciIzBaze = UcitajPotrosaca();
            
            List<Korisnik> administratori = new List<Korisnik>();
            List<Korisnik> dostavljaci = new List<Korisnik>();
            List<Korisnik> potrosaci = new List<Korisnik>();
            if (administratoriIzBaze != null) {
                foreach (var row in administratoriIzBaze)
                {
                    administratori.Add(new Korisnik
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
            if (dostavljaciIzBaze != null)
            {
                foreach (var row in dostavljaciIzBaze)
                {
                    dostavljaci.Add(new Korisnik
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

            #region VALIDACIJA

            if (korisnik.KorisnickoIme == null) {
                ViewBag.GreskaRegistrovanja = "Niste dobro uneli polje korisnicko ime, pokusajte ponovo";
                return View();
            }
            if (korisnik.Lozinka == null) {
                ViewBag.GreskaRegistrovanja = "Niste dobro uneli polje lozinka, pokusajte ponovo";
                return View();
            }
            if (korisnik.Ime == null) {
                ViewBag.GreskaRegistrovanja = "Niste dobro uneli polje Ime, pokusajte ponovo";
                return View();
            }
            if (korisnik.Prezime == null ) {
                ViewBag.GreskaRegistrovanja = "Niste dobro uneli polje prezime, pokusajte ponovo";
                return View();
            }
            if (korisnik.Adresa == null ) {
                ViewBag.GreskaRegistrovanja = "Niste dobro uneli polje adresa, pokusajte ponovo";
                return View();
            }
            if (korisnik.Email == null ) {
                ViewBag.GreskaRegistrovanja = "Niste dobro uneli polje email, pokusajte ponovo";
                return View();
            }
            if (korisnik.Slika == null ) {
                ViewBag.GreskaRegistrovanja = "Niste dobro uneli polje slika, pokusajte ponovo";
                return View();
            }
             if (korisnik.DrugaLozinka == null ) {
                ViewBag.GreskaRegistrovanja = "Niste dobro uneli polje ponovite lozinku, pokusajte ponovo";
                return View();
            }
            if (korisnik.DatumRodjenja.ToString() == "1/1/0001 12:00:00 AM")
            {
                ViewBag.GreskaRegistrovanja = "Niste dobro uneli polje prezime, pokusajte ponovo";
                return View();
            }
            #endregion


            if (korisnik.TipKorisnika == TipKorisnika.DOSTAVLJAC)
            {
                foreach (Korisnik k in dostavljaci)
                {
                    if (k.KorisnickoIme == korisnik.KorisnickoIme)
                    {
                        ViewBag.GreskaRegistrovanja = $"Korisnicko ime vec postoji";
                        return View();
                    }
                }
                if (korisnik.Lozinka.Length < 4)
                {
                    ViewBag.GreskaRegistrovanja = $"Lozinka mora imati 5 slova";
                    return View();
                }
                if (korisnik.DrugaLozinka != korisnik.Lozinka)
                {
                    ViewBag.GreskaRegistrovanja = $"Niste dobro ponovili lozinku";
                    return View();
                }
                korisnik.Status = Status.NEAKTIVAN;
                dostavljaci.Add(korisnik);
                Session["dostavljac"] = korisnik;////mozda dodati i na kraju
            }

            if (korisnik.TipKorisnika == TipKorisnika.POTROSAC)
            {
                    foreach (Korisnik k in potrosaci)
                    {
                        if (k.KorisnickoIme == korisnik.KorisnickoIme)
                        {
                            ViewBag.GreskaRegistrovanja = $"Korisnicko ime vec postoji";
                            return View();
                        }
                    }

                if (korisnik.Lozinka.Length < 4)
                {
                    ViewBag.GreskaRegistrovanja = $"Lozinka mora imati 5 slova";
                    return View();
                }
                if (korisnik.DrugaLozinka != korisnik.Lozinka)
                {
                    ViewBag.GreskaRegistrovanja = $"Niste dobro ponovili lozinku";
                    return View();
                }

                korisnik.Status = Status.AKTIVAN;
                potrosaci.Add(korisnik);
                Session["potrosac"] = korisnik;
            }

            DodajKorisnika(korisnik.KorisnickoIme, korisnik.Lozinka, korisnik.Ime, korisnik.Prezime, korisnik.Adresa, korisnik.Email, korisnik.DatumRodjenja, (TipKorisnikaData)korisnik.TipKorisnika, korisnik.Slika,
                (StatusData)korisnik.Status);
            //Ne treba mi vise
            Podaci.CuvanjeKorisnika(korisnik);

            ViewBag.Registracija = $"Uspesno ste se registrovali";
            // return RedirectToAction("Index", "Autentifikacija");
            return View();
        }


        public ActionResult Odjava()
        {
            //i za kupce i za administratore
            Session["dostavljac"] = null;
            Session["administrator"] = null;
            Session["potrosac"] = null;

            ViewBag.odjava = $"Odjavili ste se";
            return View("~/Views/Home/Prijava.cshtml");
        }

        /*
        public class ExternalLoginConfirmationViewModel
        {
            [Required]
            [Display(Name = "Email")]
            public string Email { get; set; }
        }
        */

    }
}