using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Keeltekool_2.Core.DTO
{
    public class FileToDatabaseDTO
    {
        public Guid ImageID { get; set; }
        public string? ImageTitle { get; set; }
        public byte[]? ImageData { get; set; }
        public Guid? LecturerId { get; set; }
    }
}
