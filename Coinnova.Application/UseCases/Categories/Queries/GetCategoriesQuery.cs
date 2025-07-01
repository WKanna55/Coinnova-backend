using Coinnova.Application.Dtos.Category;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Categories.Queries;

public class GetCategoriesQuery : IRequest<IEnumerable<CategoryResponseDto>>
{
    
}

internal sealed class GetCategoriesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryResponseDto>>
{
    public async Task<IEnumerable<CategoryResponseDto>> Handle(GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await unitOfWork.Categories.GetAll();
        var response = categories.Adapt<IEnumerable<CategoryResponseDto>>();
        return response;
    }
}