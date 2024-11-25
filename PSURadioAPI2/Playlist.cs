using System;
using System.Collections.Generic;

namespace PSURadioAPI2;

public partial class Playlist
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public List<string> Songs { get; set; } = null!;

    public byte[]? Image { get; set; }

    public DateTime? Date { get; set; }

    public string? Link { get; set; }
}
