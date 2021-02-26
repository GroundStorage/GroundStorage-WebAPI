using System;

namespace Ground_Storage_WebAPI.Models
{
    public class Record_Upsert
    {
        public string Entity { get; set; }
        public Guid? Id { get; set; } // For update only
        public Guid? Rev { get; set; } // For update only
        public Guid OfflineId { get; set; }
        public Guid CreatorId { get; set; }
        public string Keys { get; set; }
        public string Body { get; set; }
        public Guid[] Conflicts { get; set; }
    }
}