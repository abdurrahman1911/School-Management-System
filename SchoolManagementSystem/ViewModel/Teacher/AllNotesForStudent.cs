namespace SchoolManagementSystem.ViewModel.Teacher
{
    public class AllNotesForStudent
    {
        public string StudentName { get; set; }
        public List<NoteItem> Notes { get; set; } = new();
    }
    public class NoteItem
    {
        public int Id { get; set; }
        public string Note { get; set; }
    }
}
