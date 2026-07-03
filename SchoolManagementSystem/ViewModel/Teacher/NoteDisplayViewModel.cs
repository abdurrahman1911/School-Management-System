using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel.Teacher
{
    public class NoteItemViewModel
    {


        [Display(Name = "الملاحظة")]
        public string NoteDetails { get; set; }

        [Display(Name = "تاريخ الإضافة")]
        public DateTime AddedDate { get; set; }

        [Display(Name = "كاتب الملاحظة")]
        public string WriterName { get; set; }

        public string FormattedAddedDate => AddedDate.ToString("dd/MM/yyyy");
        public NavigationViewModel NavigationInfo { get; set; } = new();


    }
    public class NoteDisplayViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; } = new();
        public List<NoteItemViewModel> Notes { get; set; } = new();
    }

}
