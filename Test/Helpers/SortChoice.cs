using System.ComponentModel.DataAnnotations;

namespace Test.Helpers
{
    public enum SortChoice
    {
        [Display(Name = "Спочатку молодші")]
        YoungerFirst,
        [Display(Name = "Спочатку старші")]
        OlderFirst
    }
}