namespace SchoolManagementSystem.Models
{
    public class Student
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public int AdminId { get; set; }
        public int parentId { get; set; }
        public string ParentRelation { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? ExiteDate { get; set; }
        public bool IsGraduated { get; set; }

        // One-To-One
        public virtual User User { get; set; }
        // Many-To-One
        public virtual Parent Parent { get; set; }
        public virtual Admin Admin { get; set; }
        // One-To-Many
        public virtual ICollection<StudentClassEnrollment> StudentClassEnrollments { get; set; } 
        public virtual ICollection<StudentExamDegree> StudentExamDegrees { get; set; }
        public virtual ICollection<StudentsSubjectsEnrollment> StudentsSubjectsEnrollments { get; set; }
        public virtual ICollection<StudentHomeworkAnswer>StudentHomeworkAnswers { get; set; }
    }
}
