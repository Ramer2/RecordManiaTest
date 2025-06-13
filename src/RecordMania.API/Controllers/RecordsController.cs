using Microsoft.AspNetCore.Mvc;
using RecordMania.Services.DTOs;
using RecordMania.Services.Services;

namespace RecordMania.API.Controllers;

[Route("api/records/[controller]")]
[ApiController]
public class RecordsController
{
    private readonly IRecordsService _service;

    public RecordsController(IRecordsService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("/api/records")]
    public async Task<IResult> GetAllRecords(CancellationToken cancellationToken)
    {
        try
        {
            var records = await _service.GetAllRecords(cancellationToken);
            if (records.Count == 0)
                return Results.NotFound();
            
            return Results.Ok(records);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    [HttpPost]
    [Route("/api/records")]
    public async Task<IResult> CreateRecord(CreateRecordWithoutTask createRecordWithoutTask, CancellationToken cancellationToken)
    {
        try
        {
            await _service.CreateRecord(createRecordWithoutTask, cancellationToken);
            return Results.Created();
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            throw;
            return Results.Problem(ex.Message);
        }
    }
}