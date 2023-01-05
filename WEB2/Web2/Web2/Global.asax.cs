using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web2.Models;
using DataLibrary;
using static DataLibrary.BusinessLogic.KorisnikProcessor;
using DataLibrary.Models;

namespace Web2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ////izbrisati mozda
            var korisnici2 = UcitajPotrosaca();

            List<Korisnik> administrator = Podaci.CitanjeAdministratora("~/App_Data/Administratori.txt");
            HttpContext.Current.Application["administratori"] = administrator;
            

            List<Korisnik> dostavljac = Podaci.CitanjeDostavljaca("~/App_Data/Dostavljaci.txt");
            HttpContext.Current.Application["dostavljaci"] = dostavljac;


            List<Korisnik> potrosac = Podaci.CitanjePotrosaca("~/App_Data/Potrosaci.txt");
            HttpContext.Current.Application["potrosaci"] = potrosac;

            List<Proizvod> proizvodi = Podaci.CitanjeProizvoda("~/App_Data/Proizvodi.txt");
            HttpContext.Current.Application["proizvodi"] = proizvodi;

            List<Porudzbina> porudzbine = Podaci.CitanjePorudzbine("~/App_Data/Porudzbine.txt");
            HttpContext.Current.Application["porudzbine"] = porudzbine;


            /*

            string path = Path.Combine(Server.MapPath("~/Files/"));
            List<UploadedFile> files = new List<UploadedFile>();
            foreach (string file in Directory.GetFiles(path))
            {
                files.Add(new UploadedFile(Path.GetFileName(file), file));
            }
            HttpContext.Current.Application["Files"] = files;

            */
        }








    }
}
