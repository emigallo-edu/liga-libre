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

##### Flujo

1. Push a main → arranca commit-stage
2. Si pasa → acceptance-test despliega en staging con curl al hook de staging, espera que levante, y corre los tests de aceptación apuntando a la URL de staging
3. Si pasa → release hace curl al hook de producción

El punto más interesante es el paso 2: después del curl al hook de staging, Render tarda un rato en construir y desplegar. Vas a necesitar esperar a que el servicio esté disponible antes de correr los tests de aceptación. ¿Querés que armemos los tres workflows?

##### 1. Crear los recursos en Render manualmente

1. Ir a [Render](dashboard.render.com)
2. Crear New > PostgreSQL > plan Free > nombre liga-libre-db
3. Crear New > Web Service > conectar tu repo > plan Free > elegir Docker > agregar las variables de entorno:

```text
DATABASE_TYPE = PostgreSql
ConnectionStrings__DefaultConnection = (copiar la Internal Connection String de la DB que creaste)
```

##### 2. Obtener el Deploy Hook:

En el Web Service que creaste > Settings > Deploy Hook
Copiar la URL (tiene forma https://api.render.com/deploy/srv-xxxxx?key=yyyyy)


##### 3. Guardar el hook como GitHub Secret:

Repo en GitHub > Settings > Secrets and variables > Actions
Nuevo secret: RENDER_DEPLOY_HOOK_URL = la URL que copiaste
RENDER_DEPLOY_HOOK_URL_STAGING

##### 4. Agregar el job de deploy al pipeline:



