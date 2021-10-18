namespace NewsApp.WebApp.ViewModels
{
    public class DialogViewModel
    {
        public int Id { get; set; }

        public string ActionUrl { get; }


        public DialogViewModel(int id, string actionUrl)
        {
            Id = id;
            ActionUrl = actionUrl;
        }
    }
}