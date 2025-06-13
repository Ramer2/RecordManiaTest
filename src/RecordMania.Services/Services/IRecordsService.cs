using RecordMania.Services.DTOs;

namespace RecordMania.Services.Services;

public interface IRecordsService
{
    public Task<List<GetAllRecordsDto>> GetAllRecords(CancellationToken cancellationToken);

    public Task<bool> CreateRecord(CreateRecordWithoutTask newRecord, CancellationToken cancellationToken);
}