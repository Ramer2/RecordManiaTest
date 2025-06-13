using Microsoft.EntityFrameworkCore;
using RecordMania.Services.Context;
using RecordMania.Services.DTOs;

namespace RecordMania.Services.Services;

public class RecordsService : IRecordsService
{
    private readonly RecordManiaContext _context;

    public RecordsService(RecordManiaContext context)
    {
        _context = context;
    }

    public async Task<List<GetAllRecordsDto>> GetAllRecords(CancellationToken cancellationToken)
    {
        try
        {
            var recordDtos = new List<GetAllRecordsDto>();
            var records = await _context.Records
                .Include(r => r.Student)
                .Include(r => r.Task)
                .Include(r => r.Language)
                .OrderByDescending(r => r.CreatedAt)
                .OrderBy(r => r.Student.LastName)
                .ToListAsync(cancellationToken);

            foreach (var record in records)
            {
                var taskDto = new TaskDto
                {
                    Id = record.Task.Id,
                    Name = record.Task.Name,
                    Description = record.Task.Description,
                };
                var languageDto = new LanguageDto
                {
                    Id = record.Language.Id,
                    Name = record.Language.Name,
                };
                var studentDto = new StudentDto
                {
                    Id = record.Student.Id,
                    FirstName = record.Student.FirstName,
                    LastName = record.Student.LastName,
                    Email = record.Student.Email,
                };
                recordDtos.Add(new GetAllRecordsDto
                {
                    Id = record.Id,
                    Language = languageDto,
                    Student = studentDto,
                    Task = taskDto,
                    ExecutionTime = record.ExecutionTime,
                    Created = record.CreatedAt.ToString("MM/dd/yyyy HH:mm:ss"),
                });
            }
            
            return recordDtos;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}