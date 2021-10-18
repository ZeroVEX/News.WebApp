using System.ComponentModel.DataAnnotations;

namespace NewsApp.WebApp.ViewModels.News
{
    public class NewsManagementViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(DomainModel.News.TitleMaxLength)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(DomainModel.News.SubtitleMaxLength)]
        [Display(Name = "Subtitle")]
        public string Subtitle { get; set; }

        [Required]
        [StringLength(DomainModel.News.TextMaxLength)]
        [Display(Name = "Text")]
        public string Text { get; set; }

        public string Action { get; set; }
    }
}