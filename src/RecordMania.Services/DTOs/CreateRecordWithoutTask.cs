using System.ComponentModel.DataAnnotations;

namespace RecordMania.Services.DTOs;

public class CreateRecordWithoutTask
{
    [Required]
    public int LanguageId { get; set; }

    [Required]
    public int TaskId { get; set; }
    
    public object? Task { get; set; }
    
    [Required]
    public int StudentId { get; set; }
    
    [Required]
    public double ExecutionTime { get; set; }
    
    [Required]
    public string Created { get; set; }
}