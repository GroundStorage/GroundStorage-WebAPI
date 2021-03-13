using System;

namespace Ground_Storage_WebAPI.Models
{
    public class Record_Search
    {
        public string Entity { get; set; }
        public Guid? Id { get; set; }
        public Guid? Rev { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatorId { get; set; }
        public string KeySearch { get; set; }
        public KeySearchType KeySearchType { get; set; }
    }
}