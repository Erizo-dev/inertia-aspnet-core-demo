# Sample projects exploring inertia - aspnet core integration

Run the hosting asp mvc app
```
cd Host

dotnet ef database update
dotnet ef migrations migrate
dotnet run
```

Run the client 
```
cd VueClient
npm run ssr:build
npm run ssr:server
npm run watch

# or just (to run all client side at once)

npm run dev

```

Browse to the url given by the dotnet cli.

## server configuration

based on a default mvc project (dotnet new mvc)
dotnet version 7.0.100 (edit global.json and csproj to override)

- feat : AspNetCore.InertiaCore (version 0.0.7)

## Client Features

- vue spa : VueClient
   - feat typescript
   - feat vite 
   - ssr available
   - Identity

# Notes 

 - switched to kapi2289/InertiaCore as it support ssr out of the box and that shared data are not shared among all users
 - Inertia core is more configurable than the previous implementation
 - keeping cshtml layout for now

# TODO 

   - Error pages / 404 - 500
   - Complete implementations details
   - Images management