using System;
using System.ComponentModel.DataAnnotations;

namespace Ground_Storage_WebAPI.Models
{
    public class Conflict
    {
        [Key]
        public Guid Id { get; set; } // just conflict Id - useless
        public string Entity { get; set; }
        public Guid Rev { get; set; }
        public string OfflineId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatorId { get; set; }
        public string Keys { get; set; }
        public string Body { get; set; }

        // Relation(s)
        public Guid RecordId { get; set; }
        public Record Record { get; set; }
    }
}