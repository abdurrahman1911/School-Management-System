namespace SchoolManagementSystem.ViewModel.Owner
{
    public class OwnerNotificationsViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; }

        public List<LogInfo> Logs { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

    }

    public class LogInfo
    {
        public string ActorName { get; set; }

        public string Action {  get; set; }

        public string? Details { get; set; }

        public DateTime LogDate { get; set; }

    }
}
