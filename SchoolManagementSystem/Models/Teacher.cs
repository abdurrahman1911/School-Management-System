namespace SchoolManagementSystem.Models
{
    public class Teacher
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public int AdminId { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? ExiteDate { get; set; }

        // One-To-One
        public virtual User User { get; set; }
        public virtual TeacherTimeTable TeacherTimeTable { get; set; }
        // One-To-Many
        public virtual ICollection<Homework>Homeworks { get; set; }
        public virtual ICollection<TeacherSubject>TeacherSubjects { get; set; }
        public virtual ICollection<MultiChoiceExam> MultiChoiceExam { get; set; }
        public virtual ICollection<StudentsSubjectsEnrollment> StudentsSubjectsEnrollments { get; set; }
        public virtual ICollection<ExtraSubjectMaterial> ExtraSubjectsMaterials { get; set; }
        // Many-To-One
        public virtual Admin Admin { get; set; }
    }
}
