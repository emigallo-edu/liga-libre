# Liga Libre

## Introducción
Este proyecto se usa en diversos cursos: C#, Programación Orientada a Objetos, API, Blazor y entrega continua. Aquí encontrarás instrucciones para levantar la API, la aplicación web, configurar la base de datos, trabajar con migraciones de EF Core y ejecutar pruebas automáticas.

## Estructura principal
- `Api/`: API REST en .NET 8.
- `Web/`: aplicación web Blazor Server que consume la API.
- `Repository/`: contexto y migraciones de Entity Framework.
- `Security/`: identidad y seguridad.
- `Tests/`: pruebas unitarias, de integración, de aceptación y de arquitectura.

[Detalle de diagramas](docs/diagrams.md)

## Cómo levantar la API
1. Abrir la terminal en la raíz del repositorio.
2. Ejecutar:
   ```powershell
   dotnet restore
   dotnet build
   dotnet run --project Api/Api.csproj
   ```
3. La API por defecto está configurada para escuchar en:
   - `http://localhost:5182`
   - `https://localhost:7054`
4. El `launchSettings.json` de `Api` define el perfil `NetWebApi` con `applicationUrl` en `http://localhost:5182`.

## Cómo levantar la Web
1. En otra terminal, desde la raíz del repositorio:
   ```powershell
   dotnet run --project Web/Web.csproj
   ```
2. La aplicación Blazor usa la configuración de `Web/appsettings.json`:
   ```json
   {
     "ApiBaseUrl": "http://localhost:5182"
   }
   ```
3. Si la API corre en otro puerto, cambia `ApiBaseUrl` para que apunte al URL correcto de la API.

## Configurar la base de datos y la API
### API local
- Archivo de configuración: `Api/appsettings.json`
- Cadena de conexión por defecto:
  ```json
  "ConnectionStrings": {
    "DefaultConnectionString": "Server=localhost;Database=NetWebApi;Trusted_Connection=True;Encrypt=false"
  }
  ```
- Esta configuración usa SQL Server local con autenticación de Windows.
- Si usas SQL Server en otra máquina o puerto, reemplaza `Server=localhost` por la dirección adecuada.

### API con Docker
El `docker-compose.yml` ya define un servicio SQL Server y la API:
- Base de datos `db` con usuario `sa` y contraseña `LigaLibre123!`.
- La API usa la variable de entorno:
  ```text
  ConnectionStrings__DefaultConnection=Server=db;Database=NetWebApi;User Id=sa;Password=LigaLibre123!;Encrypt=false;TrustServerCertificate=true
  ```
- Para levantar con Docker:
  ```powershell
  docker-compose up --build
  ```
- En este escenario, la API está expuesta en el puerto `5000` del host, porque `docker-compose.yml` mapea `5000:8080`.
- Si deseas usar la Web local contra esta API Docker, actualiza `Web/appsettings.json` con:
  ```json
  {
    "ApiBaseUrl": "http://localhost:5000"
  }
  ```

## Cómo manipular migraciones de EF Core
El proyecto utiliza `Repository/ApplicationDbContext.cs` y `Api/Context/ApplicationDbContextFactory.cs` para la configuración de EF Core.

### Comandos básicos
- Agregar una migración nueva:
  ```powershell
  Add-Migration {nombre} -StartUpProject Api -Project Repository -Context ApplicationDbContext
  ```
- Actualizar la base de datos:
  ```powershell
  Update-Database -StartUpProject Api -Project Repository -Context ApplicationDbContext
  ```
- Eliminar la última migración (si no está aplicada en la DB):
  ```powershell
  Remove-Migration -StartUpProject Api -Project Repository -Context ApplicationDbContext
  ```
- Volver a un estado anterior de migración:
  ```powershell
  Update-Database {nombre de la migración en la cual queremos dejar el estado de la DB} -StartUpProject Api -Project Repository -Context ApplicationDbContext -Migration {nombre}
  ```

### Puntos importantes
- `Api/Context/ApplicationDbContextFactory.cs` usa primero la variable de entorno `ConnectionStrings__DefaultConnection` y luego `Api/appsettings.json`.
- Si trabajas con la base de datos en Docker, modifica la conexión según el puerto y el nombre del servicio.
- Para migraciones de seguridad, usa el contexto `SecurityDbContext` y el proyecto `Security/Security.csproj`:
  ```powershell
  Add-Migration {nombre} -StartUpProject Api -Project Repository -Context ApplicationDbContext
  ```

## Cómo entender y correr las pruebas automáticas
### Ejecutar todas las pruebas
Desde la raíz del repo:
```powershell
dotnet test LigaLibre.sln
```

### Ejecutar proyectos de prueba individuales
- Unitarias:
  ```powershell
  dotnet test Tests/Test.Unit/Test.Unit.csproj
  ```
- Integración:
  ```powershell
  dotnet test Tests/Test.Integration/Test.Integration.csproj
  ```
- Aceptación:
  ```powershell
  dotnet test Tests/Test.Acceptance/Test.Acceptance.csproj
  ```
- Arquitectura:
  ```powershell
  dotnet test Tests/Test.Architecture/Test.Architecture.csproj
  ```

> Nota: esta guía se diseñó para que los estudiantes puedan entender rápidamente cómo levantar el proyecto, cambiar los puntos de conexión y trabajar con migraciones y pruebas.

## Paso a paso: Desplegar en Render

##### 1. Crear los recursos en Render manualmente:

1. Ir a [Render](dashboard.render.com)
2. Crear New > PostgreSQL > plan Free > nombre `liga-libre-db`
3. Crear New > Web Service > elegir **"Deploy an existing image from a registry"** > plan Free
    - En Image URL poner: `ghcr.io/{tu-usuario}/liga-libre-api:latest`
    - Agregar las variables de entorno:

```text
DATABASE_TYPE = PostgreSql
ConnectionStrings__DefaultConnection = (copiar la Internal Connection String de la DB que creaste)
```

4. Crear la web


##### 2. Obtener el API Key y Service ID de Render:

- API Key: ir a Account Settings > API Keys > crear una key
- Service ID: en el Web Service que creaste, copiar el ID de la URL (tiene forma `srv-xxxxxxxxxx`)

##### 3. Guardar los secrets en GitHub:

Repo en GitHub > Settings > Secrets and variables > Actions
- `RENDER_API_KEY` = la API Key que creaste
- `RENDER_SERVICE_ID` = el Service ID del Web Service

##### 4. Pipeline (ya configurado en `.github/workflows/`):

El pipeline se dispara automáticamente con cada push a `main`:

1. **Commit Stage** (`commit-stage.yml`): compila, corre tests unitarios y de arquitectura, construye la imagen Docker y la publica en GitHub Container Registry (GHCR)
2. **Acceptance Tests** (`acceptance-test.yml`): corre tests de aceptación e integración
3. **Release** (`release.yml`): despliega en Render la imagen exacta que pasó todos los tests



Los 3 workflows

| Paso | Workflow | Archivo	| Trigger	| Qué hace|
|------|----------|---------|-----------|---------|
| 1    | Commmit Stage | commit-stage.yml | Push/PR a main | Build + tests unitarios + arquitectura |
| 2    | Acceptance Tests | acceptance-test.yml | Tag | Build + acceptance + integration tests + publica imagen en GHCR + despliega en staging |
| 3    | Release | release.yml | Manual | Despliega la misma imagen en Render via API |

Secrets que necesitás crear en GitHub
RENDER_API_KEY — API key de tu cuenta Render
RENDER_SERVICE_ID — el ID del Web Service (lo ves en la URL, srv-xxxxx