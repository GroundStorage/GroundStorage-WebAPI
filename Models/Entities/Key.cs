using System;
using Microsoft.EntityFrameworkCore;

namespace Ground_Storage_WebAPI.Models
{
    [Index("Name", "RecordId")]
    public class Key
    {
        public string Name { get; set; }
        public Guid RecordId { get; set; }
    }
}