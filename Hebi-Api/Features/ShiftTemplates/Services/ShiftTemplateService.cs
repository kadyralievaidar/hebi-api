using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.ShiftTemplates.Dtos;

namespace Hebi_Api.Features.ShiftTemplates.Services;

public class ShiftTemplateService : IShiftTemplateService
{
    private readonly IUnitOfWork _unitOfWork;

    public ShiftTemplateService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task CreateShiftTemplate(CreateShiftTemplateDto dto)
    {
        var shiftTemplate = new ShiftTemplate()
        {
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Name = dto.Name
        };
        await _unitOfWork.ShiftTemplateRepository.InsertAsync(shiftTemplate);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteShiftTemplate(Guid id)
    {
        var shiftTempalte = _unitOfWork.ShiftTemplateRepository.GetById(id);
        _unitOfWork.ShiftTemplateRepository.Delete(shiftTempalte!);
        await _unitOfWork.SaveAsync();
    }

    public async Task<ShiftTemplateDto> GetShiftTemplateById(Guid id)
    {
        var shiftTemplate = await _unitOfWork.ShiftTemplateRepository.GetByIdAsync(id);

        var shiftTempalteDto = new ShiftTemplateDto()
        {
            Id = shiftTemplate!.Id,
            Name = shiftTemplate.Name,
            StartTime = shiftTemplate.StartTime,
            EndTime = shiftTemplate.EndTime
        };
        return shiftTempalteDto;
    }

    public async Task<PagedResult<ShiftTemplateDto>> GetShiftTemplates(GetPagedListOfShiftsTemplatesDto dto)
    {
        var templates = await _unitOfWork.ShiftTemplateRepository.SearchAsync(x => true, dto.SortBy,
                                dto.SortDirection, dto.PageSize * dto.PageIndex, dto.PageSize);

        var results = templates.Select(x => new ShiftTemplateDto()
        {
            EndTime = x.EndTime,
            StartTime = x.StartTime,
            Id = x.Id,
            Name = x.Name,
        }).ToList();

        var count = _unitOfWork.ShiftTemplateRepository.Count();

        var result = new PagedResult<ShiftTemplateDto>()
        {
            Results = results,
            TotalCount = count
        };
        return result;
    }

    public async Task UpdateShiftTempalate(Guid id, CreateShiftTemplateDto templateDto)
    {
        var shiftTemplate = await _unitOfWork.ShiftTemplateRepository.GetByIdAsync(id);
        shiftTemplate!.EndTime = templateDto.EndTime;
        shiftTemplate.StartTime = templateDto.StartTime;
        shiftTemplate.Name = templateDto.Name;
        _unitOfWork.ShiftTemplateRepository.Update(shiftTemplate);
        await _unitOfWork.SaveAsync();
    }
}
