public class EmailReplyDTO
{
    public string To { get; set; }     
    public string From { get; set; }  
    public string Subject { get; set; } 
    public string Body { get; set; }  
    public bool IsHtml { get; set; } = true;

    public List<string>? AttachmentsBase64 { get; set; }
    public List<string>? AttachmentFileNames { get; set; }
}
