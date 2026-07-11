using LMS.API.Enums.MaterialEnums;

namespace LMS.API.DTOs.Material
{
    public class UpdateMaterialDto
    {
        public string Title { get; set; }

        public MaterialType MaterialType { get; set; }

        public AttachmentType AttachmentType { get; set; }

        public string FileUrl { get; set; }
    }
}
