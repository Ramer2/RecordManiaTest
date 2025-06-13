namespace RecordMania.Models.Models;

public class Language
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public virtual ICollection<Record> Records { get; set; } = new List<Record>();
}