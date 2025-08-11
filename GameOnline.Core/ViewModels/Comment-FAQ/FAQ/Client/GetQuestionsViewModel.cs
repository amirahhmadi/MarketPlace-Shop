namespace GameOnline.Core.ViewModels.Comment_FAQ.FAQ.Client
{
    public class GetQuestionsViewModel
    {
        public int QuestionId { get; set; }
        public string UserName { get; set; }
        public string QuestionText { get; set; }
        public string CreationDate { get; set; }
        public GetFAQAnswersViewModel? getFAQAnswer { get; set; }
    }
}
