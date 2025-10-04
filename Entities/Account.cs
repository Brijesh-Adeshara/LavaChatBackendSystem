namespace LavaChatBackend.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public long FacebookId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? Birthday { get; set; }

        public string? ProfilePictureUrl { get; set; }

    }
}
