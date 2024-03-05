namespace DotnetAPI.Dtos {
    public partial class UserRegistrationDto{
        public string Email {get;set;} = "";
        public string Password {get;set;} = "";
        public string PasswordConfirmation {get;set;} = "";
        public string FirstName {get; set;} = "";
        public string LastName {get; set;} = "";
        public string Gender {get; set;} = "";
    }
}