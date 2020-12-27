
namespace MyBlog.Entities.Messages
{
    public enum ErrorMessageCode
    {
        UsernameAlreadyExists = 101,
        EmailAlreadyExists = 102,
        UserIsNotActive = 151,
        UsernameOrPassWrong = 152,
        CheckYourEmail = 153,
        UserAlreadyActive = 154,
        ActivateIdDoesNotExists = 155,
        UserNotFound = 156,
        ProfileClouldNotUpdated = 157,
        UserCouldNotRemove = 158,
        UserCouldNotInserted = 159,
        UserCouldNotUpdated = 160
    }
}
