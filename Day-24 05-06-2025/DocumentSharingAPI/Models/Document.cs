using System;

namespace DocumentSharingAPI.Models
{
    public class Document
    {
        public int Id { get; set; } // Optionally used as a primary key in the database.
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
