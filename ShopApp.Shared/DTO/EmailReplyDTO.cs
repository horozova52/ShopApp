public class EmailReplyDTO
{
    public string To { get; set; }      // destinatar
    public string From { get; set; }    // expeditor
    public string Subject { get; set; } // de ex: Re: Subiectul anterior
    public string Body { get; set; }    // corpul răspunsului
    public bool IsHtml { get; set; } = true;
}
