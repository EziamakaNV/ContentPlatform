using Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newsletter.Reporting.Database;
using Newsletter.Reporting.Entities;

namespace Newsletter.Reporting.Features.Articles
{
    public sealed class ArticleViewedConsumer : IConsumer<ArticleViewedEvent>
    {
        private readonly ApplicationDbContext _dbContext;

        public ArticleViewedConsumer(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<ArticleViewedEvent> context)
        {
            var article = await _dbContext
                .Articles
                .FirstOrDefaultAsync(article => article.Id == context.Message.Id);

            // received the view event out of order
            // or article event wasn't processed
            if (article is null)
            {
                return;
            }

            var articleEvent = new ArticleEvent
            {
                Id = Guid.NewGuid(),
                ArticleId = article.Id,
                CreatedOnUtc = context.Message.ViewedOnUtc,
                EventType = ArticleEventType.View
            };

            _dbContext.ArticleEvents.Add(articleEvent);

            await _dbContext.SaveChangesAsync();
        }
    }
}
