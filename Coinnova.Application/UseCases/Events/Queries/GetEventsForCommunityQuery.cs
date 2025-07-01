using Coinnova.Application.Dtos.Event;
using Coinnova.Domain.Interfaces.Base;
using MediatR;

namespace Coinnova.Application.UseCases.Events.Queries;

public record GetEventsForCommunityQuery(int CommunityId, int Skip, int? Take = null) : IRequest<IEnumerable<EventPreviewDto>>;

public class GetEventsForCommunityQueryHandler : IRequestHandler<GetEventsForCommunityQuery, IEnumerable<EventPreviewDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetEventsForCommunityQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EventPreviewDto>> Handle(GetEventsForCommunityQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.EventRepository.GetEventsForCommunitySources(request.CommunityId, request.Skip, request.Take);

        if (request.Skip > 0) result = result.Skip(request.Skip);
        if (request.Take.HasValue) result = result.Take(request.Take.Value);
        
        return result.Cast<EventPreviewDto>();
    }
}