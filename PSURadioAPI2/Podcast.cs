using System;
using System.Collections.Generic;

namespace PSURadioAPI2;

public partial class Podcast
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Text { get; set; }

    public byte[] Audio { get; set; } = null!;

    public byte[]? Image { get; set; }

    public DateTime? Date { get; set; }
}
