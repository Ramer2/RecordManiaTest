using Microsoft.AspNetCore.Mvc;
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
}