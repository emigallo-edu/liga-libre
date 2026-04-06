# Diagrama de Paquetes - Liga Libre

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

## Dependencias

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
