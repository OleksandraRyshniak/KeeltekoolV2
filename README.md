Käivita projekt (`Keeltekool_2`, profiil **https**, F5).

| Mis | Link |
|---|---|
| Swagger UI | https://localhost:7153/docs |
| OpenAPI JSON | https://localhost:7153/swagger/v1/swagger.json |
| Õpilased | https://localhost:7153/api/students |
| Õpetajad | https://localhost:7153/api/lecturers |
| Keelekursused | https://localhost:7153/api/languagecourses |

Iga endpoint: `GET`, `GET /{id}`, `POST`, `DELETE /{id}`.

**Enne esimest käivitust** loo andmebaas:

    dotnet ef database update --project ..\Keeltekool_2.Data --startup-project .\Keeltekool_2

(käivita kaustas `Keeltekool_2`; vajadusel `dotnet tool install --global dotnet-ef --version 10.0.12`)

> Swagger töötab ainult Development-režiimis. Kui port on teine, vaata konsoolist rida `Now listening on:`.
