using Coinnova.Application.Common.Files;
using Coinnova.Application.Common.Helpers;
using Coinnova.Application.Dtos.Event;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using Coinnova.Domain.Interfaces.Common;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Events.Commands;

public record CreateEventCommand(CreateEventDto CreateEventDto) : IRequest<EventDto>;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICloudStorageService _cloudStorage;
    private readonly FileUploadFactory _fileUploadFactory;

    public CreateEventCommandHandler(IUnitOfWork unitOfWork, ICloudStorageService cloudStorage, FileUploadFactory fileUploadFactory)
    {
        _unitOfWork = unitOfWork;
        _cloudStorage = cloudStorage;
        _fileUploadFactory = fileUploadFactory;
    }

    public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        int countUsageOfEvent = 0;
        
        var eventEntity = new Event
        {
            Initialdate = request.CreateEventDto.Initialdate,
            Enddate = request.CreateEventDto.Enddate,
            Place = request.CreateEventDto.Place,
            Name = request.CreateEventDto.Name,
            Description = request.CreateEventDto.Description,
            Createdby = request.CreateEventDto.Createdby,
            VisibilityPrivate = false
        };

        await _unitOfWork.EventRepository.Add(eventEntity);
        await _unitOfWork.Complete();

        // Subir imagen si existe
        if (request.CreateEventDto.Image != null)
        {
            await UploadEventImage(eventEntity.Id, request.CreateEventDto.Image);
        }
        
        // Subir documento si existe
        if (request.CreateEventDto.File != null)
        {
            await UploadEventDocument(eventEntity.Id, request.CreateEventDto.File);
        }
        
        // Lógica para ingresar el evento a categorías, instituciones o ambas

        // Si se agrega a una, varias o ninguna categorías
        if (request.CreateEventDto.EventCategoryIds != null)
        {
            foreach (var categoriesId in request.CreateEventDto.EventCategoryIds)
            {
                var eventCategory = new EventCategory
                {
                    IdEvent = eventEntity.Id,
                    IdCategory = categoriesId
                };

                await _unitOfWork.EventCategories.Add(eventCategory);
                await _unitOfWork.Complete();

                countUsageOfEvent += 2;
            }
        }
        
        // Si se agrega a una, varias o ninguna instituciones
        if (request.CreateEventDto.InstitutionEventsIds != null)
        {
            foreach (var institutionId in request.CreateEventDto.InstitutionEventsIds)
            {
                var institutionEvent = new InstitutionEvent
                {
                    IdEvent = eventEntity.Id,
                    IdInstitution = institutionId
                };

                await _unitOfWork.InstitutionEvents.Add(institutionEvent);
                await _unitOfWork.Complete();

                countUsageOfEvent += 1;
            }
        }
        
        // Lógica para agregar si el evento pertenece solo a una institución o no
        if (countUsageOfEvent == 1)
        {
            eventEntity.VisibilityPrivate = true;
            await _unitOfWork.EventRepository.Update(eventEntity);
            await _unitOfWork.Complete();
        }
        
        return eventEntity.Adapt<EventDto>();
    }

    private async Task<bool> UploadEventImage(int eventId, Microsoft.AspNetCore.Http.IFormFile image)
    {
        var completeImageFile = await _fileUploadFactory.FromFormFileAsync(image,
            CloudinaryFolders.ForEvent(eventId));
        
        if (completeImageFile == null)
            return false;

        var eventEntity = await _unitOfWork.EventRepository.GetById(eventId);
        if (eventEntity == null)
            return false;

        var imageUrl = await _cloudStorage.UploadImageAsync(completeImageFile);
        eventEntity.Imageurl = imageUrl;
        await _unitOfWork.Complete();
        return true;
    }
    
    private async Task<bool> UploadEventDocument(int eventId, Microsoft.AspNetCore.Http.IFormFile document)
    {
        var completeDocumentFile = await _fileUploadFactory.FromFormFileAsync(document,
            CloudinaryFolders.ForEvent(eventId));
        
        if (completeDocumentFile == null)
            return false;

        var eventEntity = await _unitOfWork.EventRepository.GetById(eventId);
        if (eventEntity == null)
            return false;

        var documentUrl = await _cloudStorage.UploadRawFileAsync(completeDocumentFile);
        eventEntity.Rulesurl = documentUrl;
        await _unitOfWork.Complete();
        return true;
    }
} 