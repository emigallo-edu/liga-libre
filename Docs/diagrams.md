## Diagrama de Paquetes

```
  Web                           Api
 ┌────────────────┐            ┌──────────────────────────────────┐
 │  Services      │            │  Controllers                     │
 │  Models        │───HTTP────>│  Middlewares                     │
 │  Components    │            │  DTOs                            │
 └────────────────┘            │  Context                         │
                               │  Utils                           │
                               │  Mocks                           │
                               └───────┬──────────┬───────────────┘
                                       │          │
                              ┌────────┘          └────────┐
                              v                            v
  ┌────────────────────────────────┐          ┌────────────────────┐
  │  ApplicationBusinessRules      │          │  Security          │
  │ ┌────────────────────────────┐ │          │ ┌────────────────┐ │
  │ │  UseCases                  │ │          │ │  Identity      │ │
  │ └────────────────────────────┘ │          │ │  Migrations    │ │
  └───────────────┬────────────────┘          └────────────────────┘
                  │
                  v
  ┌──────────────────────────────────────────┐
  │  Entities                                │
  │ ┌──────────────┐ ┌────────────────────┐  │
  │ │  Entidades   │ │ EnterpriseBusiness │  │
  │ │  (clases)    │ │ Rules (clases)     │  │
  │ └──────────────┘ └────────────────────┘  │
  │ ┌──────────────────────────────────────┐ │
  │ │  Repositories (interfaces)           │ │
  │ └──────────────────────────────────────┘ │
  └──────────────────────────────────────────┘
                  ^
                  │
  ┌───────────────┴──────────────┐
  │  Repository                  │
  │ ┌──────────────────────────┐ │
  │ │  Implementaciones        │ │
  │ │  Migrations              │ │
  │ └──────────────────────────┘ │
  └──────────────────────────────┘
                  ^
                  │
  ┌───────────────┴──────────────┐
  │  Test                        │
  │ ┌──────────────────────────┐ │
  │ │  MockRepositories        │ │
  │ └──────────────────────────┘ │
  └──────────────────────────────┘
```
------

## Diagrama de Clases - Liga Libre

```
 ┌──────────────────────┐
 │      Tournament       │
 ├──────────────────────┤
 │  Start               │
 │  End                 │
 ├──────────────────────┤
 │                      │
 └───┬──────┬───────────┘
     │      │
     │ ◇    │ ◇
     │ *    │ *
     v      v
 ┌────────┐ ┌──────────────────────┐        ┌──────────────────────┐
 │ Match  │ │       Club           │        │      Standing        │
 ├────────┤ ├──────────────────────┤        ├──────────────────────┤
 │ Date   │ │  Name                │   ◇ *  │  Win                 │
 │        │ │  Birthday            │<───────│  Draw                │
 │        │ │  City                │        │  Loss                │
 │        │ │  Email               │        │                      │
 └───┬──┬─┘ │  NumberOfPartners    │        └──────────────────────┘
     │  │   │  Phone               │                 ▲
     │  │   │  Address             │                 │ ◆ 1
     │  │   │  StadiumName         │                 │
     │  │   ├──────────────────────┤        ┌────────┴─────────────┐
     │  │   │  IsFromBuenosAires() │        │      Tournament      │
     │  │   │  GetDTO()            │        └──────────────────────┘
     │  │   └───────┬──────────────┘
     │  │           │
     │  │      ◇    │
     │  │      *    │
     │  │           v
     │  │   ┌──────────────────────┐
     │  │   │      Player          │
     │  │   ├──────────────────────┤
     │  │   │  Name                │
     │  │   │  Birthday            │
     │  │   │  Address             │
     │  │   │  ClubId              │
     │  │   └──────────────────────┘
     │  │
     │  └──────> 2 Club (Asociacion: LocalClub, VisitingClub)
     │
     │  ..uses..
     v
 ┌──────────────────────┐
 │      Stadium          │
 ├──────────────────────┤
 │  Name                │
 │  Capacity            │
 │  ClubId              │
 └──────────────────────┘
```

#### Relaciones

| Origen     | Destino  | Tipo          | Cardinalidad | Descripcion                          |
|------------|----------|---------------|--------------|--------------------------------------|
| Tournament | Club     | Agregacion    | 1 a *        | Un torneo agrega multiples clubes    |
| Tournament | Match    | Agregacion    | 1 a *        | Un torneo agrega multiples partidos  |
| Tournament | Standing | Composicion   | 1 a 1        | Un torneo compone su tabla           |
| Club       | Player   | Agregacion    | 1 a *        | Un club agrega multiples jugadores   |
| Match      | Club     | Asociacion    | 1 a 2        | Un partido tiene local y visitante   |
| Match      | Stadium  | Uso           | 1 a 1        | Un partido se juega en un estadio    |
| Standing   | Club     | Agregacion    | 1 a *        | Una tabla agrega multiples clubes    |

#### Dependencias

| Origen                     | Destino                    | Tipo        |
|----------------------------|----------------------------|-------------|
| Web                        | Api                        | HTTP        |
| Api                        | ApplicationBusinessRules   | Referencia  |
| Api                        | Repository                 | Referencia  |
| Api                        | Security                   | Referencia  |
| ApplicationBusinessRules   | Entities                   | Referencia  |
| Repository                 | Entities                   | Referencia  |
| Test                       | ApplicationBusinessRules   | Referencia  |
| Test                       | Repository                 | Referencia  |

## Implementacion de un Caso de Uso

#### Ejemplo: `CreateClubUseCase`

```
 ApplicationBusinessRules                     Entities
┌─────────────────────────────────────────────────────────────────────────────────┐
│                                                                                 │
│                                              EnterpriseBusinessRules            │
│                                             ┌─────────────────────┐             │
│  ┌──────────────────────┐   usa clase       │                     │             │
│  │                      │ ─────────────────>│    InsertClub       │             │
│  │                      │   (sin interface) │                     │             │
│  │  CreateClubUseCase   │                   └──────────┬──────────┘             │
│  │                      │                              │                        │
│  │                      │                    usa clase  │                       │
│  │                      │   usa clase       ┌──────────v──────────┐             │
│  │                      │ ─────────────────>│                     │             │
│  │                      │   (sin interface) │    GetStadium       │             │
│  │                      │                   │                     │             │
│  │                      │                   └──────────┬──────────┘             │
│  │                      │                              │                        │
│  │                      │   usa clase                  │                        │
│  │                      │ ────────────┐                │                        │
│  └──────────────────────┘             │                │                        │
│                                       │                │                        │
│                                       v                │                        │
│                              Entities/Entities         │                        │
│                             ┌──────────────┐           │                        │
│                             │    Club      │           │                        │
│                             │   (clase)    │           │                        │
│                             └──────────────┘           │                        │
│                                                        │                        │
│                                                        │                        │
│                               Repositories             │                        │
│                              (INTERFACES)              │                        │
│                             ┌───────────────────┐      │                        │
│                             │ <<interface>>     │      │ usa interface          │
│                             │ IClubRepository   │<─────┘                        │
│                             ├───────────────────┤                               │
│                             │ <<interface>>     │                               │
│                             │ IStadiumRepository│<──────────────────────────┐   │
│                             └───────────────────┘                           │   │
│                                                     (GetStadium tambien     │   │
│                                                      usa IStadiumRepository)│   │
│                                                                             │   │
└─────────────────────────────────────────────────────────────────────────────────┘

                                       ▲
                                       │ implementa interface
                                       │
                               Repository (proyecto)
                              ┌──────────────────────┐
                              │  ClubDbRepository    │──── implements IClubRepository
                              │  StadiumRepository   │──── implements IStadiumRepository
                              │  (clases concretas)  │
                              └──────────────────────┘
```

##### Por que la diferencia?

| Capa                       | Se usa como...     | Razon                                              |
|----------------------------|--------------------|-----------------------------------------------------|
| **EnterpriseBusinessRule** | Clase directa      | Son reglas de negocio puras, no necesitan ser reemplazadas por otra implementacion. No acceden a infraestructura. |
| **Repository**             | Interface          | Abstrae el acceso a datos. Permite cambiar la implementacion (DB, archivo, mock) sin modificar las reglas de negocio. |

##### Ejemplo en codigo

```csharp
// UseCase usa CLASE DIRECTA de EnterpriseBusinessRule (no hay IInsertClub)
public class CreateClubUseCase
{
    private readonly InsertClub _insertClub;        // clase concreta
    private readonly GetStadium _getStadium;        // clase concreta

    public CreateClubUseCase(InsertClub insertClub, GetStadium getStadium)
    {
        _insertClub = insertClub;
        _getStadium = getStadium;
    }
}

// EnterpriseBusinessRule usa INTERFACE de Repository (no usa ClubDbRepository)
public class InsertClub
{
    private readonly IClubRepository _clubRepository;  // interface!

    public InsertClub(IClubRepository clubRepository)
    {
        _clubRepository = clubRepository;
    }
}
```

##### Registro en DI (Program.cs)

```csharp
// EBR: se registran como clase concreta -> clase concreta
builder.Services.AddScoped<InsertClub>();
builder.Services.AddScoped<GetStadium>();

// Repository: se registran como interface -> implementacion concreta
builder.Services.AddScoped<IClubRepository, ClubDbRepository>();
builder.Services.AddScoped<IStadiumRepository, StadiumRepository>();
```