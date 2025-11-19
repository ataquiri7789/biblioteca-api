# 📚 Biblioteca API – CRUD de Libros (.NET 8 + PostgreSQL + Arquitectura Limpia)

API REST construida con **ASP.NET Core 8**, **Arquitectura Limpia**, **PostgreSQL**, **Stored Procedures** y **Vistas**, diseñada como parte de una prueba técnica.

Incluye:

- ✔ Arquitectura en capas / limpia (Domain, Application, Infrastructure, Api)  
- ✔ CRUD de Libros (crear, leer, actualizar, eliminar lógico)  
- ✔ Base de datos PostgreSQL con **tabla**, **vista** y **procedimientos almacenados**  
- ✔ Script SQL unificado (`db/script.txt`)  
- ✔ Swagger para probar los endpoints  
- ✔ Instrucciones claras para ejecutar la solución localmente  

---

## 📁 Estructura del Proyecto

```text
BibliotecaApi/
│  BibliotecaApi.sln
│  README.md
│
├─ BibliotecaApi/                # Capa API (ASP.NET Core 8 Web API)
│
├─ BibliotecaApi.Application/    # Capa de Aplicación (servicios, DTOs, casos de uso)
│
├─ BibliotecaApi.Domain/         # Capa de Dominio (entidades + interfaces)
│
├─ BibliotecaApi.Infrastructure/ # Capa de Infraestructura (EF Core + PostgreSQL + repositorios)
│
└─ db/
   └─ script.txt                 # Script de base de datos (tabla, vista, SPs, datos de prueba)
```

---

## 🗄 Base de Datos (PostgreSQL)

Toda la lógica de base de datos está centralizada en:

```text
/db/script.txt
```

Este archivo contiene:

- Creación de la base de datos `biblioteca_db`
- Creación de la tabla `libros`
- Creación de la vista `vw_libros_activos`
- Procedimientos almacenados:
  - `sp_insertar_libro`
  - `sp_actualizar_libro`
  - `sp_eliminar_libro`
- 3 registros de ejemplo relacionados con **bioquímica**

### 🔹 Uso del script en PostgreSQL

1. Abrir **psql** (SQL Shell) o cualquier cliente PostgreSQL.
2. Conectarse al servidor con el usuario deseado (por ejemplo `postgres`).
3. Ejecutar el script indicando la ruta completa, por ejemplo:

```sql
\i 'C:/RUTA/A/TU/PROYECTO/BibliotecaApi/db/script.txt'
```

4. Verificar que todo quedó correcto:

```sql
\c biblioteca_db;
SELECT * FROM libros;
SELECT * FROM vw_libros_activos;
```

Si ves 3 libros de prueba, la base de datos está lista ✅.

---

## 🛠 Dependencias Requeridas

### Software

- **.NET SDK 8.0**
- **PostgreSQL 14+** (local o remoto)
- Git (para clonar el repositorio)
- Opcional: Visual Studio 2022 / VS Code

### Paquetes NuGet principales (ya referenciados en el proyecto)

En la capa de infraestructura / API se utilizan, entre otros:

- `Microsoft.EntityFrameworkCore` (8.x)
- `Npgsql.EntityFrameworkCore.PostgreSQL` (8.x)
- `Npgsql` (8.x)
- `Swashbuckle.AspNetCore` (Swagger)
- `Amazon.Lambda.AspNetCoreServer.Hosting` (para preparación a AWS Lambda)

> No es necesario instalar nada manualmente: `dotnet restore` descargará todas las dependencias.

---

## ⚙️ Configuración de la Cadena de Conexión

En el proyecto **API** (`BibliotecaApi`), archivo `appsettings.json`, configurar:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=biblioteca_db;Username=postgres;Password=TU_PASSWORD"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

Reemplaza `TU_PASSWORD` por la contraseña real del usuario de PostgreSQL.

---

## ▶️ Cómo Ejecutar la Solución Localmente

### 1️⃣ Clonar el repositorio (si aplica)

```bash
git clone https://github.com/ataquiri7789/TU_REPO.git
cd BibliotecaApi
```

### 2️⃣ Crear la base de datos

Ejecutar el script:

```sql
\i 'RUTA_COMPLETA/BibliotecaApi/db/script.txt'
```

Desde `psql`, como se indicó arriba.

### 3️⃣ Restaurar y ejecutar la API

Desde la carpeta del proyecto API:

```bash
cd BibliotecaApi/BibliotecaApi
dotnet restore
dotnet run
```

La API iniciará normalmente en:

- `https://localhost:5001`
- `http://localhost:5134`

### 4️⃣ Probar desde Swagger

Navegar a:

```text
http://localhost:5134/swagger
```

Ahí verás todos los endpoints documentados y podrás ejecutarlos directamente.

---

## 🌐 Endpoints Disponibles y Cómo Consumirlos

Base URL local (ejemplo):

```text
http://localhost:5134/api/libros
```

### 1️⃣ Listar todos los libros

**Endpoint:**

```http
GET /api/libros
```

**Descripción:**  
Devuelve la lista de libros activos usando la vista `vw_libros_activos`.

---

### 2️⃣ Obtener un libro por ID

**Endpoint:**

```http
GET /api/libros/{id}
```

**Ejemplo:**

```http
GET /api/libros/1
```

**Respuesta:**  
Un objeto JSON con los datos del libro, o **404 Not Found** si no existe o está inactivo.

---

### 3️⃣ Crear un nuevo libro

**Endpoint:**

```http
POST /api/libros
```

**Body (JSON):**

```json
{
  "titulo": "Bioquímica de los Procesos Metabólicos",
  "autor": "Ana López",
  "anioPublicacion": 2022,
  "editorial": "Editorial Universitaria",
  "paginas": 450,
  "categoria": "Bioquímica",
  "isbn": "9789876543210"
}
```

**Notas:**

- El endpoint valida los campos (título, autor, rangos de año, páginas, etc.).

---

### 4️⃣ Actualizar un libro existente

**Endpoint:**

```http
PUT /api/libros/{id}
```

**Body (JSON):**

```json
{
  "titulo": "Bioquímica Médica Esencial (2da Edición)",
  "autor": "Juan Carlos Rivas",
  "anioPublicacion": 2023,
  "editorial": "Editorial Médica Panamericana",
  "paginas": 540,
  "categoria": "Bioquímica",
  "isbn": "9789581234570"
}
```

**Notas:**

- Si el libro no existe o está inactivo, se devuelve **404 Not Found**.

---

### 5️⃣ Eliminar libro (eliminado lógico)

**Endpoint:**

```http
DELETE /api/libros/{id}
```

**Descripción:**

- No se elimina físicamente el registro, solo se marca con `activo = FALSE`.

**Respuesta:**

- **204 No Content** si se realizó el eliminado lógico.
- **404 Not Found** si el libro no existe o ya estaba inactivo.

---

## 🧪 Pruebas con Postman o cURL

### Ejemplo con `curl` – Obtener todos los libros

```bash
curl -X GET https://localhost:5001/api/libros
```

### Ejemplo con `curl` – Crear un libro

```bash
curl -X POST https://localhost:5001/api/libros   -H "Content-Type: application/json"   -d '{
    "titulo": "Bioquímica Clínica Aplicada",
    "autor": "María Fernández",
    "anioPublicacion": 2021,
    "editorial": "Editorial Científica",
    "paginas": 380,
    "categoria": "Bioquímica Clínica",
    "isbn": "9781234567890"
  }'
```

## 📮 Colección de Postman

El proyecto incluye una colección de Postman lista para probar todos los endpoints de la API.

Archivo:

- [`postman/BibliotecaApi.postman_collection.json`](postman/BibliotecaApi.postman_collection.json)

### Cómo usarla

1. Abrir **Postman**.
2. Ir a **Import**.
3. Seleccionar el archivo `BibliotecaApi.postman_collection.json`.
4. Configurar la variable `baseUrl` (opcional):
   - Por defecto está en `http://localhost:5134`.
   - Si la API corre en otro puerto, actualizar la variable en Postman.

La colección contiene:

- `GET /api/libros` – Listar libros activos  
- `GET /api/libros/{id}` – Obtener libro por ID  
- `POST /api/libros` – Crear libro (con todos los campos)  
- `PUT /api/libros/{id}` – Actualizar libro  
- `DELETE /api/libros/{id}` – Eliminar (lógico) libro




---

## 🧩 Resumen de la Arquitectura

Se sigue una **Arquitectura Limpia / en capas**:

- **Domain (`BibliotecaApi.Domain`)**
  - Entidad `Libro`
  - Interfaz `ILibroRepositorio`

- **Application (`BibliotecaApi.Application`)**
  - DTOs (`LibroCrearDto`, `LibroActualizarDto`, `LibroLeerDto`)
  - Servicio `ILibroServicio` / `LibroServicio`
  - Aquí se implementan las reglas de negocio y casos de uso.

- **Infrastructure (`BibliotecaApi.Infrastructure`)**
  - `AppDbContext` (EF Core + Npgsql)
  - `LibroRepositorio` (implementa `ILibroRepositorio`)
  - Consumo de la vista y los procedimientos almacenados de PostgreSQL.

- **Api (`BibliotecaApi`)**
  - `LibrosController` expone los endpoints REST.
  - `ManejadorErroresMiddleware` maneja errores globales.
  - Configuración de DI, Swagger y (opcionalmente) AWS Lambda.

---

## ☁️ Preparación para AWS Lambda

El proyecto puede ejecutarse como una API tradicional (Kestrel) o adaptarse a AWS Lambda usando:

```csharp
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
```

---

## 👨‍💻 Autor

**Alexander Taquiri**  
Desarrollador .NET / Arquitectura Limpia / PostgreSQL / APIs REST  

---

## 📝 Notas Finales

Este proyecto está orientado a demostrar:

- Buen uso de **ASP.NET Core 8**  
- Separación de responsabilidades con Arquitectura Limpia  
- Uso de **PostgreSQL** con **Stored Procedures** y **Vistas**  
- Buenas prácticas de diseño para APIs REST  


