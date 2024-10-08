namespace api.DTOs.Chat;

public class RoomDTO
{
    public string? Id { get; set; }
    public string RoomName { get; set; } = null!;
    public List<int> Members { get; set; } = new List<int>();
    public List<MessageDTO> Messages { get; set; } = new List<MessageDTO>();
    public DateTime CreatedAt { get; set; }
}
public class RoomLessDTO
{
    public string? Id { get; set; }
    public string RoomName { get; set; } = null!;
    public List<int> Members { get; set; } = new List<int>();
    public List<string> Messages { get; set; } = new List<string>();
    public DateTime CreatedAt { get; set; }
}