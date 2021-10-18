namespace NewsApp.Foundation.UsersServices
{
    public enum RegistrationError
    {
        UnknownError,
        InvalidEmail,
        PasswordTooShort,
        PasswordTooLong,
        DisplayNameTooLong,
        DuplicateEmail
    }
}