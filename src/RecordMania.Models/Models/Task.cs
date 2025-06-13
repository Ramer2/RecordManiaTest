namespace RecordMania.Models.Models;

public class Task
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public virtual ICollection<Record> Records { get; set; } = new List<Record>();
}