using Microsoft.EntityFrameworkCore;
using ModusCreate.Core.DAL;
using ModusCreate.Core.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ModusCreate.Core.Infrastructure
{
    public interface IInstaller
    {
        Task Install();
    }

    class DbMigrationInstaller : IInstaller
    {
        private readonly NewsFeedContext _context;

        public DbMigrationInstaller(NewsFeedContext context)
        {
            _context = context;
        }

        public async Task Install()
        {
            await _context.Database.MigrateAsync();
        }
    }

    class SeedDataInstaller : IInstaller
    {
        private readonly NewsFeedContext _context;
        private readonly string[] Tags = new string[] { "money", "crypto", "usa", "world", "opinion", "kings", "general", "agenda", "futbol", "basket", "emergency", "startup", "no idea" };
        Random random = new Random();

        public SeedDataInstaller(NewsFeedContext context)
        {
            _context = context;
        }

        public async Task Install()
        {
            if (!await _context.Categories.AnyAsync())
            {
                await AddCategories();

                await AddNewsFeeds();
            }
        }
        private async Task AddCategories()
        {
            var categories = new string[] { "Sports", "Economy", "Politics", "Art", "Ciense", "Leisure", "Travel", "Bussiness", "Fashion", "Social", "Health" };

            foreach (var category in categories)
            {
                _context.Categories.Add(new FeedCategoryEntity
                {
                    Name = category,
                    Description = $"{category} description"
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task AddNewsFeeds()
        {
            var categories = await _context.Categories.ToListAsync();

            foreach (var category in categories)
            {
                var feed = new FeedEntity
                {
                    Category = category,
                    IsDeleted = false,
                    Name = $"Feed {category.Name} 1",
                    Description = $"Feed {category.Name} 1 Description",
                    News = GenerateNews()
                };

                _context.Feeds.Add(feed);

                var feed2 = new FeedEntity
                {
                    Category = category,
                    IsDeleted = false,
                    CreatedOn = DateTime.Today.AddYears(-1),
                    Name = $"Feed {category.Name} 2",
                    Description = $"Feed {category.Name} 2 Description",
                    News = GenerateNews()
                };

                _context.Feeds.Add(feed2);
                await _context.SaveChangesAsync();
            }
        }

        private IList<NewsEntity> GenerateNews()
        {
            var result = new List<NewsEntity>();
            for (int i = 0; i < 15; i++)
            {
                result.Add(new NewsEntity
                {
                    Title = $"Feriens quique cibi suisque admonita {i}",
                    Tags = GetRandomTags(),
                    CreatedOn = new DateTime(2018, random.Next(1, 12), random.Next(1, 28)),
                    Body = @"## Nemus siluere

Lorem markdownum, deploravit dextrum et [meorum perque
urbis](http://tamen-simul.io/adcommodat.html) possederat corpore spargere non
dona me caesis ima nec: obfuit. Femineo bracchia sexangula territus. Luget telo,
ignis *in* nudaque pariter colit non aut simulatas, fervida contigit.
Retemptantem orbem stamine bracchia invisa, ire maiora venienti Echion solebat
postquam **Adoni**, quod **ire cives** tum hoc! *Aras in* cadit et exigui alter
non si castris tegumenque poscitur *feracis iube*.

    if (zebibyteScsiOnline > ctpDcimCore) {
        cybercrimeBarSpam(fontRoomSearch, input_toolbar_source - 1);
    } else {
        jsonImapTag.socialLinkedin += -2;
    }
    leopard_visual_dslam += jpeg_bare + adapterHdvReimage +
            programming_baseband;
    var schemaBounce = parameter_net;

Pervigilem neque humum infit perque, qua et simulacraque *sororis* oleis, et
exitus quidem voto et lecto erroresque. Magnoque turba, avara novi certos,
miserum, erat sublimibus eripiat: sed potes. Mundi queruntur tenet, miserande
micantes in quamquam perire mihi. Dare praedae accipit **Psophidaque manabant**
inque utrimque de inque modo, pedumque, imagine Alcyone quantoque nulloque: eram
coniugialia."

                });
            }

            return result;
        }

        private string GetRandomTags()
        {
            var count = random.Next(Tags.Length);
            var sb = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                var index = random.Next(Tags.Length);
                if (!sb.ToString().Contains(Tags[index]))
                {
                    sb.Append($"{Tags[index]} ");
                }
            }
            return sb.ToString();
        }
    }

}
