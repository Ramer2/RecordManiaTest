namespace RecordMania.Models.Models;

public class Student
{
    public int Id { get; set; }
    
    public string FirstName { get; set; } = null!;
    
    public string LastName { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    
    public virtual ICollection<Record> Records { get; set; } = new List<Record>();
}