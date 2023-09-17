using Carter;
using Contracts;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Contracts;
using Newsletter.Api.Database;
using Newsletter.Api.Shared.Exceptions;

namespace Newsletter.Api.Features.Articles
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
            private readonly IPublishEndpoint _publishEndpoint;

            public Handler(ApplicationDbContext dbContext, IPublishEndpoint publishEnpoint)
            {
                _dbContext = dbContext;
                _publishEndpoint = publishEnpoint;
            }

            public async Task<ArticleResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var articleResponse = await _dbContext
                    .Articles
                    .Where(article => article.Id == request.Id)
                    .Select(article => article.Adapt<ArticleResponse>())
                    .FirstOrDefaultAsync(cancellationToken);

                if (articleResponse == null)
                    throw new ArticleNotFoundException(request.Id);

                await _publishEndpoint.Publish(
                    new ArticleViewedEvent
                    {
                        Id = articleResponse.Id,
                        ViewedOnUtc = DateTime.UtcNow,
                    }, cancellationToken);

                return articleResponse;
            }

        }
    }

    

    public class GetArticleEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/articles/{Id:guid}", async (Guid Id, ISender sender) =>
            {
                var query = new GetArticle.Query { Id = Id };

                var result = await sender.Send(query);

                return result;
            });
        }
    }
}
