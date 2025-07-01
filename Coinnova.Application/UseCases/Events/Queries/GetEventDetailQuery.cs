using Coinnova.Application.Dtos.Event;
using Coinnova.Domain.Interfaces.Base;
using MediatR;

namespace Coinnova.Application.UseCases.Events.Queries;

public record GetEventDetailQuery(int EventId) : IRequest<EventDetailDto?>;

public class GetEventDetailQueryHandler : IRequestHandler<GetEventDetailQuery, EventDetailDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetEventDetailQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<EventDetailDto?> Handle(GetEventDetailQuery request, CancellationToken cancellationToken)
    {
        var ev = await _unitOfWork.EventRepository.GetEventDetailByIdAsync(request.EventId);
        if (ev == null) return null;

        return new EventDetailDto
        {
            Id = ev.Id,
            Name = ev.Name,
            Place = ev.Place,
            Description = ev.Description,
            InitialDate = ev.Initialdate,
            EndDate = ev.Enddate,
            RulesUrl = ev.Rulesurl,
            ImageUrl = ev.Imageurl
        };
    }
} 