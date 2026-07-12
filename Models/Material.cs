using LMS.API.Enums.MaterialEnums;

namespace LMS.API.Models;

public class Material
{
    public int Id { get; set; }

    public int CourseId { get; set; }
    public int? SessionId { get; set; }
    public string Title { get; set; }

    public MaterialType MaterialType { get; set; }
    public AttachmentType AttachmentType { get; set; }
    public string FileUrl { get; set; }
    
    //internal relationship
    public Course Course { get; set; }
    
    //external relationships
    public virtual LiveSession? LiveSession { get; set; }
}