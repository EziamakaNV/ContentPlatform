using Carter;
using Contracts;
using FluentValidation;
using Mapster;
using MassTransit;
using MediatR;
using Newsletter.Api.Contracts;
using Newsletter.Api.Database;
using Newsletter.Api.Entities;
using Newsletter.Api.Shared.Exceptions;

namespace Newsletter.Api.Features.Articles
{
    public static class CreateArticle
    {
        public class Command : IRequest<Guid>
        {
            public string Title { get; set; } = string.Empty;
            public string Content { get; set; } = string.Empty;
            public List<string> Tags { get; set; } = new();

        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Title).NotEmpty();
                RuleFor(c => c.Content).NotEmpty();
            }
        }

        internal sealed class Handler : IRequestHandler<Command, Guid>
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly IValidator<Command> _validator;
            private readonly IPublishEndpoint _publishEndpoint;

            public Handler(ApplicationDbContext dbContext, IValidator<Command> validator,
                IPublishEndpoint publishEndpoint)
            {
                _dbContext = dbContext;
                _validator = validator;
                _publishEndpoint = publishEndpoint;
            }
            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);

                if (!validationResult.IsValid)
                {
                    throw new CreateArticleValidationException("validation failed",
                        validationResult.Errors.Select(x => x.ErrorMessage));
                }


                var article = new Article
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    Content = request.Content,
                    Tags = request.Tags,
                    CreatedOnUtc = DateTime.UtcNow
                };

                _dbContext.Articles.Add(article);

                await _dbContext.SaveChangesAsync(cancellationToken);

                await _publishEndpoint.Publish(new ArticleCreatedEvent
                {
                    Id = article.Id,
                    CreatedOnUtc = article.CreatedOnUtc,
                }, cancellationToken);

                return article.Id;
            }
        }


    }

    public class CreateArticleEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/articles", async (CreateArticleRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateArticle.Command>();

                var articleId = await sender.Send(command);

                return Results.Ok(articleId);
            });
        }
    }
}
