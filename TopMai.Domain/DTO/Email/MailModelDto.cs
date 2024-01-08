namespace TopMai.Domain.DTO.Email
{
    public class MailModelDto
    {
        public List<EmailAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool IsBodyHtml { get; set; }
        public List<string> Files { get; set; }
    }
    public class EmailAddress
    {
        public string DisplayName { get; set; }
        public string Addres { get; set; }
    }
}
