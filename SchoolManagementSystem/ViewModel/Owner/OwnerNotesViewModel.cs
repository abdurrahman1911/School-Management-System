namespace SchoolManagementSystem.ViewModel.Owner
{
    public class OwnerNotesViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; }

        
        public OwnerNotesInfo OwnerNotesInfo {  get; set; }
    }

    public class OwnerNotesInfo
    {
        public List<NoteInfo> StudentsNotes { get; set; }

        public List<NoteInfo> TeachersNotes { get; set; }

        public List<NoteInfo> SupervisorsNotes { get; set; }
    }

    public class NoteInfo
    {
        public int TargetUserID { get; set; }

        public int WriterUserID { get; set; }
        public string TargetName { get; set; }

        public string Detail { get; set; }

        public DateTime Date { get; set; }

        public string WriterName { get; set; }
    }
}
