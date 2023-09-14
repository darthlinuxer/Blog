namespace WebApi.Seeds;

public static class SeedInitializer
{
    public static async Task InitializeAsync(IUserService service)
    {
        await service.RegisterAsync(
            new UserRecordDTO(
                username: "admin",
                password: "ChangeMe1$",
                email: "editor@blog.com",
                role: "Editor"
            )
        );

        await service.RegisterAsync(
               new UserRecordDTO(
                username: "darthlinuxer",
                password: "ChangeMe1$",
                email: "darthlinuxer@blog.com",
                role: "Writer"
            )
        );

        await service.RegisterAsync(
              new UserRecordDTO(
               username: "luke",
               password: "ChangeMe1$",
               email: "luke@rebellalliance.com",
               role: "Public"
           )
       );


    }
}