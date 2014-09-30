using BgDeal.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BgDeal.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Добавяме ги за да си създадем собствени глобални съобщения за валидация. Създаваем папка App_GlobalResources като в нея добвяме файл Messages.resx в който записваме какво искаме да се покаже при съответната валидация
            //Ако сайта ни е на няколко различни езика добавяме друг файл Messages.bg.resx например ако имаме и БГ версия на сайта
            ClientDataTypeModelValidatorProvider.ResourceClassKey = "Messages";
            DefaultModelBinder.ResourceClassKey = "Messages";


            Database.SetInitializer<ApplicationDbContext>(new MigrateDatabaseToLatestVersion<ApplicationDbContext, DatabaseInitializer>());
        }
    }
}
