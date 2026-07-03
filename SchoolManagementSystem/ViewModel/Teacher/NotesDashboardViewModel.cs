namespace SchoolManagementSystem.ViewModel.Teacher
{
    public class NotesDashboardViewModel
    {
        public int LevelId { get; set; }
        public int ClassId { get; set; }
        public int StageId { get; set; }  

        public List<NotesPageViewModel> Notes { get; set; } = new List<NotesPageViewModel>();
    }
    public class StudentNoteItemViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string Note { get; set; }
    }
    public class NotesPageViewModel
    {
        public List<StudentNoteItemViewModel> Students { get; set; } = new();
        public List<IdNameViewModel> Stages { get; set; } = new();
        public List<IdNameViewModel> Levels { get; set; } = new();
        public List<IdNameViewModel> Classes { get; set; } = new();
        public NavigationViewModel NavigationInfo { get; set; } = new();

    }
}
