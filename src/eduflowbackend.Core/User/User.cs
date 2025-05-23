﻿namespace eduflowbackend.Core.User;

public class User : AuditableEntity
{
    public Guid Id { get; set; }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public Role Role { get; private set; }
    public string IdentityProviderId { get; private set; }
    
    
    public User(string firstName, string lastName, string email, string phoneNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        IdentityProviderId = $"NA-{GetUsername()}";
    }

    private User(string firstName, string lastName, string email, string phoneNumber, Role role)
    :this(firstName, lastName, email, phoneNumber)
    {
        Role = role;
    }

    public static User CreateAdmin(string firstName, string lastName, string email, string phoneNumber)
    {
        return new User(firstName, lastName, email, phoneNumber, Role.Admin);
    }
    
    public static User CreateInstructor(string firstName, string lastName, string email, string phoneNumber)
    {
        return new User(firstName, lastName, email, phoneNumber, Role.Instructor);
    }
    
    public static User CreateParticipant(string firstName, string lastName, string email, string phoneNumber)
    {
        return new User(firstName, lastName, email, phoneNumber, Role.Participant);
    }
    
    public string GetUsername()
    {
        return $"{FirstName.Replace(' ', '-')}-{LastName.Replace(' ', '-')}".ToLower();
    }

    public static string GetUsername(string firstname, string lastname)
    {
        return $"{firstname.Replace(' ', '-')}-{lastname.Replace(' ', '-')}".ToLower();
    }

    // // Navigation properties
    // public ICollection<Session.Session> CreatedSessions { get; set; } //Sessions created (If Instructor)
    // public ICollection<Session.Session> AttendedSessions { get; set; } //Sessions joined (If Participant)
    // public ICollection<Resource.Resource> AddedResources { get; set; } //Resources added by the User
}