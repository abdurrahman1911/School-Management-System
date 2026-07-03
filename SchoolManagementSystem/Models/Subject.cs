namespace SchoolManagementSystem.Models
{
    public class Subject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public int WeeklyHours { get; set; }
        // One-To-Many
        public virtual ICollection<MultiChoiceExam> MultiChoiceExam { get; set; }
        public virtual ICollection<ExtraSubjectMaterial> ExtraSubjectMaterials { get; set; }
        public virtual ICollection<TeacherSubject> TeacherSubjects { get; set; }
        public virtual ICollection<StudentsSubjectsEnrollment> StudentsSubjectsEnrollments { get; set; }

    }
}
