using BgDeal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgDeal.Data
{
    public class DatabaseInitializer : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public DatabaseInitializer()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (context.Deals.Count() > 0)
            {
                return;
            }


            //Zadavame si nqkakwi danni v bazata

            Random rand = new Random();

            Topic sampleTopic = new Topic { Name = "Food" };
            ApplicationUser user = new ApplicationUser() { UserName = "TestUser", Email = "test@testov.com" };

            for (int i = 0; i < 10; i++)
            {
                Deal deal = new Deal();
                deal.Content = "Nqkakkyv content";
                deal.DateAdded = DateTime.Now;
                deal.HeadImage = "http://laptop.bg/system/images/14757/thumb/ThinkPad_T530.gif";
                deal.Location = "London" + i;
                deal.Price = rand.Next(50, 2000);
                deal.Title = "Title" + i;

                var votesCount = rand.Next(0, 10);

                for (int j = 0; j < votesCount; j++)
                {
                    deal.Votes.Add(new Vote { Deal = deal, VotedBy = user });
                }


            }


            context.SaveChanges();
            base.Seed(context);
        }
    }
}
