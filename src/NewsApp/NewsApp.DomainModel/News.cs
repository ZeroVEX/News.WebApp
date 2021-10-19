using System;

namespace NewsApp.DomainModel
{
    public class News
    {
        public const int TitleMaxLength = 4000;
        public const int SubtitleMaxLength = 4000;
        public const int TextMaxLength = 4000;


        public int Id { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Text { get; set; }

        public DateTime ChangeDate { get; set; }

        public byte[] ImageData { get; set; }

        public int? CreatorId { get; set; }

        public User Creator { get; set; }
    }
}