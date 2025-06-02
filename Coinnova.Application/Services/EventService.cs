using Coinnova.Application.Common.Files;
using Coinnova.Application.Common.Helpers;
using Coinnova.Application.Dtos.Event;
using Coinnova.Application.Dtos.User;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using Coinnova.Domain.Interfaces.Common;
using Mapster;
using MapsterMapper;

namespace Coinnova.Application.Services;

public class EventService : IEventService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICloudStorageService _cloudStorage;
    private readonly FileUploadFactory _fileUploadFactory;


    public EventService(IUnitOfWork unitOfWork, ICloudStorageService cloudStorage,
        FileUploadFactory fileUploadFactory)
    {
        _unitOfWork = unitOfWork;
        _cloudStorage = cloudStorage;
        _fileUploadFactory = fileUploadFactory;
    }

    public async Task<IEnumerable<EventPreviewDto>> GetEventsForCommunityAsync(int communityId, int skip, int? take = null)
    {
        var result = await _unitOfWork.EventRepository.GetEventsForCommunitySources(communityId, skip, take);

        if (skip > 0) result = result.Skip(skip);
        if (take.HasValue) result = result.Take(take.Value);
        
        return result.Cast<EventPreviewDto>();
    }

    public async Task<EventDetailDto?> GetEventDetailAsync(int eventId)
    {
        var ev = await _unitOfWork.EventRepository.GetEventDetailByIdAsync(eventId);
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

    public async Task<EventDto> CreateEvent(CreateEventDto eventDto)
    {

        int countUsageOfEvent = 0;
        
        var eventEntity = new Event
        {
            Initialdate = eventDto.Initialdate,
            Enddate = eventDto.Enddate,
            Place = eventDto.Place,
            Name = eventDto.Name,
            Description = eventDto.Description,
            Createdby = eventDto.Createdby,
            VisibilityPrivate = false
        };

        await _unitOfWork.EventRepository.Add(eventEntity);
        await _unitOfWork.Complete();

        // subir imagen seleccionada
        var eventImageDto = new UploadEventImageDto
        {
            EventId = eventEntity.Id,
            Image = eventDto.Image
        };
        await UploadEventImage(eventImageDto);
        
        // subir documento seleccionado
        var eventDocumentDto = new UploadEventDocumentDto
        {
            EventId = eventEntity.Id,
            Document = eventDto.File
        };
        await UploadEventDocument(eventDocumentDto);
        
        // Logica para ingresar el evento a categorias, instituticiones o ambas

        // si se agrega a una, varias o ninguna categorias
        if (eventDto.EventCategoryIds != null)
        {
            foreach (var categoriesId in eventDto.EventCategoryIds)
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
        
        // si se agrega a una, varias o ninguna instituciones
        if (eventDto.InstitutionEventsIds != null)
        {
            foreach (var insitutionId in eventDto.InstitutionEventsIds)
            {
                var institutionEvent = new InstitutionEvent
                {
                    IdEvent = eventEntity.Id,
                    IdInstitution = insitutionId
                };

                await _unitOfWork.InstitutionEvents.Add(institutionEvent);
                await _unitOfWork.Complete();

                countUsageOfEvent += 1;
            }
        }
        
        // logica para agregar si el evento pertenece solo a una institutcion o no
        if (countUsageOfEvent == 1)
        {
            eventEntity.VisibilityPrivate = true;
            await _unitOfWork.EventRepository.Update(eventEntity);
            await _unitOfWork.Complete();
        }
        
        return eventEntity.Adapt<EventDto>();
    }

    public async Task<bool> UploadEventImage(UploadEventImageDto uploadEventImageDto)
    {
        if (uploadEventImageDto.Image == null) 
            return false;
        
        var completeImageFile = await _fileUploadFactory.FromFormFileAsync(uploadEventImageDto.Image,
            CloudinaryFolders.ForEvent(uploadEventImageDto.EventId));
        if (completeImageFile == null) 
            return false;

        var eventEntity = await _unitOfWork.EventRepository.GetById(uploadEventImageDto.EventId);

        if (eventEntity == null) 
            return false;
        
        var imageUrl = await _cloudStorage.UploadImageAsync(completeImageFile);
        eventEntity.Imageurl = imageUrl;
        await _unitOfWork.Complete();
        return true;
    }
    
    public async Task<bool> UploadEventDocument(UploadEventDocumentDto uploadEventDocumentDto)
    {
        if (uploadEventDocumentDto.Document == null) 
            return false;
        
        var completeDocumentFile = await _fileUploadFactory.FromFormFileAsync(uploadEventDocumentDto.Document,
            CloudinaryFolders.ForEvent(uploadEventDocumentDto.EventId));
        if (completeDocumentFile == null) 
            return false;

        var eventEntity = await _unitOfWork.EventRepository.GetById(uploadEventDocumentDto.EventId);

        if (eventEntity == null) 
            return false;
        
        var documentUrl = await _cloudStorage.UploadRawFileAsync(completeDocumentFile);
        eventEntity.Rulesurl = documentUrl;
        await _unitOfWork.Complete();
        return true;
    }
}