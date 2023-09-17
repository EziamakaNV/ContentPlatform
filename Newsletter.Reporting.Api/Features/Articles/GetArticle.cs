using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newsletter.Reporting.Database;
using Newsletter.Reporting.Entities;
using Newsletter.Reporting.Shared.Exceptions;

namespace Newsletter.Reporting.Features.Articles
{
    public static class GetArticle
    {
        public class Query : IRequest<ArticleResponse>
        {
            public Guid Id { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Query, ArticleResponse>
        {
            private readonly ApplicationDbContext _dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<ArticleResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var articlesResponse = await _dbContext
                    .Articles
                    .AsNoTracking()
                    .Where(article => article.Id == request.Id)
                    .Select(article => new ArticleResponse
                    {
                        Id = article.Id,
                        CreatedOnUtc = article.CreatedOnUtc,
                        PublishedOnUtc = article.PublishedOnUtc,
                        Events = _dbContext
                        .ArticleEvents
                        .Where(articleEvent => articleEvent.ArticleId == article.Id)
                        .Select(articleEvent => new ArticleEventResponse()
                        {
                            Id = articleEvent.Id,
                            EventType = articleEvent.EventType,
                            CreatedOnUtc = articleEvent.CreatedOnUtc
                        })
                        .ToList()
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (articlesResponse is null)
                    throw new ArticleNotFoundException(request.Id);

                return articlesResponse;
            }
        }
    }

    public class GetArticleEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/articles/{Id:guid}", async(Guid Id, ISender sender) =>
            {
                var query = new GetArticle.Query { Id = Id };

                var result = await sender.Send(query);

                return result;
            });
        }
    }

    public class ArticleResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? PublishedOnUtc { get; set; }
        public List<ArticleEventResponse> Events { get; set; } = new();
    }

    public class ArticleEventResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public ArticleEventType EventType { get; set; }
    }
}
