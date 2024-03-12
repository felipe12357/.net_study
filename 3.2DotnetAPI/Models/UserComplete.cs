namespace DotnetAPI.Models {

    //partial indica q esta clase se puede generar apartir de varios archivos
    //cuando la aplicacion se compila todos se juntan en la misma clase
    public partial class UserComplete {
        public int UserId {get;set;}
        public string FirstName  {get;set;} = "";
        public string LastName  {get;set;} = "";
        public string Email  {get;set;} = "";
        public string? Department {get;set;}
        public int? AverageSalary {get;set;}
    }
}
