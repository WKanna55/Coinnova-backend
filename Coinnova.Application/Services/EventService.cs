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

    public async Task<IEnumerable<EventPreviewDto>> GetTop6EventsForCommunity(int communityId)
    {
        var result = await _unitOfWork.Events.GetTop6EventsForCommunity(communityId);
        return result.Cast<EventPreviewDto>();
    }

    public async Task<EventDto> CreateEvent(CreateEventDto eventDto)
    {
        var eventEntity = new Event
        {
            Initialdate = eventDto.Initialdate,
            Enddate = eventDto.Enddate,
            Place = eventDto.Place,
            Name = eventDto.Name,
            Description = eventDto.Description,
            Createdby = eventDto.Createdby,
            VisibilityPrivate = eventDto.VisibilityPrivate
        };

        await _unitOfWork.Events.Add(eventEntity);
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

        var eventEntity = await _unitOfWork.Events.GetById(uploadEventImageDto.EventId);

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

        var eventEntity = await _unitOfWork.Events.GetById(uploadEventDocumentDto.EventId);

        if (eventEntity == null) 
            return false;
        
        var documentUrl = await _cloudStorage.UploadRawFileAsync(completeDocumentFile);
        eventEntity.Rulesurl = documentUrl;
        await _unitOfWork.Complete();
        return true;
    }
    
    
}