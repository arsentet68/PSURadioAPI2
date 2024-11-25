using System;
using System.Collections.Generic;

namespace PSURadioAPI2;

public partial class Message
{
    public int Id { get; set; }

    public string? Text { get; set; }

    public string? Sender { get; set; }

    public DateTime? Timestamp { get; set; }

    public byte[]? Senderprofilepic { get; set; }
}
