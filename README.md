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
  - Smoke:
  ```powershell
  dotnet test Tests/Test.Smoke/Test.Smoke.csproj
  ```
- 
> Nota: esta guía se diseñó para que los estudiantes puedan entender rápidamente cómo levantar el proyecto, cambiar los puntos de conexión y trabajar con migraciones y pruebas.

## Paso a paso: Desplegar en Render

##### 1. Crear los recursos manualmente:

**1.a Crear la base de datos en Azure SQL (plan gratuito)**

Render no ofrece SQL Server nativo, así que la DB vive en Azure SQL Database. El plan gratuito da 32 GB por 12 meses.

1. Entrar al [Portal de Azure](https://portal.azure.com) y crear una cuenta si no tenés.
2. Crear un **SQL Server**:
    - Create a resource > **SQL Database** > Create.
    - En la pestaña *Basics* > *Server*, clic en **Create new**:
        - Server name: `liga-libre1` (debe ser único globalmente)
        - Location: una región cercana (ej. `Brazil South`)
        - Authentication method: **Use SQL authentication**
        - Admin login + password: guardalos, los vas a necesitar para la connection string.
3. Crear la **Database**:
    - Database name: `liga-libre-staging`
    - Compute + storage: clic en **Configure database** > elegir **General Purpose - Serverless** > seleccionar **Free tier** (100.000 vCore-segundos por mes) o el tier más chico disponible.
    - Backup storage redundancy: Locally-redundant.
4. En la pestaña *Networking*:
    - Connectivity method: **Public endpoint**.
    - Allow Azure services and resources to access this server: **Yes** (para que Render pueda conectarse).
    - Add current client IP address: **Yes** (para poder correr migraciones desde tu máquina).
5. Revisar y crear. Tarda 1-2 minutos.
6. Una vez creada, ir a la DB > *Connection strings* > pestaña **ADO.NET** y copiar la cadena. Va a tener la forma:
    ```text
    Server=tcp:liga-libre-sql.database.windows.net,1433;Initial Catalog=LigaLibre;Persist Security Info=False;User ID={tu-admin};Password={tu-password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
    ```
    Reemplazar `{tu-admin}` y `{tu-password}` por los valores reales.

> **Nota sobre instancias free**: Azure SQL en free tier se auto-pausa tras un rato de inactividad. La primera request tras el pause tarda ~30 segundos en levantar la DB y puede devolver `500`. Si el pipeline falla con 500 en el step de `Update DB`, re-ejecutar el job suele resolverlo.

**1.b Crear el Web Service en Render**

1. Ir a [Render](https://dashboard.render.com) y crear cuenta.
2. **New > Web Service** > elegir **"Deploy an existing image from a registry"** > plan **Free**.
    - Image URL: `ghcr.io/{tu-usuario}/liga-libre-api:latest` (reemplazar `{tu-usuario}` por tu usuario/org de GitHub).
    - Region: la más cercana a la region de Azure SQL.
    - Agregar las variables de entorno:
    ```text
    DATABASE_TYPE   = SqlServer
    CONNECTION_STRING = (pegar la Connection String obtenida en el paso 1.a)
    ```
3. Crear el servicio. Render va a intentar el primer deploy; puede fallar si la imagen aún no fue publicada — no importa, los próximos deploys los dispara el pipeline.

##### 2. Obtener el API Key y Service ID de Render:

- **API Key**: *Account Settings > API Keys* > **Create API Key**.
- **Service ID**: entrar al Web Service creado y copiar el ID desde la URL del dashboard (tiene la forma `srv-xxxxxxxxxx`).

##### 3. Configurar secrets y variables en GitHub:

*Repo en GitHub > Settings > Secrets and variables > Actions*.

**Secrets** (pestaña *Secrets*):
- `RENDER_API_KEY` — la API Key creada en el paso 2.
- `RENDER_SERVICE_ID_API_STAGING` — el Service ID del Web Service de la API.
- `RENDER_SERVICE_ID_WEB_STAGING` — el Service ID del Web Service de la Web *(pendiente hasta que se despliegue la Web)*.

**Variables** (pestaña *Variables*):
- `SMOKE_STAGING_API_URL` — la URL pública del Web Service en Render, ej. `https://liga-libre-api.onrender.com`. Se usa en el pipeline para el smoke test y el update-db.

##### 4. Pipeline (ya configurado en `.github/workflows/`):

El release está gateado por tags `v*`. Un push o PR a `main` solo corre Commit Stage; el deploy real arranca cuando pusheás un tag de versión:

```powershell
git tag v1.0.0
git push origin v1.0.0
```

Flujo completo disparado por el tag:

| # | Workflow | Archivo | Trigger | Qué hace |
|---|----------|---------|---------|----------|
| 1 | Commit Stage | `commit-stage.yml` | Push/PR a `main`, push de tag `v*` | Build + tests unitarios + tests de arquitectura |
| 2 | Acceptance Tests — job `acceptance-tests` | `acceptance-test.yml` | `workflow_run` sobre Commit Stage exitoso (solo si el branch/tag empieza con `v`) | Tests de aceptación + integración |
| 3 | Acceptance Tests — job `publish-and-deploy` | `acceptance-test.yml` | Depende de `acceptance-tests` | Publica la imagen en GHCR y dispara el deploy en Render |
| 4 | Acceptance Tests — job `update-db` | `acceptance-test.yml` | Depende de `publish-and-deploy` | Espera a que la API esté arriba y llama a `POST /Deployment/update-database` para aplicar migraciones |
| 5 | Acceptance Tests — job `smoke-tests` | `acceptance-test.yml` | Depende de `update-db` | Verifica drift modelo↔migraciones y corre el proyecto `Test.Smoke` contra staging |

`release.yml` por ahora es un placeholder; el deploy a staging se hace desde `acceptance-test.yml`.

**Resumen de secrets y variables**:

| Nombre | Tipo | Dónde se usa |
|--------|------|--------------|
| `RENDER_API_KEY` | secret | Autenticación contra la API de Render para disparar deploys |
| `RENDER_SERVICE_ID_API_STAGING` | secret | Identifica el Web Service de la API |
| `RENDER_SERVICE_ID_WEB_STAGING` | secret | Identifica el Web Service de la Web (pendiente) |
| `SMOKE_STAGING_API_URL` | variable | URL pública del API para update-db y smoke tests |