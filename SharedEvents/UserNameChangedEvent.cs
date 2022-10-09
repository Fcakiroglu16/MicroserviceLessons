using System.Reflection.Metadata.Ecma335;

namespace SharedEvents;

public class UserNameChangedEvent
{
  
    public UserNameChangedEvent(int id, string userName, DateTime createdDateTime)
    {
        UserName = userName;
        CreatedDateTime = createdDateTime;
        Id = id;
    }

    public int Id { get; set; }
    public string UserName { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public override string ToString()
    {
        return $"UserName : {UserName}";
    }
}