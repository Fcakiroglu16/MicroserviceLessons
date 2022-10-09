namespace WorkerService1.Models;

public class User
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }
    public int Age { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? UpdatedDateTime { get; set; }
}