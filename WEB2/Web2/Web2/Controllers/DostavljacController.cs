using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web2.Models;
using static DataLibrary.BusinessLogic.KorisnikProcessor;
using static DataLibrary.BusinessLogic.PorudzbineProcessor;
using static DataLibrary.BusinessLogic.ProizvodiProcessor;
using DataLibrary.Models;
using DataLibrary.BusinessLogic;

namespace Web2.Controllers
{
    public class DostavljacController : Controller
    {
        // GET: Dostavljac
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult DostupnePorudzbine()
        {
            //OGRANICITI ILI PROVERITI DA AKO VEC IMA NARUCENU PORUDZBINU NE MOZE OPET DA NARUCUJE
            #region CITANJE TABELA ZA SJEDINJAVANJE
            var porudzbineProizvodi = UcitajPorudzbineProizvod();
            var porudzbineDostavljaci = UcitajDostavljacaPorudzbine();
            List<PorProModel> porudzbineProizvodiLista = new List<PorProModel>();
            List<PorDosModel> porudzbineDostavljaciLista = new List<PorDosModel>();
            foreach (var item in porudzbineProizvodi)
            {
                porudzbineProizvodiLista.Add(item);
            }
            foreach (var item2 in porudzbineDostavljaci)
            {
                porudzbineDostavljaciLista.Add(item2);
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
            if (porudzbineIzBaze != null) {
                foreach (var row in porudzbineIzBaze)
                {
                    Porudzbina porudzbina = new Porudzbina();
                    List<int> kolicinaProizvoda = new List<int>();
                    string[] tokens = row.Kolicina.Split(',');
                    foreach (var item in tokens)
                    {
                        if (item != "") {
                            kolicinaProizvoda.Add(Int32.Parse(item));
                        }
                    }
                    porudzbina.IdPorudzbine = row.IdPorudzbine;
                    porudzbina.Kolicina = kolicinaProizvoda;
                    porudzbina.Adresa = row.Adresa;
                    porudzbina.Komentar = row.Komentar;
                    porudzbina.CenaDostave = row.CenaDostave;
                    porudzbina.UkupnaCena = row.UkupnaCena;
                    porudzbina.StatusPorudzbine =(StatusPorudzbine)row.StatusPorudzbine;
                    porudzbina.DatumIsporuke = row.DatumIsporuke;

                    List<Proizvod> proizvodiZaPorudzbinu = new List<Proizvod>();
                    foreach (var item2 in porudzbineProizvodiLista)
                    {
                        if (item2.IdPorudzbine == row.IdPorudzbine) {
                            foreach (var proizvod in proizvodi)
                            {
                                if (proizvod.IdProizvoda == item2.IdProizvoda) {
                                    proizvodiZaPorudzbinu.Add(proizvod);
                                }
                            }
                        }
                    }
                    porudzbina.proizvodi = proizvodiZaPorudzbinu; ;
                    porudzbine.Add(porudzbina);
                }
                
            }

            #endregion

            //TREBA JOS DODATI PORUDZBINE U DOSTAVLJACA
            #region CITANJE DOSTAVLJACA
            var dostavljaciIzBaze = UcitajDostavljaca();
            List<Korisnik> dostavljaci = new List<Korisnik>();
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
            #endregion



            //List<Korisnik> dostavljaci = (List<Korisnik>)HttpContext.Application["dostavljaci"];
            Korisnik dostavljac = (Korisnik)Session["dostavljac"];

            if (dostavljac.Porudzbine != null)
            {
                foreach (var item in dostavljac.Porudzbine)
                {
                    if (item.StatusPorudzbine == StatusPorudzbine.POTVRDJENA)
                    {
                        ViewBag.NemaDostupnihPorudzbina = "VEC IMATE JEDNU POTVRDJENU PORUDZBINU";
                        return View("~/Views/Home/Dashboard.cshtml");
                    }
                }
            }


            //List<Porudzbina> porudzbine = Podaci.CitanjePorudzbine("~/App_Data/Porudzbine.txt");

            List<Porudzbina> dostupnePorudzbine = new List<Porudzbina>();

            foreach (var item in porudzbine)
            {
                if (item.StatusPorudzbine == StatusPorudzbine.NEPOTVRDJENA) {
                    dostupnePorudzbine.Add(item);
                }
            }

            ViewBag.porudzbine = dostupnePorudzbine;
            if (dostupnePorudzbine.Count == 0) {
                ViewBag.NemaDostupnihPorudzbina = "Nema dostupnih porudzbina";
                return View("~/Views/Home/Dashboard.cshtml");
            }
            return View();
        }

        public ActionResult PrihvatanjePorudzbine(int idPorudzbine) {

            #region CITANJE TABELA ZA SJEDINJAVANJE
            var porudzbineProizvodi = UcitajPorudzbineProizvod();
            var porudzbineDostavljaci = UcitajDostavljacaPorudzbine();
            List<PorProModel> porudzbineProizvodiLista = new List<PorProModel>();
            List<PorDosModel> porudzbineDostavljaciLista = new List<PorDosModel>();
            foreach (var item in porudzbineProizvodi)
            {
                porudzbineProizvodiLista.Add(item);
            }
            foreach (var item2 in porudzbineDostavljaci)
            {
                porudzbineDostavljaciLista.Add(item2);
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
                    Porudzbina porudzbina = new Porudzbina();
                    List<int> kolicinaProizvoda = new List<int>();
                    string[] tokens = row.Kolicina.Split(',');
                    foreach (var item in tokens)
                    {
                        if (item != "")
                        {
                            kolicinaProizvoda.Add(Int32.Parse(item));
                        }
                    }
                    porudzbina.IdPorudzbine = row.IdPorudzbine;
                    porudzbina.Kolicina = kolicinaProizvoda;
                    porudzbina.Adresa = row.Adresa;
                    porudzbina.Komentar = row.Komentar;
                    porudzbina.CenaDostave = row.CenaDostave;
                    porudzbina.UkupnaCena = row.UkupnaCena;
                    porudzbina.StatusPorudzbine = (StatusPorudzbine)row.StatusPorudzbine;
                    porudzbina.DatumIsporuke = row.DatumIsporuke;

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
                    porudzbina.proizvodi = proizvodiZaPorudzbinu; ;
                    porudzbine.Add(porudzbina);
                }

            }

            #endregion

            #region CITANJE DOSTAVLJACA
            var dostavljaciIzBaze = UcitajDostavljaca();
            List<Korisnik> dostavljaci = new List<Korisnik>();
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
            #endregion

            //List<Porudzbina> porudzbine = Podaci.CitanjePorudzbine("~/App_Data/Porudzbine.txt");
            //List<Korisnik> dostavljaci = (List<Korisnik>)HttpContext.Application["dostavljaci"];



            List<Korisnik> potrosaci = (List<Korisnik>)HttpContext.Application["potrosaci"];
            
            Korisnik dostavljac = (Korisnik)Session["dostavljac"];


            PorudzbinaModel porudzbinaZaBazu = new PorudzbinaModel();
            foreach (Porudzbina por in porudzbine)
            {
                if (por.IdPorudzbine==idPorudzbine)
                {
                    por.StatusPorudzbine = StatusPorudzbine.POTVRDJENA;
                    por.DatumIsporuke = DateTime.Now.AddMinutes(2);
                    ViewBag.porudzbina = por;
                    porudzbinaZaBazu.IdPorudzbine = por.IdPorudzbine;
                    string kolicinaString = "";
                    foreach (var item in por.Kolicina)
                    {
                        kolicinaString = item.ToString() + ",";
                    }
                    porudzbinaZaBazu.Kolicina = kolicinaString;
                    porudzbinaZaBazu.Adresa = por.Adresa;
                    porudzbinaZaBazu.Komentar = por.Komentar;
                    porudzbinaZaBazu.CenaDostave = por.CenaDostave;
                    porudzbinaZaBazu.UkupnaCena = por.UkupnaCena;
                    porudzbinaZaBazu.StatusPorudzbine = (StatusPorudzbineData)por.StatusPorudzbine;
                    porudzbinaZaBazu.DatumIsporuke = por.DatumIsporuke;
                    IzmeniDatumIsporuke(porudzbinaZaBazu);
                    IzmeniStatusPorudzbine(porudzbinaZaBazu);

                    //Podaci.UpdejtovanjFajlaPorudzbine("C:/Users/basic/Desktop/WEB2/Web2/Web2/App_Data/Porudzbine.txt", por);
                }
            }
            //ovde treba da mu se dopise porudzbina i krene odbrojavanje

            if (dostavljac != null) {
                foreach (Korisnik dost in dostavljaci)
                {
                    if (dost.KorisnickoIme == dostavljac.KorisnickoIme) {
                       //OVDE TREBA DA UPISE U DOSTAVLJAC PORUDZBINA BAZU
                       DodajDostavljacaPorudzbinu(idPorudzbine, dost.KorisnickoIme);
                       //dost.Porudzbine.Add(item);
                       //Podaci.UpdejtovanjFajla("C:/Users/basic/Desktop/WEB2/Web2/Web2/App_Data/Dostavljaci.txt", dost);
                    }
                }
            }
            ViewBag.DatumIsporuke = DateTime.Now.AddMinutes(2);

            return View("~/Views/Home/TrenutnaPorudzbina.cshtml");
        }







    }
}