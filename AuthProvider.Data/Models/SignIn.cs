﻿using System.ComponentModel.DataAnnotations;

namespace AuthProvider.Data.Models;

public class SignIn
{
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
