using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Web2.Models;
using DataLibrary;
using static DataLibrary.BusinessLogic.KorisnikProcessor;
using DataLibrary.Models;
using DataLibrary.BusinessLogic;

namespace Web2.Controllers
{
    public class AdministratorController : Controller
    {
        public static int minValue = 0000;
        public static int maxValue = 9999;
        // GET: Administrator
        public ActionResult PrikazProfila()
        {
            if (Session["administrator"] == null) {
                ViewBag.GreskaAdministrator = "Morate biti prijavljeni kao administrator!";
                return View("~/Views/Home/Dashboard.cshtml");
            }

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

            ViewBag.dostavljaci = dostavljaci;
            ViewBag.potrosaci = potrosaci;
            
            return View();
        }

        [HttpPost]
        public ActionResult Email(NewCommentEmail mail) {
            NewCommentEmail mailSlanje = new NewCommentEmail();


            mailSlanje.Subject = " Test odobravanje dostavljaca";
            mailSlanje.Body = "Dobili ste odobrenje da postanjete dostavljac";
            mailSlanje.From = "basic.aco@hotmail.com";
            mailSlanje.To = "basic.aco@hotmail.com";

            MailMessage mm = new MailMessage(mailSlanje.From, mailSlanje.To);
            mm.Subject = mailSlanje.Subject;
            mm.Body = mailSlanje.Body;
            mm.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;


            NetworkCredential nc = new NetworkCredential("basic.aco@hotmail.com", "ackosi98");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;
            smtp.Send(mm);
            return View();
        }



        //OVDE TREBA UPDATE FAJLA

        public ActionResult Odobri(string korisnickoIme) {

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
            
            Korisnik k = new Korisnik();
            foreach (var item in dostavljaci)
            {
                if (item.KorisnickoIme == korisnickoIme) {
                    item.Status = Status.AKTIVAN;
                    //ovde saljem mail
                    NewCommentEmail mailSlanje = new NewCommentEmail();

                    mailSlanje.Subject = " Test odobravanje dostavljaca";
                    mailSlanje.Body = "Dobili ste odobrenje da postanjete dostavljac";
                    //mailSlanje.From = "andreabasicsiracki@gmail.com";
                    mailSlanje.From = "basic.aco@hotmail.com";
                    //mailSlanje.To = "andreabasicsiracki@gmail.com";
                    mailSlanje.To = "basic.aco@hotmail.com";

                    MailMessage mail = new MailMessage(mailSlanje.From, mailSlanje.To);
                    mail.Subject = mailSlanje.Subject;
                    mail.Body = mailSlanje.Body;
                    mail.IsBodyHtml = false;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.office365.com";//smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = true;


                    //NetworkCredential nc = new NetworkCredential("andreabasicsiracki@gmail.com", "11142107Ba");//
                    NetworkCredential nc = new NetworkCredential("basic.aco@hotmail.com", "ackosi98");
                    smtp.Credentials = nc;
                    smtp.Send(mail);
                   
                }
                k = item;
            }
            //uploadovati datoteku i opet pozvati pregled profila
            KorisnickiModel korisnikModel = new KorisnickiModel();
            korisnikModel.KorisnickoIme = k.KorisnickoIme;
            korisnikModel.Lozinka = k.Lozinka;
            korisnikModel.Ime = k.Ime;
            korisnikModel.Prezime = k.Prezime;
            korisnikModel.Adresa = k.Adresa;
            korisnikModel.Email = k.Email;
            korisnikModel.DatumRodjenja = k.DatumRodjenja;
            korisnikModel.TipKorisnika = (TipKorisnikaData)k.TipKorisnika;
            korisnikModel.Slika = k.Slika;
            korisnikModel.Status = (StatusData)k.Status;
            
            IzmeniStatusDostavljaca(korisnikModel);

            //Podaci.UpdejtovanjFajla("C:/Users/basic/Desktop/WEB2/Web2/Web2/App_Data/Dostavljaci.txt", k);


            // return View("~/Views/Administrator/PrikazProfila.cshtml");
            ViewBag.Message = "Uspesno ste odobrili zahtev";
            return View("~/Views/Home/Prijava.cshtml");
        }

        public ActionResult Odbij(string korisnickoIme)
        {
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



            //List<Korisnik> dostavljaci = (List<Korisnik>)HttpContext.Application["dostavljaci"];
            Korisnik k = new Korisnik();
            foreach (var item in dostavljaci)
            {
                if (item.KorisnickoIme == korisnickoIme)
                {
                    item.Status = Status.ODBIJEN;
                }
                k = item;
            }

            KorisnickiModel korisnikModel = new KorisnickiModel();
            korisnikModel.KorisnickoIme = k.KorisnickoIme;
            korisnikModel.Lozinka = k.Lozinka;
            korisnikModel.Ime = k.Ime;
            korisnikModel.Prezime = k.Prezime;
            korisnikModel.Adresa = k.Adresa;
            korisnikModel.Email = k.Email;
            korisnikModel.DatumRodjenja = k.DatumRodjenja;
            korisnikModel.TipKorisnika = (TipKorisnikaData)k.TipKorisnika;
            korisnikModel.Slika = k.Slika;
            korisnikModel.Status = (StatusData)k.Status;

            IzmeniStatusDostavljaca(korisnikModel);
            //uploadovati datoteku i opet pozvati pregled profila
            //OVO ZAKOMENTARISAO ZBOG BAZE
            //Podaci.UpdejtovanjFajla("C:/Users/basic/Desktop/WEB2/Web2/Web2/App_Data/Dostavljaci.txt", k);

            // return View("~/Views/Administrator/PrikazProfila.cshtml");
            return View("~/Views/Home/Dashboard.cshtml");
        }

        public ActionResult DodajProizvod() {
            return View();
        }

        [HttpPost]
        public ActionResult DodajProizvod(Proizvod proizvod)
        {
            if (proizvod.Cena == 0 || proizvod.ImeProizvoda == null || proizvod.Sastojci == null) {
                ViewBag.GreskaDodavanja = "Niste dobro uneli polja za dodavanje proizvoda";
                return View("~/Views/Home/Dashboard.cshtml");
            }
            Random random = new Random();
            proizvod.IdProizvoda = random.Next(minValue, maxValue);
            //
            //ProizvodData proizvodZaBazu = new ProizvodData();
            //proizvodZaBazu.IdProizvoda = proizvod.IdProizvoda;
            //proizvodZaBazu.ImeProizvoda = proizvod.ImeProizvoda;
            //proizvodZaBazu.Cena = proizvod.Cena;
            //proizvodZaBazu.Sastojci = proizvod.Sastojci;
            //
            //Podaci.CuvanjeProizvoda(proizvod);

            //DOdali
            //MISLIM DA MI OVO NE TREBA JER SADA NECEMO MORATI DA RADIMO TO SA SEISJAMA ALI VIDECEMO
            ProizvodiProcessor.DodajProizvod(proizvod.ImeProizvoda, proizvod.IdProizvoda, proizvod.Cena,proizvod.Sastojci);
            //OVO MI ISTO NE TREBA VRV
            Session["proizvodi2"] = Podaci.CitanjeProizvoda("~/App_Data/Proizvodi.txt");

            ViewBag.DodatiProizvod = "Uspesno se dodali Proizvod";
            return View("~/Views/Home/Dashboard.cshtml");
        }
        

    }
}