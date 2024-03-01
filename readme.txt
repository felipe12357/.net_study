## repositorio original
    https://github.com/DomTripodi93/DotNetAPICourse

SA =>user
Password =>SQLConnect1

##Comandos 
    console => nueva aplicación tipo consola

-n inicia q le queremos dar un nombre
dotnet new console -n HelloWorld
dotnet new webapi -n DotnetAPIHellow

dotnet run => Ejecuta el proyecto
dotnet watch run  => esta pendiente de los cambios y cada vez q modifico algo lo ejecuta
dotnet watch --no-hot-reload => sirve para proyectos tipo api
dotnet restore => se usa despues de instalar paquetes para q los cargue visual studio.. 


dotnet nuget list source => lista los nugets instalados
    nuget.org => es el q permite descagar paquetes, importante.

    si no aparece en la lista se debe ejecutar este Comando:
    dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org

Solucion para habilitar los snippets y extensiones 
    1. conocer la ruta donde esta instalado .net
        dotnet --info
        el comando anterior nos arroja la informacion entre ella: /usr/local/share/dotnet/sdk

    2. utilizar esta ruta y agregarla en los settings de visualstudio code:
        https://stackoverflow.com/questions/60712895/the-net-core-sdk-cannot-be-located-net-core-debugging-will-not-be-enabled-ma

        dar click en la tuerca ubicada en la parte inferior de vs code y luego en settings:
        luego en el input de search buscar por: @ext:ms-dotnettools.csharp

        buscar la opcion: dotnetpath: y agregar alli el path del paso 1

    3. tambien se creo una variable de entorno (no se si esto ultimo tambien es necesario)
         sudo ln -s /snap/dotnet-sdk/current/dotnet /usr/local/share/dotnet

    4. reinciar el pc

## Comandos Docker
    docker container ls -a  => muestra todos los contenedores
    docker stop sql_connect => detiene el contenedor "sql_connect"
    docker start sql_connect => inicia el contenedor "sql_connect"

## Explicacion clases staticas
    //https://es.stackoverflow.com/questions/152642/static-que-es-y-para-que-sirve

    Un miembro estático de la clase es una propiedad, procedimiento o campo que comparten todas las instancias de una clase.
    todos los miembros estáticos son accesibles desde otras clases sin necesidad de instanciar la clase.

    se adecua por ejemplo a lo que se conoce como una "clase de utilidad", es cuando debes usar la palabra clave static con la clase,
    teniendo en cuenta que eso supone no poder crear instancias, y tener que declarar todos los miembros de esta como estáticos

    public static class StaticMathsClass {
        public const double PI = 3.14159265359;

        public static float Sum (float x, float y) {
            float result = x + y;
            return result;
        }
    }

    //Accedo a los valores y methodos sin necesidad de crear una instancia
    double pi = StaticMathsClass.PI;
    float x = StaticMathsClass.Sum (5, 6);

## BD Comandos
    CREATE DATABASE DotNetCourseDatabase
    GO

    USE DotNetCourseDatabase
    GO

    CREATE SCHEMA TutorialAppSchema
    GO

    CREATE TABLE TutorialAppSchema.Computer(
        ComputerId INT IDENTITY(1,1) PRIMARY KEY,
        Motherboard NVARCHAR(50),
        CPUCores INT,
        HasWifi BIT,
        HasLTE BIT,
        ReleaseDate DATE,
        Price DECIMAL(18,4),
        VideoCard NVARCHAR(50)
    );

    CREATE table TutorialAppSchema.Auth (
        Email NVARCHAR(50),
        PasswordHash VARBINARY (MAX),
        PasswordSalt VARBINARY (MAX),
    )

## Añadir paquetes necesarios:
    dotnet add package Dapper
    dotnet add package AutoMapper
    dotnet add package microsoft.data.sqlclient

    dotnet add package microsoft.Extensions.Configuration
    dotnet add package microsoft.Extensions.Configuration.Json

    dotnet add package Newtonsoft.Json =>utilizado para poder mostrar objetos completos en consola

    dotnet add package microsoft.entityframeworkcore
    dotnet add package microsoft.entityframeworkcore.sqlserver
    dotnet add package microsoft.entityframeworkcore.Relational

## SQL
    en sql, para mostrar todos los campos poner el cursor luego del * y presionar ctrl "+" space

    Cross apply = inner join
    OUTER apply = LEFT join

    SELECT 
    FirstName + ' ' + LastName as FullName
    FROM TutorialAppSchema.Users as Users  



    select 
    ISNULL (Department, 'No Department Label') as Department, //si no hay nombre del departamenot aparece el mensaje seleccionado
    SUM(salary) as salary,
    Count( job.UserId) as TotalPeople,
    AVG( Salary) AS SalaryAverage,
    STRING_AGG(job.UserId, ',') as UsersInDepartment  //en esta columna muestra los ids de cada dpto separados por ","
    from TutorialAppSchema.UserJobInfo as job JOIN
    TutorialAppSchema.UserSalary as salaries on job.UserId = salaries.UserId
    GROUP By Department; 


    SELECT 
    FirstName + ' ' + LastName as FullName,
    Salary,
    dptoAverage.SalaryAverage,
    job.Department
    FROM TutorialAppSchema.Users as Users   JOIN
    TutorialAppSchema.UserSalary as salaries on Users.UserId = salaries.UserId
    JOIN  TutorialAppSchema.UserJobInfo as job on job.UserId = Users.UserId 
    JOIN (
        select 
        ISNULL (Department, 'No Department Label') as Dpto,
        AVG( Salary) AS SalaryAverage,
        job.Department
        from TutorialAppSchema.UserJobInfo as job JOIN
        TutorialAppSchema.UserSalary as salaries on job.UserId = salaries.UserId
        GROUP By Department
    ) as dptoAverage ON dptoAverage.Department = job.Department
    ORDER by Users.UserId


    //esta es la misma consulta pero con OUTER apply
    SELECT 
        FirstName + ' ' + LastName as FullName,
        Salary,
        dptoAverage.SalaryAverage,
        UserJobInfo.Department
        FROM TutorialAppSchema.Users as Users 
        JOIN TutorialAppSchema.UserSalary as userSalaries on Users.UserId = userSalaries.UserId
        LEFT JOIN  TutorialAppSchema.UserJobInfo as UserJobInfo on UserJobInfo.UserId = Users.UserId 
        OUTER APPLY (
            select 
            ISNULL (Department, 'No Department Label') as Dpto,
            AVG( Salary) AS SalaryAverage
        -- job.Department
            from TutorialAppSchema.UserJobInfo as job 
            LEFT JOIN TutorialAppSchema.UserSalary as salaries on job.UserId = salaries.UserId
            where job.Department = UserJobInfo.Department -- ojo que aca es donde hace el join
            GROUP By Department
        ) as dptoAverage
        ORDER by Users.UserId

## DOTNET API
    dotnet new webapi -n DotnetAPIHellow

    recordar q podemos probar los endpoints atraves de swagger
    http://localhost:5000/swagger/index.html

    o en la url normal si son de tipo get
    http://localhost:5000/user/23


    CORS=> Cross Origin resourse sharing


##  Configuraciones:

    Recordar la configuración q se modifica los archivos:

    * 3.2DotnetAPI.csproj,  agregar  <InvariantGlobalization>false</InvariantGlobalization> dentro del propertyGroup
    * Program.cs , configuracion del cors y habilita q se puedan generar los controladores en archivos diferentes
    * Properties/launchSettings =>aca se asigna para q la aplicación siempre se ejecute en el puerto 5000;    
    * appsetings, se agrega la propiedad "ConnectionStrings" al json para la conexión a la BD

