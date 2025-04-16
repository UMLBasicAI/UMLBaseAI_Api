namespace UpdatePromptTitleNoteById.Models;

public sealed class UserInformationModal
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsEmailConfirmed { get; set; }

    public AdditionalUserInforModel AdditionalUserInfor { get; set; }

    public sealed class AdditionalUserInforModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
