# Frontend

This Angular app was built with Angular 19 and a simple responsive table UI.

## Diseño

La interfaz está basada en el mockup de referencia como guía de tabla y estructura. No hay un archivo de mockup integrado; se implementó una vista ligera y funcional para mostrar préstamos existentes.

## Prerrequisitos

- Node 18+ compatible
- npm
- Backend corriendo en `https://localhost:5001` o ajustar `src/environments/environment.ts`

## Configuración local

1. Instala dependencias:
```sh
npm install
```

2. Ejecuta el frontend:
```sh
npm start
```

3. Abre la aplicación en el navegador:

- `http://localhost:4200/`

## Docker

Este proyecto incluye un Dockerfile para el frontend en `frontend/Dockerfile`.

### Ejecutar solo el frontend en Docker

Desde la carpeta `frontend`:
```sh
docker build -t take-home-test-frontend .
docker run --rm -p 4200:80 take-home-test-frontend
```

El frontend quedará disponible en:

- `http://localhost:4200/`

### Ejecutar frontend y backend juntos con docker-compose

Desde la raíz del repositorio:
```sh
docker-compose up --build
```

Esto levantará:
- `frontend` en `http://localhost:4200/`
- `backend` en `http://localhost:5000/`
- `sqlserver` en el contenedor asociado

Si solo quieres detener los servicios:
```sh
docker-compose down
```

## Endpoints usados

- `GET /api/loans`
- `POST /api/loans`
- `POST /api/loans/{id}/payment`

El URL base se configura en `src/environments/environment.ts`.

## Pruebas

Ejecuta tests unitarios de Angular con:
```sh
npm test
```

## Notas

- La app usa `HttpClient` para consumir la API.
- El componente principal usa Angular signals y `ChangeDetectionStrategy.OnPush`.
