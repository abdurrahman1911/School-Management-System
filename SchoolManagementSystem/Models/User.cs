using Microsoft.SqlServer.Server;
using SchoolManagementSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string? SecondName { get; set; }
    public string? ThirdName { get; set; }
    public string LastName { get; set; }
    [NotMapped]
    public string FullName => $"{FirstName} {SecondName} {ThirdName} {LastName}".Replace("  ", " ").Trim();
    public string Phone { get; set; }
    public string? Email { get; set; }
    public string Password { get; set; }
    public string? ProfilPhotoURL { get; set; }
    public string SSN { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime AddedDate { get; set; }
    public string Governorate { get; set; }
    public string City { get; set; }
    public string? Street { get; set; }
    public string? Area { get; set; }
    public bool Gender { get; set; }
    public string Nationality { get; set; }

    // One-to-One 
    public virtual Student Student { get; set; }        
    public virtual Parent Parent { get; set; }          
    public virtual Teacher Teacher { get; set; }       
    public virtual Supervisor Supervisor { get; set; }  
    public virtual Owner Owner { get; set; }            
    public virtual Headmaster Headmaster { get; set; }  
    public virtual Admin Admin { get; set; }

    // Many-To-One
    
    // One-to-Many

    public virtual ICollection<Log> Logs { get; set; }      
    public virtual ICollection<Note> WrittenNotes { get; set; } 
    public virtual ICollection<Note> ReceivedNotes { get; set; } 
    public virtual ICollection<Absence> Absences { get; set; }
    public virtual ICollection<UserUserType> UserUserTypes { get;set; }

    
}
