﻿namespace HMS.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }

        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key to ApplicationUser
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
