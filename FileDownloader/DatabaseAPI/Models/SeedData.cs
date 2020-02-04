using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseAPI.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new VitunDatabaseContext(serviceProvider.GetRequiredService<DbContextOptions<VitunDatabaseContext>>()))
            {
                if (context.Authors.Any())
                {
                    return;
                }
                context.Authors.AddRange(
                    new Author { Name = "Phong", Birthplace = "Saigon" },
                    new Author { Name = "Toni", Birthplace = "Juva" }
                );
                
                if (context.Questions.Any())
                {
                    return;
                }
                context.Questions.AddRange(
                    new Question { Title = "Is this Toni?", Content = "I am Phong", AuthorId = 1 },
                    new Question { Title = "Is this Phong?", Content = "I am Toni", AuthorId = 2 }
                );
                context.SaveChanges();
            }
        }
    }
}
