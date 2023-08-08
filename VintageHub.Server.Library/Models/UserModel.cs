using System.ComponentModel.DataAnnotations;

namespace VintageHub.Server.Library.Models;
public class UserModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(36)]
    public string ObjectIdentifier { get; set; }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }

    [Required]
    [StringLength(100)]
    public string DisplayName { get; set; }

    [Required]
    [StringLength(320)]
    public string EmailAddress { get; set; }

    [Required]
    [StringLength(200)]
    public string Address { get; set; }
}
