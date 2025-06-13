namespace RecordMania.Services.DTOs;

public class GetAllRecordsDto
{
    public int Id { get; set; }
    
    public LanguageDto Language { get; set; }
    
    public TaskDto Task { get; set; }
    
    public StudentDto Student { get; set; }
    
    public long ExecutionTime { get; set; }
    
    public string Created { get; set; }
}