using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace lesson2_handson
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Run(async (context) =>
            {
                // Get the cookie 'MyCoolLittleCookie' from the browser, if it's not there this will return an empty string
                var cookie = context.Request.Cookies["MyCoolLittleCookie"];

                string response =
                    "<h1>Query String Parameters</h1>" 
                    + "<p>Enter a URL like:</p>"
                    + "<a href=\"https://localhost:5001/?firstname=Momo&lastname=Hirai&age=24\">" 
                    + "http://localhost:5001/?firstname=Momo&lastname=Hirai&age=24</a>"
                    + "<h1>HTTP Cookies</h1>"
                    + $"<p>This is the cookie value received from browser: \"<strong>{cookie}</strong>\".</p>"
                    + "<p>Refresh page to see current cookie value...</p>" 
                    + "<p>Cookie expires after 15 seconds.</p>";

                foreach (var queryParameter in context.Request.Query) {
                    response += "<p>" + queryParameter + "</p>";
                }

                // if var cookie is an empty string this will create a cookie named 'MyCoolLittleCookie'
                if (string.IsNullOrWhiteSpace(cookie))
                {
                    DateTime now = DateTime.Now;
                    DateTime expires = now + TimeSpan.FromSeconds(15);
                    
                    // With Query Params
                    if (context.Request.QueryString.HasValue)
                    {
                        context.Response.Cookies.Append
                    (
                        "MyCoolLittleCookie",
                        "Cookie created at: " + now.ToString("h:mm:ss tt") 
                            + " params(firstname: " 
                            + context.Request.Query["firstname"]
                            + ", lastname: "
                            + context.Request.Query["lastname"]
                            + ", age: "
                            + context.Request.Query["age"]
                            + ")",
                        new CookieOptions
                        {
                            Path = "/",
                            HttpOnly = false,
                            Secure = false,
                            Expires = expires
                        }
                    );
                    }
                    // No Query Params
                    else 
                    {
                        context.Response.Cookies.Append
                    (
                        "MyCoolLittleCookie",
                        "Cookie created at: " + now.ToString("h:mm:ss tt") + " Default params(firstname: Matt, lastname: Cullen, age: 30)",
                        new CookieOptions
                        {
                            Path = "/",
                            HttpOnly = false,
                            Secure = false,
                            Expires = expires
                        }
                    );
                    }
                }
                await context.Response.WriteAsync(response);
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
