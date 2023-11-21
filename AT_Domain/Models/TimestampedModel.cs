namespace AT_Domain.Models
{
    public abstract class TimestampedModel : BaseModel
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
