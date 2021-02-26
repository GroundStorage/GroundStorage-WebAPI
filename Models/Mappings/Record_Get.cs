using System;

namespace Ground_Storage_WebAPI.Models
{
    public class Record_Get
    {
        public string Entity { get; set; }
        public Guid Id { get; set; }
        public Guid Rev { get; set; }
        public Guid OfflineId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid CreatorId { get; set; }
        public string Keys { get; set; }
        public string Body { get; set; }
        public Guid[] Conflicts { get; set; }
    }
}