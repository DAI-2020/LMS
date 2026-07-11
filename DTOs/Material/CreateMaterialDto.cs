using LMS.API.Enums.MaterialEnums;
using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.Material
{
    public class CreateMaterialDto
    {
        [Required]
        public int CourseId { get; set; }

        public int? SessionId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public MaterialType MaterialType { get; set; }

        [Required]
        public AttachmentType AttachmentType { get; set; }

        [Required]
        public string FileUrl { get; set; }
    }
}
