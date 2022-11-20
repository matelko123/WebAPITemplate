﻿namespace WebAPITemplate.Shared.Models.Requests.User;

public class UserUpdateRequest
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}