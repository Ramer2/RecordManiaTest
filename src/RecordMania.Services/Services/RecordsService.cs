using System.Globalization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RecordMania.Services.Context;
using RecordMania.Services.DTOs;
using RecordMania.Models;
using RecordMania.Models.Models;
using Task = RecordMania.Models.Models.Task;

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

    public async Task<bool> CreateRecord(CreateRecordWithoutTask newRecord, CancellationToken cancellationToken)
    {
        try
        {
            var format = "MM/dd/yyyy HH:mm:ss";
            
            var language = await _context.Languages.FirstOrDefaultAsync(l => l.Id == newRecord.LanguageId, cancellationToken);
            if (language == null)
                throw new KeyNotFoundException($"Language {newRecord.LanguageId} not found");
            
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == newRecord.StudentId, cancellationToken);
            if (student == null)
                throw new KeyNotFoundException($"Student {newRecord.StudentId} not found");
            
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == newRecord.TaskId, cancellationToken);
            if (task == null)
            {
                if (newRecord.Task == null)
                    throw new KeyNotFoundException($"Task {newRecord.TaskId} not found and no body provided for a new one.");
                
                var newTask = JsonDocument.Parse((newRecord.Task).ToString()).RootElement;
                
                if (!newTask.TryGetProperty("name", out var nameProperty))
                    throw new ArgumentException($"New task name not provided.");
                
                if (!newTask.TryGetProperty("description", out var descriptionProperty))
                    throw new ArgumentException($"New task description not provided.");
                
                if (nameProperty.GetString() == null)
                    throw new ArgumentException($"New task name is illegal.");
                
                if (descriptionProperty.GetString() == null)
                    throw new ArgumentException($"New task description is illegal.");
                
                task = new Task
                {
                    Id = newRecord.TaskId,
                    Name = nameProperty.GetString(),
                    Description = descriptionProperty.GetString()
                };
                
                await _context.Tasks.AddAsync(task, cancellationToken);
            }

            var record = new Record
            {
                Language = language,
                Student = student,
                Task = task,
                ExecutionTime = Convert.ToInt32(newRecord.ExecutionTime),
                CreatedAt = DateTime.ParseExact(newRecord.Created, format, CultureInfo.InvariantCulture)
            };
            
            await _context.Records.AddAsync(record, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (ArgumentException ex)
        {
            throw;
        }
        catch (KeyNotFoundException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}