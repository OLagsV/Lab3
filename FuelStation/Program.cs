using OrganizationsWaterSupply.Data;
using OrganizationsWaterSupply.Infrastructure;
using OrganizationsWaterSupply.Middleware;
using OrganizationsWaterSupply.Models;
using OrganizationsWaterSupply.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using System;
using System.Web;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using OrganizationsWaterSupply.Models;
using Microsoft.AspNetCore.Html;
using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using NuGet.RuntimeModel;
using System.IO.Pipelines;
using System.Xml.Linq;

namespace OrganizationsWaterSupply
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            string connection = builder.Configuration.GetConnectionString("SqlServerConnection");
            services.AddDbContext<OrganizationsWaterSupplyContext>(options => options.UseSqlServer(connection));

            services.AddMemoryCache();

            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddScoped<ICachedCountersService, CachedCountersService>();
            services.AddScoped<IOrganizationsService, OrganizationsService>();

            var app = builder.Build();

            app.UseStaticFiles();

            app.UseSession();

            app.UseDbInitializer();
            
            app.Map("/form1", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    string key = "cs";
                    CounterSession cs = context.Session.Get<CounterSession>(key) ?? new CounterSession(-1,-1);
                    ICachedCountersService cachedCountersService = context.RequestServices.GetService<ICachedCountersService>();
                    IEnumerable<CounterModel> models = cachedCountersService.GetCounterModels(20);
                    IEnumerable<Counter> countersSelect = cachedCountersService.GetCountersByRegNumAndModel(cs.RegistrationNumber, cs.ModelId);
                    if (context.Request.Method == "POST")
                    {
                        string reg_num = context.Request.Form["RegNum"];
                        string model = context.Request.Form["ModelSelect"];
                        cs.RegistrationNumber = Int32.Parse(reg_num);
                        cs.ModelId = Int32.Parse(model);
                        context.Session.Set(key, cs);
                    }
                    
                    string strResponse = "<HTML><HEAD><TITLE>Form1</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><FORM action ='/form1' method = \"POST\"/ >" +
                    "Регистрационный номер:<BR><INPUT type = 'text' name = 'RegNum' value=" + cs.RegistrationNumber + ">";
                    strResponse += "<label for= \"ModelSelect\"> Выберите модель:</label>";
                    strResponse += "<select id=\"ModelSelect\" name=\"ModelSelect\"> ";
                    foreach (var m in models)
                    {
                        if (m.ModelId == cs.ModelId)
                        {
                            strResponse += "<option value=\"" + m.ModelId + "\" selected>" + m.ModelName + "</option>";
                        }
                        else
                        {
                            strResponse += "<option value=\"" + m.ModelId + "\">" + m.ModelName + "</option>";
                        }
                        
                    }
                    strResponse += "</select>";
                    strResponse += "<TABLE BORDER=1>";
                    strResponse += "<TR>";
                    strResponse += "<TH>Регистрационный номер</TH>";
                    strResponse += "<TH>Модель</TH>";
                    strResponse += "<TH>Время установки</TH>";
                    strResponse += "</TR>";
                    foreach (var counter in countersSelect)
                    {
                        strResponse += "<TR>";
                        strResponse += "<TD>" + counter.RegistrationNumber + "</TD>";
                        strResponse += "<TD>" + counter.Model.ModelName + "</TD>";
                        strResponse += "<TD>" + counter.TimeOfInstallation + "</TD>";
                        strResponse += "<TD>" + counter.Organization.OrgName + "</TD>";
                        strResponse += "</TR>";
                    }
                    strResponse += "</TABLE>";
                    strResponse += "<BR><BR><INPUT type ='submit' value='Сохранить в Session'></FORM>";
                    strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";
                    await context.Response.WriteAsync(strResponse);
                });
            });



            app.Map("/form2", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    string key = "org_cookie";
                    string org_name = "";
                    CookieOptions options = new CookieOptions { Expires = DateTime.Now.AddDays(1) };
                    if (context.Request.Method == "POST")
                    {

                        org_name = context.Request.Form["OrgName"];
                        context.Response.Cookies.Append(key, org_name, options);

                    }
                    else if (context.Request.Cookies.ContainsKey(key))
                    {
                        org_name = context.Request.Cookies[key];
                    }
                    ICachedCountersService cachedCountersService = context.RequestServices.GetService<ICachedCountersService>();
                    IOrganizationsService organizationService = context.RequestServices.GetService<IOrganizationsService>();
                    IEnumerable<Organization> models = organizationService.GetOrganizations(20);
                    IEnumerable<Counter> countersByOrg = cachedCountersService.GetCountersByOrganization(org_name);
                    
                    string strResponse = "<HTML><HEAD><TITLE>Form2</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><FORM action ='/form2' method = \"POST\"/ >" +
                    "Название организации:<BR><INPUT type = 'text' name = 'OrgName' value=" + org_name + ">";
                    strResponse += "<TABLE BORDER=1>";
                    strResponse += "<TR>";
                    strResponse += "<TH>Регистрационный номер</TH>";
                    strResponse += "<TH>Модель</TH>";
                    strResponse += "<TH>Время установки</TH>";
                    strResponse += "</TR>";
                    foreach (var counter in countersByOrg)
                    {
                        strResponse += "<TR>";
                        strResponse += "<TD>" + counter.RegistrationNumber + "</TD>";
                        strResponse += "<TD>" + counter.Model.ModelName + "</TD>";
                        strResponse += "<TD>" + counter.TimeOfInstallation + "</TD>";
                        strResponse += "</TR>";
                    }
                    strResponse += "</TABLE>";
                    strResponse += "<BR><BR><INPUT type ='submit' value='Сохранить в Cookie'><INPUT type ='submit' value='Показать'></FORM>";
                    strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";
                    await context.Response.WriteAsync(strResponse);
                });
            });


            app.Map("/info", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    string strResponse = "<HTML><HEAD><TITLE>Информация</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Информация:</H1>";
                    strResponse += "<BR> Сервер: " + context.Request.Host;
                    strResponse += "<BR> Путь: " + context.Request.PathBase;
                    strResponse += "<BR> Протокол: " + context.Request.Protocol;
                    strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";
                    await context.Response.WriteAsync(strResponse);
                });
            });

            app.Map("/Counters", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    ICachedCountersService cachedCountersService = context.RequestServices.GetService<ICachedCountersService>();
                    IEnumerable<Counter> counters = cachedCountersService.GetCounters("Counters20");
                    string HtmlString = "<HTML><HEAD><TITLE>Емкости</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список счетчики</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Регистрационный номер</TH>";
                    HtmlString += "<TH>Модель</TH>";
                    HtmlString += "<TH>Время установки</TH>";
                    HtmlString += "<TH>Организация</TH>";
                    HtmlString += "</TR>";
                    foreach (var counter in counters)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + counter.RegistrationNumber + "</TD>";
                        HtmlString += "<TD>" + counter.Model.ModelName + "</TD>";
                        HtmlString += "<TD>" + counter.TimeOfInstallation + "</TD>";
                        HtmlString += "<TD>" + counter.Organization.OrgName + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/Counters'>Емкости</A></BR>";
                    HtmlString += "<BR><A href='/form'>Данные пользователя</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                });
            });


            app.Run((context) =>
            {
                ICachedCountersService cachedCountersService = context.RequestServices.GetService<ICachedCountersService>();
                cachedCountersService.AddCounters("Counters20");
                string HtmlString = "<HTML><HEAD><TITLE>Счетчики</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY><H1>Главная</H1>";
                HtmlString += "<H2>Данные записаны в кэш сервера</H2>";
                HtmlString += "<BR><A href='/'>Главная</A></BR>";
                HtmlString += "<BR><A href='/info'>Информация о клиенте</A></BR>";
                HtmlString += "<BR><A href='/Counters'>Счетчики</A></BR>";
                HtmlString += "<BR><A href='/form1'>Форма 1</A></BR>";
                HtmlString += "<BR><A href='/form2'>Форма 2</A></BR>";
                HtmlString += "</BODY></HTML>";

                return context.Response.WriteAsync(HtmlString);

            });

            app.Run();
        }
    }
}