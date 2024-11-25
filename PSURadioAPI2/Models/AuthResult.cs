﻿namespace PSURadioAPI2.Models
{
    public class AuthResult
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Role { get; set; }
        public string Email { get; set; }
        public byte[]? ProfilePic { get; set; }
    }
}
