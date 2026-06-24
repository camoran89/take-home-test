# Backend

Este proyecto implementa una API REST de administración de préstamos usando .NET 6.

## Prerrequisitos

- .NET 6 SDK instalado
- Docker y Docker Compose (opcional para contenedor)

## Montaje local

1. Abre la carpeta `backend/src`.
2. Restaura paquetes y compila:
```sh
dotnet restore src.sln
dotnet build src.sln
```

3. Ejecuta la API:
```sh
cd Fundo.Applications.WebApi
dotnet run
```

La API quedará disponible en:

- `https://localhost:5001`

## Con Docker

### Ejecutar con Docker Compose (recomendado)

Desde la raíz del repositorio:
```sh
docker-compose up --build
```

Esto levantará los servicios:
- `sqlserver`
- `backend` en `http://localhost:5000/`
- `frontend` en `http://localhost:4200/`

Para detener los servicios:
```sh
docker-compose down
```

### Ejecutar solo el backend en Docker

Desde la carpeta `backend`:
```sh
docker build -t take-home-test-backend .
docker run --rm -p 5000:80 \
  -e ConnectionStrings__DefaultConnection="Server=sqlserver,1433;Database=LoanManagement;User Id=sa;Password=Your_strong_Passw0rd!;TrustServerCertificate=True;" \
  take-home-test-backend
```

Nota: si usas este comando, necesitas un servidor SQL Server accesible para la cadena de conexión.

## Endpoints

- `GET /api/loans` → lista préstamos
- `GET /api/loans/{id}` → detalle de loan
- `POST /api/loans` → crea un préstamo
- `POST /api/loans/{id}/payment` → registra un pago

## Pruebas

Ejecutar pruebas unitarias e integración:
```sh
dotnet test src.sln
```

## Notas

- La solución usa `Fundo.Applications.WebApi` y `Fundo.Services.Tests`.
- La base de datos puede usar InMemory para desarrollo local y SQL Server con Docker.
