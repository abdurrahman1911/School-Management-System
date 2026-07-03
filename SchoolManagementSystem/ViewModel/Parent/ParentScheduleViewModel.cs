using System.Collections.Generic;

namespace SchoolManagementSystem.ViewModel.Parent
{
    public class ParentScheduleViewModel
    {
        public string ParentFullName { get; set; }
        public string ParentFirstLetter { get; set; }
        public List<ChildScheduleInfo> Children { get; set; }
    }

    public class ChildScheduleInfo
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string GradeName { get; set; }
        public string SchedulePhotoUrl { get; set; }
    }
}
