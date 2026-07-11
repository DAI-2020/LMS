namespace LMS.API.Enums.TasksEnums
{
    public enum AssignmentStatus
    {
        Pending = 1,     // معلق / لسه الطالب مرفعش الواجب والـ Deadline شغال
        Submitted = 2,   // تم التسليم بنجاح ولسه تحت المراجعة
        Reviewed = 3,    // تم المراجعة والتصحيح ورصد الدرجة
        Missed = 4      // فات الميعاد والطالب مسلمش الواجب
    }
}
