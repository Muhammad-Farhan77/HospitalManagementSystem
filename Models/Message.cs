namespace HMS.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        // Foreign key for sender
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }

        // Foreign key for receiver
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }

        public bool IsRead { get; set; } = false; // Optional: to track read status
    }
}
