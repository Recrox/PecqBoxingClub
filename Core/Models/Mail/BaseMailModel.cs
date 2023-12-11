namespace PecqBoxingClubApi.BackEnd.Core.Models.Mail
{
    public class BaseMailModel
    {
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string ProjectName { get; set; }
    }

    public class OrderMailModel : BaseMailModel
    {
        public string Identifier { get; set; }
        public string CreatedOn { get; set; }
        public string Site { get; set; }
        public string Sender { get; set; }
        public string SenderAddress { get; set; }
        public string SenderService { get; set; }
        public string Receiver { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverService { get; set; }
        public string TransportTypes { get; set; }
        public string Urgency { get; set; }
        public string OrdererName { get; set; }
        public string OrdererPhone { get; set; }
        public string OrdererComment { get; set; }
        public string DispatcherComment { get; set; }
        public string StartDate { get; set; }
    }

    public class OrderOkMailModel : OrderMailModel
    {
        public string UrlNoComment { get; set; }
        public string UrlWithComment { get; set; }
    }

    public class ContactMailModel : BaseMailModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
    }
}
