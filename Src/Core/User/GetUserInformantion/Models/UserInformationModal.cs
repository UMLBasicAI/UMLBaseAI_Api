namespace GetUserInformation.Models;

public sealed class UserInformationModal
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public bool IsEmailConfirmed { get; set; }
    

}
