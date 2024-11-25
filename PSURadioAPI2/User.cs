using System;
using System.Collections.Generic;

namespace PSURadioAPI2;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Role { get; set; }

    public byte[]? ProfilePic { get; set; }
}
