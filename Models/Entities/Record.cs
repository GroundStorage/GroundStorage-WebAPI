using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Ground_Storage_WebAPI.Models
{
    [Index(
        "Entity",
        /* "Id", // redundant */
        "Rev",
        "OfflineId",
        "CreatedAt",
        "UpdatedAt",
        "CreatorId"
    )]
    public class Record
    {
        public string Entity { get; set; }
        [Key]
        public Guid Id { get; set; } // Auto fill
        public Guid Rev { get; set; } // Auto fill
        public Guid OfflineId { get; set; }
        public DateTime CreatedAt { get; set; } // Auto fill
        public DateTime UpdatedAt { get; set; } // Auto fill
        public Guid CreatorId { get; set; }
        public string Keys { get; set; }
        public string Body { get; set; }

        // Relation(s)
        public List<Conflict> Conflicts { get; set; }
    }
}