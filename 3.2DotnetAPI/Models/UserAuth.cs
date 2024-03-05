namespace DotnetAPI.Models {

    //cuando la aplicacion se compila todos se juntan en la misma clase
    public class UserAuth {

        public string Email  {get;set;} = "";
        public byte[] PasswordHash {get;set;} = new byte[0];
        public byte[] PasswordSalt {get;set;} = new byte[0];
    }
}
