using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Web2.Models
{
    public class Podaci
    {
        public static List<Korisnik> CitanjeAdministratora(string path)
        {
            List<Korisnik> korisnici = new List<Korisnik>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');

                //ovde smo dirali
                Korisnik k = new Korisnik(tokens[0], tokens[1],tokens[2],tokens[3],tokens[4],tokens[5], DateTime.Parse(tokens[6]),
                    (TipKorisnika)Enum.Parse(typeof(TipKorisnika), tokens[7]), tokens[8],
                    (Status)Enum.Parse(typeof(Status), tokens[9]),null,false);
                korisnici.Add(k);
            }
            sr.Close();
            stream.Close();

            return korisnici;

        }
        public static List<Korisnik> CitanjeDostavljaca(string path)
        {
            List<Korisnik> korisnici = new List<Korisnik>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";


            List<Porudzbina> porudzbine = new List<Porudzbina>();
            porudzbine = CitanjePorudzbine("~/App_Data/Porudzbine.txt");

            List<Porudzbina> porudzbineKorisnika = new List<Porudzbina>();
            List<Porudzbina> praznaListaPorudzbina = new List<Porudzbina>(); 
            
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');

                string[] listaIdPorudzbine = tokens[10].Split(',');
                //citanjePorudzbinai proizvoda

                if (tokens[10] != "")
                {
                    foreach (Porudzbina por in porudzbine)
                    {
                        foreach (string item in listaIdPorudzbine)
                        {
                            if (item != "")
                            {
                                if (por.IdPorudzbine == Int32.Parse(item))
                                {
                                    porudzbineKorisnika.Add(por);
                                }
                            }
                        }
                    }
                    Korisnik k = new Korisnik(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], DateTime.Parse(tokens[6]), (TipKorisnika)Enum.Parse(typeof(TipKorisnika), tokens[7]), tokens[8], (Status)Enum.Parse(typeof(Status), tokens[9]), porudzbineKorisnika, false);
                    korisnici.Add(k);
                }
                else {
                    Korisnik k = new Korisnik(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], DateTime.Parse(tokens[6]), (TipKorisnika)Enum.Parse(typeof(TipKorisnika), tokens[7]), tokens[8], (Status)Enum.Parse(typeof(Status), tokens[9]), praznaListaPorudzbina, false);
                    korisnici.Add(k);
                }

                porudzbineKorisnika = new List<Porudzbina>();
                praznaListaPorudzbina = new List<Porudzbina>();



            }

            sr.Close();
            stream.Close();

            return korisnici;
        }
        public static List<Korisnik> CitanjePotrosaca(string path)
        {
            List<Korisnik> korisnici = new List<Korisnik>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            List<Porudzbina> porudzbine = new List<Porudzbina>();
            porudzbine = CitanjePorudzbine("~/App_Data/Porudzbine.txt");

            List<Porudzbina> porudzbineKorisnika = new List<Porudzbina>();
            List<Porudzbina> praznaListaPorudzbina = new List<Porudzbina>();

            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');

                    string[] listaIdPorudzbine = tokens[10].Split(',');
                if (tokens[10] != "")
                {
                        foreach (Porudzbina por in porudzbine)
                        {
                            foreach (string item in listaIdPorudzbine)
                            {
                                if (item != "") {
                                    if (por.IdPorudzbine == Int32.Parse(item))
                                    {
                                        porudzbineKorisnika.Add(por);
                                    }
                                }   
                            }
                        }
                    
                    Korisnik k = new Korisnik(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], DateTime.Parse(tokens[6]), (TipKorisnika)Enum.Parse(typeof(TipKorisnika), tokens[7]), tokens[8], (Status)Enum.Parse(typeof(Status), tokens[9]), porudzbineKorisnika, false);
                    korisnici.Add(k);
                }
                else {
                    Korisnik k = new Korisnik(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], DateTime.Parse(tokens[6]), (TipKorisnika)Enum.Parse(typeof(TipKorisnika), tokens[7]), tokens[8], (Status)Enum.Parse(typeof(Status), tokens[9]), praznaListaPorudzbina, false);
                    korisnici.Add(k);

                }
                porudzbineKorisnika = new List<Porudzbina>();
                praznaListaPorudzbina = new List<Porudzbina>();
            }

            sr.Close();
            stream.Close();

            return korisnici;
        }
        
        public static string CuvanjeKorisnika(Korisnik k)
        {
            if (k.TipKorisnika == TipKorisnika.DOSTAVLJAC)
            {
                string path = HostingEnvironment.MapPath("~/App_Data/Dostavljaci.txt");
                FileStream stream = new FileStream(path, FileMode.Append);
                StreamWriter sw = new StreamWriter(stream);

                string idPorudzbina = "";
                if (k.Porudzbine != null) {
                    int brojPoruzbina = k.Porudzbine.Count;
                    foreach (var item in k.Porudzbine)
                    {
                        if (brojPoruzbina+1== brojPoruzbina) {
                            idPorudzbina = idPorudzbina + item.IdPorudzbine;
                            break;
                        }
                            idPorudzbina = idPorudzbina + item.IdPorudzbine + ",";
                    }
                }

                sw.WriteLine(k.KorisnickoIme + ";" + k.Lozinka + ";" + k.Ime + ";" + k.Prezime + ";" + k.Adresa + ";" + k.Email + ";" + k.DatumRodjenja + ";" + k.TipKorisnika + ";" + k.Slika + ";" + k.Status + ";" + idPorudzbina );

                sw.Close();
                stream.Close();
                return k.KorisnickoIme;

            }
            if (k.TipKorisnika == TipKorisnika.POTROSAC)
            {
                string path = HostingEnvironment.MapPath("~/App_Data/Potrosaci.txt");
                FileStream stream = new FileStream(path, FileMode.Append);
                StreamWriter sw = new StreamWriter(stream);

                string idPorudzbina = "";
                if (k.Porudzbine != null)
                {
                    int brojPoruzbina = k.Porudzbine.Count;
                    foreach (var item in k.Porudzbine)
                    {
                        if (brojPoruzbina + 1 == brojPoruzbina)
                        {
                            idPorudzbina = idPorudzbina + item.IdPorudzbine;
                            break;
                        }
                        idPorudzbina = idPorudzbina + item.IdPorudzbine + ",";
                    }
                }

                sw.WriteLine(k.KorisnickoIme + ";" + k.Lozinka + ";" + k.Ime + ";" + k.Prezime + ";" + k.Adresa +";" + k.Email + ";" + k.DatumRodjenja + ";" + k.TipKorisnika + ";" + k.Slika + ";" + k.Status + ";" + idPorudzbina);

                sw.Close();
                stream.Close();
                return k.KorisnickoIme;
            }
            if (k.TipKorisnika == TipKorisnika.ADMINISTRATOR)
            {
                string path = HostingEnvironment.MapPath("~/App_Data/Administrator.txt");
                FileStream stream = new FileStream(path, FileMode.Append);
                StreamWriter sw = new StreamWriter(stream);
                

                sw.WriteLine(k.KorisnickoIme + ";" + k.Lozinka + ";" + k.Ime + ";" + k.Prezime + ";" + k.Adresa + ";" + k.Email + ";" + k.DatumRodjenja + ";" + k.TipKorisnika + ";" + k.Slika + ";" + k.Status);

                sw.Close();
                stream.Close();
                return k.KorisnickoIme;
            }
            
            return k.KorisnickoIme;

        }


        public static string CuvanjeProizvoda(Proizvod proizvod)
        {
                string path = HostingEnvironment.MapPath("~/App_Data/Proizvodi.txt");
                FileStream stream = new FileStream(path, FileMode.Append);
                StreamWriter sw = new StreamWriter(stream);

                sw.WriteLine(proizvod.ImeProizvoda + ";" + proizvod.IdProizvoda + ";" + proizvod.Cena + ";" + proizvod.Sastojci);


                sw.Close();
                stream.Close();
                return proizvod.ImeProizvoda;
            
        }

        public static List<Proizvod> CitanjeProizvoda(string path)
        {
            List<Proizvod> proizvodi = new List<Proizvod>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');

                //ovde smo dirali
                Proizvod p = new Proizvod(tokens[0], Int32.Parse(tokens[1]), Int32.Parse(tokens[2]),tokens[3]);
                proizvodi.Add(p);
            }

            sr.Close();
            stream.Close();

            return proizvodi;
        }
        
        //ovdetreba menjati DATUM
        public static int CuvanjePorudzbine(Porudzbina porudzbina)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Porudzbine.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);
            

            int i = 0;
            int a = porudzbina.proizvodi.Count;
            string idProizvoda = "";
            foreach (Proizvod pro in porudzbina.proizvodi)
            {
                i++;
                idProizvoda = idProizvoda + pro.IdProizvoda;
                if (a != i)
                {
                    idProizvoda = idProizvoda + ",";
                }
            }
            //TREBA ISTA LOGIKA ZA CITANJE JEBENE KOLICINE
            int j = 0;
            int b = porudzbina.Kolicina.Count;
            string kolicinaProizvoda = "";

            foreach (var item in porudzbina.Kolicina)
            {
                j++;
                kolicinaProizvoda = kolicinaProizvoda + item;
                if (b != j) {
                    kolicinaProizvoda = kolicinaProizvoda + ",";
                }

            }



            sw.WriteLine(porudzbina.IdPorudzbine+";"+ idProizvoda +";"+ kolicinaProizvoda + ";" +porudzbina.Adresa + ";" +porudzbina.Komentar + ";" + porudzbina.CenaDostave + ";" +porudzbina.UkupnaCena + ";" + porudzbina.StatusPorudzbine + ";" + porudzbina.DatumIsporuke);
            
            sw.Close();
            stream.Close();
            return porudzbina.IdPorudzbine;
            
        }
        //Ovde treba menjati
        public static List<Porudzbina> CitanjePorudzbine(string path)
        {
            List<Porudzbina> porudzbine = new List<Porudzbina>();
            List<Proizvod> proizvodi = new List<Proizvod>();
            proizvodi = CitanjeProizvoda("~/App_Data/Proizvodi.txt");

            List<Proizvod> proizvodiPorudzbine = new List<Proizvod>();


            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');


                string[] listaId= tokens[1].Split(',');

                foreach (Proizvod proizvod in proizvodi)
                {
                    foreach (string item in listaId)
                    {
                        if (proizvod.IdProizvoda == Int32.Parse(item)) {
                            
                            proizvodiPorudzbine.Add(proizvod);
                        }
                    }
                }

                List<int> kolicina = new List<int>();

                string[] listaKolicine = tokens[2].Split(',');
                
                foreach (var item in listaKolicine)
                {
                    kolicina.Add(Int32.Parse(item));
                }
                DateTime dateTime = new DateTime();
                if (tokens[8] == "")
                {

                }
                else
                {
                    dateTime = DateTime.Parse(tokens[8]);
                }
                Porudzbina p = new Porudzbina(Int32.Parse(tokens[0]), proizvodiPorudzbine, kolicina, tokens[3], tokens[4], Int32.Parse(tokens[5]), Int32.Parse(tokens[6]), (StatusPorudzbine)Enum.Parse(typeof(StatusPorudzbine), tokens[7]),dateTime);
               





                porudzbine.Add(p);
                proizvodiPorudzbine = new List<Proizvod>();
            }

            sr.Close();
            stream.Close();
            return porudzbine;
        }
        

        public static string UpdejtovanjFajla(string path, Korisnik k)
        {
            string[] nizLinija = File.ReadAllLines(path);
            int count = 0, index = -1;
            foreach (var item in nizLinija)
            {
                string[] tokens = item.Split(';');
                
                if (tokens[0] == k.KorisnickoIme)
                {
                    index = count;

                }

                count++;
            }
            string porudzbine = "";
            if (k.Porudzbine != null)
            {
                foreach (var item in k.Porudzbine)
                {
                    porudzbine = porudzbine + item.IdPorudzbine.ToString() + ",";
                }
            }


            string a = k.KorisnickoIme + ";" + k.Lozinka + ";" + k.Ime + ";" + k.Prezime + ";" + k.Adresa + ";" + k.Email + ";" + k.DatumRodjenja + ";" + k.TipKorisnika + ";" + k.Slika + ";" + k.Status + ";" + porudzbine; 
            nizLinija[index] = a;
            File.WriteAllLines(path, nizLinija);
            porudzbine = "";
            return k.KorisnickoIme;

            /*
            Aleksandar;qwert;Aleksandar;Basic;Solohova38;basic.aco@hotmail.com;9/8/2020 12:00:00 AM;ADMINISTRATOR;URLSLIKE;AKTIVAN
            Fedor;kukica;Fedor;Tapavicki;sekspirova;basic.aco@hotmail.com;4/5/2022 12:00:00 AM;DOSTAVLJAC;Bart_Simpson.png;NEAKTIVAN
            Nina;zxcvbn;Nina;Basic;puskinova;basic.aco@hotmail.com;2/8/2022 12:00:00 AM;POTROSAC;Lisa_Simpson.png;AKTIVAN
            */
        }

        public static int UpdejtovanjFajlaPorudzbine(string path, Porudzbina porudzbina)
        {
            string[] nizLinija = File.ReadAllLines(path);
            int count = 0, index = -1;
            foreach (var item in nizLinija)
            {
                string[] tokens = item.Split(';');

                if (tokens[0] == porudzbina.IdPorudzbine.ToString())
                {
                    index = count;
                }

                count++;
            }

            int i = 0;
            int a = porudzbina.proizvodi.Count;
            string idProizvoda = "";
            foreach (Proizvod pro in porudzbina.proizvodi)
            {
                i++;
                idProizvoda = idProizvoda + pro.IdProizvoda;
                if (a != i)
                {
                    idProizvoda = idProizvoda + ",";
                }
            }
            //TREBA ISTA LOGIKA ZA CITANJE JEBENE KOLICINE
            int j = 0;
            int b = porudzbina.Kolicina.Count;
            string kolicinaProizvoda = "";

            foreach (var item in porudzbina.Kolicina)
            {
                j++;
                kolicinaProizvoda = kolicinaProizvoda + item;
                if (b != j)
                {
                    kolicinaProizvoda = kolicinaProizvoda + ",";
                }

            }
            string c = porudzbina.IdPorudzbine + ";" + idProizvoda + ";" + kolicinaProizvoda + ";" + porudzbina.Adresa + ";" + porudzbina.Komentar + ";" + porudzbina.CenaDostave + ";" + porudzbina.UkupnaCena + ";" + porudzbina.StatusPorudzbine+";"+porudzbina.DatumIsporuke;
            nizLinija[index] = c;

            //9000;1;5;Solohova388;pica bez sunke;50;6050;NEPOTVRDJENA
            File.WriteAllLines(path, nizLinija);

            return porudzbina.IdPorudzbine;
            
        }




    }
}