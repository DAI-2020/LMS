namespace LMS.API.DTOs.Attendance
{
    public class AttendanceSummaryResponseDto
    {
        public int TotalSessions { get; set; }
        public int PassedSessions { get; set; }
        public int AttendedSessions { get; set; }
        public int MissedSessions { get; set; }

        // حقل ذكي بيحسب نسبة الحضور الكلية تلقائياً عشان الفرونت إيند يرسم الدائرة المئوية فوراً
        public double AttendancePercentage
        {
            get
            {
                if (TotalSessions == 0) return 0;
                // الحساب بناءً على المحاضرات الحاضرة فعلياً من إجمالي المحاضرات
                return Math.Round(((double)AttendedSessions / TotalSessions) * 100, 2);
            }
        }
    }
}
