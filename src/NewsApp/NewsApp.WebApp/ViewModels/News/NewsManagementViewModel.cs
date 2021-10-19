using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApp.WebApp.Controllers;
using System.ComponentModel.DataAnnotations;

namespace NewsApp.WebApp.ViewModels.News
{
    public class NewsManagementViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "RequiredField")]
        [StringLength(DomainModel.News.TitleMaxLength, ErrorMessage = "MaxLength")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "RequiredField")]
        [StringLength(DomainModel.News.SubtitleMaxLength, ErrorMessage = "MaxLength")]
        [Display(Name = "Subtitle")]
        public string Subtitle { get; set; }

        [Required(ErrorMessage = "RequiredField")]
        [StringLength(DomainModel.News.TextMaxLength, ErrorMessage = "MaxLength")]
        [Display(Name = "Text")]
        public string Text { get; set; }

        [Display(Name = "Image")]
        public IFormFile Image { get; set; }

        public byte[] ImageData { get; set; }

        public string Action { get; set; }
    }
}