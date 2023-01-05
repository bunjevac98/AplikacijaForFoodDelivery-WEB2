using System;
using System.Collections.Generic;
using System.Data;
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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Home/Prijava.cshtml");
        }






        public ActionResult Dashboard()
        {
            Korisnik admin = (Korisnik)Session["administrator"];
            Korisnik dostavljac = (Korisnik)Session["dostavljac"];
            Korisnik potrosac = (Korisnik)Session["potrosac"];

            if (admin!=null || dostavljac != null || potrosac !=null) {
                if (dostavljac != null)
                {
                    if (dostavljac.Status == Status.NEAKTIVAN || dostavljac.Status == Status.ODBIJEN)
                    {
                        ViewBag.Message = "Vas zahtev jos nije odobren ili je odbijen";
                        return View("~/Views/Home/Prijava.cshtml");
                    }
                }
                return View("~/Views/Home/Dashboard.cshtml");
            }


            ViewBag.Message = "Morate se prijaviti";
            return View("~/Views/Home/Prijava.cshtml");

        }

        public ActionResult PrethodnePorudzbine() {
            Korisnik admin = (Korisnik)Session["administrator"];
            Korisnik dostavljac = (Korisnik)Session["dostavljac"];
            Korisnik potrosac = (Korisnik)Session["potrosac"];

            List<Porudzbina> listaPrethodnih = new List<Porudzbina>();


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
                List<Porudzbina> porudzbinaZaDostavljaca = new List<Porudzbina>();
                    foreach (var item2 in porudzbineDostavljaciLista)
                    {
                        if (item2.KorisnickoIme == row.KorisnickoIme)
                        {
                            foreach (var por in porudzbine)
                            {
                                if (por.IdPorudzbine==item2.IdPorudzbine)
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
                        Porudzbine= porudzbinaZaDostavljaca

                    });
                }
            }
            #endregion
            #region CITANJE POTROSACA
            var potrosaciIzBaze = UcitajPotrosaca();
            List<Korisnik> potrosaci = new List<Korisnik>();
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
                        Porudzbine= porudzbinaZaPotrosaca

                    });
                }
            }

            #endregion
            
            if (dostavljac != null)
            {
                if (dostavljac.Porudzbine != null)
                {
                    if (dostavljac.Porudzbine.Count() != 0)
                    {
                        foreach (Porudzbina prethodnaPorudzbina in dostavljac.Porudzbine)
                        {
                            if (prethodnaPorudzbina.StatusPorudzbine == StatusPorudzbine.IZVRSENA)
                            {
                                listaPrethodnih.Add(prethodnaPorudzbina);
                            }
                        }
                        ViewBag.PrethodnePorudzbine = listaPrethodnih;
                        return View("~/Views/Home/PrethodnePorudzbine.cshtml");
                    }
                    else
                    {
                        ViewBag.Greska = "Niste jos izvrsili nijednu porudzbinu";
                        return View("~/Views/Home/DashBoard.cshtml");
                    }
                }
                else
                {
                    ViewBag.Greska = "Niste jos narucili nijednu porudzbinu";
                    return View("~/Views/Home/DashBoard.cshtml");
                }
                }
            if (potrosac != null)
            {
                if (potrosac.Porudzbine != null)
                {
                    if (potrosac.Porudzbine.Count() != 0)
                    {
                        foreach (Porudzbina prethodnaPorudzbina in potrosac.Porudzbine)
                        {
                            if (prethodnaPorudzbina.StatusPorudzbine == StatusPorudzbine.IZVRSENA)
                            {
                                listaPrethodnih.Add(prethodnaPorudzbina);
                            }
                        }
                        if (listaPrethodnih.Count() == 0)
                        {
                            ViewBag.Greska = "Porudzbina jos nije prihvacena";
                            return View("~/Views/Home/DashBoard.cshtml");
                        }
                        ViewBag.PrethodnePorudzbine = listaPrethodnih;
                        return View("~/Views/Home/PrethodnePorudzbine.cshtml");
                    }
                    else
                    {
                        ViewBag.Greska = "Niste jos narucili nijednu porudzbinu";
                        return View("~/Views/Home/DashBoard.cshtml");
                    }
                }
                else {
                    ViewBag.Greska = "Niste jos narucili nijednu porudzbinu";
                    return View("~/Views/Home/DashBoard.cshtml");
                }
            }
              
            
            if (dostavljac == null)
            {
                ViewBag.Greska = "Nemate pravo da izvrsite ovu kaciju";
                return View("~/Views/Home/DashBoard.cshtml");
            }

            if (potrosac == null)
            {
                ViewBag.Greska = "Nemate pravo da izvrsite ovu kaciju";
                return View("~/Views/Home/DashBoard.cshtml");
            }

            return View();


        }
        
        public ActionResult TrenutnaPorudzbina()
        {
            Korisnik admin = (Korisnik)Session["administrator"];
            Korisnik dostavljac = (Korisnik)Session["dostavljac"];
            Korisnik potrosac = (Korisnik)Session["potrosac"];

            // List<Porudzbina> porudzbine = Podaci.CitanjePorudzbine("~/App_Data/Porudzbine.txt");

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

            #region CITANJE DOSTAVLJACA
            var dostavljaciIzBaze = UcitajDostavljaca();
            List<Korisnik> dostavljaci = new List<Korisnik>();

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
            #endregion
            #region CITANJE POTROSACA
            var potrosaciIzBaze = UcitajPotrosaca();
            List<Korisnik> potrosaci = new List<Korisnik>();
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

            if (potrosac!=null) {
                foreach (var item in potrosaci)
                {
                    if (item.KorisnickoIme == potrosac.KorisnickoIme) {
                        potrosac.Porudzbine = item.Porudzbine;
                    }
                }
            }
            if (dostavljac != null)
            {
                foreach (var item in dostavljaci)
                {
                    if (item.KorisnickoIme == dostavljac.KorisnickoIme)
                    {
                        dostavljac.Porudzbine = item.Porudzbine;
                    }
                }
            }





            Porudzbina porudzbina = new Porudzbina();
            TimeSpan proveraVremena = new TimeSpan();
            TimeSpan istekloVreme = new TimeSpan(0,0,0,0);

            int idPorudzbine=0;

            if (dostavljac != null)
            {
                if (dostavljac.Porudzbine != null)
                {
                    if (dostavljac.Porudzbine.Count != 0)
                    {
                        foreach (var item in dostavljac.Porudzbine)
                        {
                            if (item.StatusPorudzbine == StatusPorudzbine.POTVRDJENA)
                            {
                                porudzbina = item;
                                proveraVremena = item.DatumIsporuke.Subtract(DateTime.Now);
                                if (DateTime.Compare(item.DatumIsporuke, DateTime.Now) < 0 || DateTime.Compare(item.DatumIsporuke, DateTime.Now) == 0)
                                {
                                    ViewBag.Greska = "ISPORUKA JE IZVRSENA";
                                    item.StatusPorudzbine = StatusPorudzbine.IZVRSENA;
                                    idPorudzbine = item.IdPorudzbine;

                                    foreach (var zavrsnaPorudzbina in porudzbine)
                                    {
                                        PorudzbinaModel porudzbinaZaBazu = new PorudzbinaModel();
                                        if (idPorudzbine != 0)
                                        {
                                            if (idPorudzbine == zavrsnaPorudzbina.IdPorudzbine)
                                            {
                                                zavrsnaPorudzbina.StatusPorudzbine = StatusPorudzbine.IZVRSENA;
                                                string kolicinaString = "";
                                                foreach (var kolicine in zavrsnaPorudzbina.Kolicina)
                                                {
                                                    kolicinaString = kolicine.ToString() + ",";
                                                }
                                                porudzbinaZaBazu.IdPorudzbine = zavrsnaPorudzbina.IdPorudzbine;
                                                porudzbinaZaBazu.Kolicina = kolicinaString;
                                                porudzbinaZaBazu.Adresa = zavrsnaPorudzbina.Adresa;
                                                porudzbinaZaBazu.Komentar = zavrsnaPorudzbina.Komentar;
                                                porudzbinaZaBazu.CenaDostave = zavrsnaPorudzbina.CenaDostave;
                                                porudzbinaZaBazu.UkupnaCena = zavrsnaPorudzbina.UkupnaCena;
                                                porudzbinaZaBazu.StatusPorudzbine = (StatusPorudzbineData)zavrsnaPorudzbina.StatusPorudzbine;
                                                porudzbinaZaBazu.DatumIsporuke = zavrsnaPorudzbina.DatumIsporuke;
                                                IzmeniStatusPorudzbine(porudzbinaZaBazu);

                                                // Podaci.UpdejtovanjFajlaPorudzbine("C:/Users/basic/Desktop/WEB2/Web2/Web2/App_Data/Porudzbine.txt", zavrsnaPorudzbina);
                                            }
                                        }


                                    }

                                    return View("~/Views/Home/DashBoard.cshtml");
                                }
                                else
                                {
                                    ViewBag.DatumIsporuke = item.DatumIsporuke.Subtract(DateTime.Now);
                                    ViewBag.porudzbina = porudzbina;
                                    return View("~/Views/Home/TrenutnaPorudzbina.cshtml");
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Greska = "Nemate porudzbinu";
                        return View("~/Views/Home/DashBoard.cshtml");
                    }
                }
            }
            if (potrosac != null)
            {
                if (potrosac.Porudzbine != null)
                {
                    if (potrosac.Porudzbine.Count != 0)
                    {
                        foreach (var item in potrosac.Porudzbine)
                        {
                            if (item.StatusPorudzbine == StatusPorudzbine.POTVRDJENA)
                            {
                                porudzbina = item;
                                proveraVremena = item.DatumIsporuke.Subtract(DateTime.Now);
                                if (DateTime.Compare(item.DatumIsporuke, DateTime.Now) < 0 || DateTime.Compare(item.DatumIsporuke, DateTime.Now) == 0)
                                {
                                    ViewBag.Greska = "ISPORUKA JE IZVRSENA";
                                    item.StatusPorudzbine = StatusPorudzbine.IZVRSENA;
                                    idPorudzbine = item.IdPorudzbine;

                                    foreach (var zavrsnaPorudzbina in porudzbine)
                                    {
                                        PorudzbinaModel porudzbinaZaBazu = new PorudzbinaModel();
                                        if (idPorudzbine != 0)
                                        {
                                            if (idPorudzbine == zavrsnaPorudzbina.IdPorudzbine)
                                            {
                                                zavrsnaPorudzbina.StatusPorudzbine = StatusPorudzbine.IZVRSENA;
                                                string kolicinaString = "";
                                                foreach (var kolicine in zavrsnaPorudzbina.Kolicina)
                                                {
                                                    kolicinaString = kolicine.ToString() + ",";
                                                }
                                                porudzbinaZaBazu.IdPorudzbine = zavrsnaPorudzbina.IdPorudzbine;
                                                porudzbinaZaBazu.Kolicina = kolicinaString;
                                                porudzbinaZaBazu.Adresa = zavrsnaPorudzbina.Adresa;
                                                porudzbinaZaBazu.Komentar = zavrsnaPorudzbina.Komentar;
                                                porudzbinaZaBazu.CenaDostave = zavrsnaPorudzbina.CenaDostave;
                                                porudzbinaZaBazu.UkupnaCena = zavrsnaPorudzbina.UkupnaCena;
                                                porudzbinaZaBazu.StatusPorudzbine = (StatusPorudzbineData)zavrsnaPorudzbina.StatusPorudzbine;
                                                porudzbinaZaBazu.DatumIsporuke = zavrsnaPorudzbina.DatumIsporuke;
                                                IzmeniStatusPorudzbine(porudzbinaZaBazu);
                                            }
                                        }


                                    }

                                    return View("~/Views/Home/DashBoard.cshtml");
                                }
                                else
                                {
                                    ViewBag.DatumIsporuke = item.DatumIsporuke.Subtract(DateTime.Now);
                                    ViewBag.porudzbina = porudzbina;
                                }
                                return View("~/Views/Home/TrenutnaPorudzbina.cshtml");
                            }
                            if (item.StatusPorudzbine == StatusPorudzbine.NEPOTVRDJENA)
                            {
                                ViewBag.Greska = "Porudzbina jos nije prihvacena";
                                return View("~/Views/Home/DashBoard.cshtml");
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Greska = "Nemate porudzbinu";
                        return View("~/Views/Home/DashBoard.cshtml");
                    }
                }
                else
                {
                    ViewBag.Greska = "Nemate porudzbinu";
                    return View("~/Views/Home/DashBoard.cshtml");
                }
            }
            //ovo nije sigurno
            if (dostavljac == null)
            {
                ViewBag.Greska = "Nemate pravo ovo da radite";
                return View("~/Views/Home/DashBoard.cshtml");
            }
            if (potrosac == null)
            {
                ViewBag.Greska = "Nemate pravo ovo da radite";
                return View("~/Views/Home/DashBoard.cshtml");
            }
            
            return View();
        }
        
        public ActionResult SvePorudzbine() {
            Korisnik admin = (Korisnik)Session["administrator"];
            Korisnik dostavljac = (Korisnik)Session["dostavljac"];
            Korisnik potrosac = (Korisnik)Session["potrosac"];

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

            List<Porudzbina> svePorudzbine = new List<Porudzbina>();

            if (admin == null)
            {
                ViewBag.Greska = "Nemate pravo da izvrsite ovu kaciju";
                return View("~/Views/Home/DashBoard.cshtml");
            }
            else
            {
               foreach (Porudzbina item in porudzbine)
                {
                    svePorudzbine.Add(item);
                }


                ViewBag.SvePorudzbine = svePorudzbine;
                return View("~/Views/Home/SvePorudzbine.cshtml");
            }
        }
        






        public ActionResult PregledProfila()
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
            

            Korisnik admin = (Korisnik)Session["administrator"];
            Korisnik dostavjac = (Korisnik)Session["dostavljac"];
            Korisnik potrosac = (Korisnik)Session["potrosac"];

            if (admin == null && dostavjac == null && potrosac == null)
            {
                ViewBag.Greska = "Morate se ulogovati!";
                return View("~/Views/Home/Dashboard.cshtml");
            }

            if (admin != null)
            {
                ViewBag.korisnik = admin;
                return View("~/Views/Home/IzmenaProfila.cshtml");
            }
            if (dostavjac != null)
            {
                ViewBag.korisnik = dostavjac;
                return View("~/Views/Home/IzmenaProfila.cshtml");
            }
            if (potrosac != null)
            {
                ViewBag.korisnik = potrosac;
                return View("~/Views/Home/IzmenaProfila.cshtml");
            }

            ViewBag.Greska = "Morate se ulogovati!";
            return View("~/Views/Home/Dashboard.cshtml");
        }
        




        public ActionResult IzmenaProfila(Korisnik kor)
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
            

            Korisnik admin = (Korisnik)Session["administrator"];
            Korisnik dostavljac = (Korisnik)Session["dostavljac"];
            Korisnik potrosac = (Korisnik)Session["potrosac"];
            if (admin==null && dostavljac == null && potrosac==null) {
                ViewBag.Greska = "Morate se ulogovati!";
                return View("~/Views/Home/Dashboard.cshtml");
            }

            if (admin != null) {
                KorisnickiModel korisnikZaBazu = new KorisnickiModel();
                kor.KorisnickoIme = admin.KorisnickoIme;
                if (kor.Slika == null)
                {
                    kor.Slika = admin.Slika;
                }
                if (kor.DatumRodjenja == DateTime.Parse("1/1/0001 12:00:00 AM")) {
                    kor.DatumRodjenja = admin.DatumRodjenja;
                }
                #region KOMENTARISANO PROSLO
                //int count = 0, index = -1;
                //foreach (Korisnik korisnik in administratori)
                //{
                //    if (korisnik.KorisnickoIme == admin.KorisnickoIme)
                //    {
                //        index = count;
                //    }
                //    count++;
                //}
                //
                //administratori.RemoveAt(index);
                //administratori.Insert(index, kor);
                //
                //
                //Podaci.UpdejtovanjFajla("C:/Users/basic/Desktop/WEB2/Web2/Web2/App_Data/Administratori.txt", kor);
                #endregion
                korisnikZaBazu.KorisnickoIme = kor.KorisnickoIme;
                korisnikZaBazu.Lozinka = kor.Lozinka;
                korisnikZaBazu.Ime = kor.Ime;
                korisnikZaBazu.Prezime = kor.Prezime;
                korisnikZaBazu.Adresa = kor.Adresa;
                korisnikZaBazu.Email = kor.Email;
                korisnikZaBazu.DatumRodjenja = kor.DatumRodjenja;
                korisnikZaBazu.TipKorisnika = (TipKorisnikaData)kor.TipKorisnika;
                korisnikZaBazu.Slika = kor.Slika;
                korisnikZaBazu.DrugaLozinka = kor.DrugaLozinka;
                korisnikZaBazu.Status = (StatusData)kor.Status;



                IzmeniPodatkeKorisnika(korisnikZaBazu);

                Session["administrator"] = kor;
                ViewBag.UspesnaIzmena = "Uspesno ste izmenili podatke";
                return View("~/Views/Home/Dashboard.cshtml");
            }
            if (dostavljac != null) {
                KorisnickiModel korisnikZaBazu = new KorisnickiModel();
                kor.KorisnickoIme = dostavljac.KorisnickoIme;
                if (kor.Slika == null)
                {
                    kor.Slika = dostavljac.Slika;
                }
                if (kor.DatumRodjenja == DateTime.Parse("1/1/0001 12:00:00 AM"))
                {
                    kor.DatumRodjenja = dostavljac.DatumRodjenja;
                }
                korisnikZaBazu.KorisnickoIme = kor.KorisnickoIme;
                korisnikZaBazu.Lozinka = kor.Lozinka;
                korisnikZaBazu.Ime = kor.Ime;
                korisnikZaBazu.Prezime = kor.Prezime;
                korisnikZaBazu.Adresa = kor.Adresa;
                korisnikZaBazu.Email = kor.Email;
                korisnikZaBazu.DatumRodjenja = kor.DatumRodjenja;
                korisnikZaBazu.TipKorisnika = (TipKorisnikaData)kor.TipKorisnika;
                korisnikZaBazu.Slika = kor.Slika;
                korisnikZaBazu.DrugaLozinka = kor.DrugaLozinka;
                korisnikZaBazu.Status = (StatusData)kor.Status;



                IzmeniPodatkeKorisnika(korisnikZaBazu);




                Session["dostavljac"] = kor;
                ViewBag.UspesnaIzmena = "Uspesno ste izmenili podatke";
                return View("~/Views/Home/Dashboard.cshtml");
            }
            if (potrosac != null) {
                KorisnickiModel korisnikZaBazu = new KorisnickiModel();
                kor.KorisnickoIme = potrosac.KorisnickoIme;
                if (kor.Slika == null)
                {
                    kor.Slika = potrosac.Slika;
                }
                if (kor.DatumRodjenja == DateTime.Parse("1/1/0001 12:00:00 AM"))
                {
                    kor.DatumRodjenja = potrosac.DatumRodjenja;
                }
                korisnikZaBazu.KorisnickoIme = kor.KorisnickoIme;
                korisnikZaBazu.Lozinka = kor.Lozinka;
                korisnikZaBazu.Ime = kor.Ime;
                korisnikZaBazu.Prezime = kor.Prezime;
                korisnikZaBazu.Adresa = kor.Adresa;
                korisnikZaBazu.Email = kor.Email;
                korisnikZaBazu.DatumRodjenja = kor.DatumRodjenja;
                korisnikZaBazu.TipKorisnika = (TipKorisnikaData)kor.TipKorisnika;
                korisnikZaBazu.Slika = kor.Slika;
                korisnikZaBazu.DrugaLozinka = kor.DrugaLozinka;
                korisnikZaBazu.Status = (StatusData)kor.Status;
                
                IzmeniPodatkeKorisnika(korisnikZaBazu);
                Session["potrosac"] = kor;
                ViewBag.UspesnaIzmena = "Uspesno ste izmenili podatke";
                return View("~/Views/Home/Dashboard.cshtml");
            }
                return View("~/Views/Home/Dashboard.cshtml");
        }









    }
}