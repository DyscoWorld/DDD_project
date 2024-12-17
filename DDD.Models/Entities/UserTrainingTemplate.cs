namespace DDD.Models.Entities;

/// <summary>
/// Временная модель пользователя
/// </summary>
public class UserTrainingTemplate
{
    public string Id { get; set; } // MongoDB автоматически создает ObjectId, можно использовать string или ObjectId
    public string UserId { get; set; }
    public DateTime ScheduledTrainingTime { get; set; }
}
