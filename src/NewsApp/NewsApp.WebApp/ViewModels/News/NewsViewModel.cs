using System;

namespace NewsApp.WebApp.ViewModels.News
{
    public class NewsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Creator { get; set; }

        public DateTime ChangeDate { get; set; }
    }
}