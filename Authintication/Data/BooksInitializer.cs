using Authintication.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authintication.Data
{
    public class BooksInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                if (!context.Books.Any())
                {
                    context.Books.AddRange(
                    new Book()
                    {
                        Title = "1st Book",
                        Descreption = "This is the first Book",
                        IsRead = false,
                        DateRead = DateTime.Now.AddDays(-10),
                        Gen = "Biography",
                        CoverUrl = "Http...",
                        DateAdded = DateTime.Now,
                        
                    } ,
                    new Book()
                    {
                        Title = "2nd Book",
                        Descreption = "This is the secound Book",
                        IsRead = true,
                        DateRead = DateTime.Now.AddDays(-10),
                        Gen = "Biography",
                        CoverUrl = "Http...",
                        DateAdded = DateTime.Now
                    }) ;
                    context.SaveChanges();
                }
            }
        }
    }
}
