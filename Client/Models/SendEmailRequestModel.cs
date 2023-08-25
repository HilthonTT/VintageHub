namespace VintageHub.Client.Models;

public class SendEmailRequestModel
{
    [Required(ErrorMessage = "You must logged in")]
    public UserModel Sender { get; set; }

    [Required(ErrorMessage = "Please provide us your message")]
    [StringLength(2500, ErrorMessage = "The message must not be above 2500 characters")]
    public string Message { get; set; }
}
