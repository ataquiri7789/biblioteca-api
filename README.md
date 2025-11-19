# 📚 Biblioteca API – CRUD de Libros (.NET 8 + PostgreSQL + Arquitectura Limpia)

API REST construida con **ASP.NET Core 8**, **Arquitectura Limpia**, **PostgreSQL** y **EF Core**, desarrollada como parte de una prueba técnica.

Incluye:

- ✔ Arquitectura en capas / limpia (Domain, Application, Infrastructure, Api)
- ✔ CRUD completo de Libros (crear, consultar, actualizar y eliminado lógico)
- ✔ Base de datos PostgreSQL con tabla, vista y script unificado (`db/script.txt`)
- ✔ Documentación de cómo ejecutar la solución localmente (instrucciones, dependencias y comandos)
- ✔ Sección de endpoints con ejemplos de consumo
- ✔ Colección de Postman para pruebas manuales
- ✔ Sección opcional de despliegue en **AWS Lambda**

---

## 🧱 Estructura del Proyecto

```text
biblioteca-api/
│  BibliotecaApi.sln
│  README.md
│
├─ BibliotecaApi/                # Capa API (controllers, middlewares, Program.cs)
│
├─ BibliotecaApi.Application/    # Capa de Aplicación (servicios, DTOs, casos de uso)
│
├─ BibliotecaApi.Domain/         # Capa de Dominio (entidades + interfaces)
│
├─ BibliotecaApi.Infrastructure/ # Capa de Infraestructura (EF Core + PostgreSQL + repositorios)
│
└─ db/
   └─ script.txt                 # Script de base de datos (tabla, vista y datos de ejemplo)
```

---

## ⚙️ Requisitos

### Software necesario

- **.NET SDK 8.0**
- **PostgreSQL 14+**
- **Git**
- Opcional: **Visual Studio 2022** o **VS Code**

### Paquetes NuGet principales (ya configurados en el proyecto)

- `Microsoft.EntityFrameworkCore`
- `Npgsql.EntityFrameworkCore.PostgreSQL`
- `Npgsql`
- `Swashbuckle.AspNetCore` (Swagger)
- `Amazon.Lambda.AspNetCoreServer.Hosting` (para hosting en AWS Lambda, opcional)

> No es necesario instalar paquetes manualmente: `dotnet restore` descargará todas las dependencias del proyecto.

---

## 🗄️ Instalación de la Base de Datos (PostgreSQL)

La API utiliza una base de datos PostgreSQL llamada **`biblioteca_db`**.

Todo el SQL del proyecto se encuentra en:

```text
db/script.txt
```

Este archivo contiene:

- Creación de la tabla `libros`
- Creación de la vista `vw_libros_activos`
- Inserción de 3 registros de prueba relacionados con bioquímica

> 🔹 El script **NO** crea la base de datos, solo los objetos dentro de ella.  
> Debes crear primero la base `biblioteca_db` y luego ejecutar el script dentro de esa base.

---

### 🟦 1. Crear la base de datos

Puedes crear la base de datos desde `psql` o desde pgAdmin.

#### ✔ Opción A – Crear desde `psql`

Abrir terminal y ejecutar:

```bash
psql -U postgres
```

Dentro de `psql`:

```sql
CREATE DATABASE biblioteca_db;
```

Confirmar que exista:

```sql
\l
```

#### ✔ Opción B – Crear desde pgAdmin

1. Clic derecho en **Databases**
2. **Create → Database**
3. Nombre: `biblioteca_db`
4. Owner: `postgres` (o el usuario que corresponda)
5. Guardar

---

### 🟩 2. Conectarse a la base de datos

Desde consola:

```bash
psql -U postgres -d biblioteca_db
```

O dentro de `psql`:

```sql
\c biblioteca_db
```

Ahora ya estás dentro de **biblioteca_db** y puedes ejecutar el script del proyecto.

---

### 🟧 3. Ejecutar el script SQL del proyecto

El archivo se encuentra en:

```text
db/script.txt
```

#### ✔ Ejecutar desde `psql` (recomendado)

Desde la raíz del proyecto (`biblioteca-api/`):

```bash
psql -U postgres -d biblioteca_db -f db/script.txt
```

O, si ya estás dentro de `psql` y conectado a `biblioteca_db`:

```sql
\i 'RUTA_COMPLETA/biblioteca-api/db/script.txt'
```

Esto creará:

- Tabla `libros`
- Vista `vw_libros_activos`
- 3 libros de ejemplo

---

### 🟨 4. Verificar que todo está correcto

Dentro de `psql`, ejecutar:

```sql
SELECT * FROM libros;
SELECT * FROM vw_libros_activos;
```

Deberías ver los 3 registros iniciales de prueba 🎉

---

### 🟪 5. Contenido de `db/script.txt` (referencia)

```sql
CREATE TABLE IF NOT EXISTS libros (
    id               SERIAL PRIMARY KEY,
    titulo           VARCHAR(200) NOT NULL,
    autor            VARCHAR(150) NOT NULL,
    anio_publicacion INT NOT NULL CHECK (anio_publicacion BETWEEN 1450 AND 2100),
    editorial        VARCHAR(150),
    paginas          INT NOT NULL CHECK (paginas > 0),
    categoria        VARCHAR(100),
    isbn             VARCHAR(20),
    activo           BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE OR REPLACE VIEW vw_libros_activos AS
SELECT 
    id,
    titulo,
    autor,
    anio_publicacion,
    editorial,
    paginas,
    categoria,
    isbn,
    activo
FROM libros
WHERE activo = TRUE;

INSERT INTO libros (titulo, autor, anio_publicacion, editorial, paginas, categoria, isbn)
VALUES
('Bioquímica Médica Esencial', 'Juan Carlos Rivas', 2021, 'Editorial Médica Panamericana', 520, 'Bioquímica', '9789581234570'),
('Fundamentos de Bioquímica: Enfoque Clínico', 'María Fernanda Delgado', 2019, 'McGraw-Hill Educación', 680, 'Bioquímica Clínica', '9786071503214'),
('Metabolismo Humano y Regulación Bioquímica', 'Luis Alberto Paredes', 2020, 'Editorial Científica Latinoamericana', 430, 'Metabolismo', '9786123456789');
```

---

## ▶️ Cómo ejecutar la solución localmente

### 1️⃣ Clonar el repositorio

```bash
git clone https://github.com/ataquiri7789/biblioteca-api.git
cd biblioteca-api
```

### 2️⃣ Restaurar dependencias

```bash
dotnet restore
```

### 3️⃣ Configurar la cadena de conexión

En `BibliotecaApi/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=biblioteca_db;Username=postgres;Password=TU_PASSWORD"
  }
}
```

Reemplaza `TU_PASSWORD` por la contraseña real de tu usuario PostgreSQL.

### 4️⃣ Ejecutar la API

Desde la raíz del repo:

```bash
dotnet run --project BibliotecaApi/BibliotecaApi.csproj
```

La API arrancará típicamente en:

- `https://localhost:5001`
- `http://localhost:5000`

### 5️⃣ Probar la API con Swagger

Abrir en el navegador:

```text
https://localhost:5001/swagger
```

Ahí verás todos los endpoints de la API y podrás probarlos directamente.

---

## 📮 Colección de Postman

El proyecto incluye una colección de Postman para probar todos los endpoints del CRUD de libros.

Se recomienda colocarla en:

```text
postman/BibliotecaApi.postman_collection.json
```

Ejemplo de colección (estructura JSON simplificada):

```json
{
  "info": {
    "name": "BibliotecaApi - CRUD Libros",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "variable": [
    {
      "key": "baseUrl",
      "value": "https://localhost:5001"
    }
  ],
  "item": [
    {
      "name": "Listar libros (GET)",
      "request": {
        "method": "GET",
        "url": {
          "raw": "{{baseUrl}}/api/libros",
          "host": ["{{baseUrl}}"],
          "path": ["api", "libros"]
        }
      }
    },
    {
      "name": "Obtener libro por ID (GET)",
      "request": {
        "method": "GET",
        "url": {
          "raw": "{{baseUrl}}/api/libros/1",
          "host": ["{{baseUrl}}"],
          "path": ["api", "libros", "1"]
        }
      }
    },
    {
      "name": "Crear libro (POST)",
      "request": {
        "method": "POST",
        "header": [
          { "key": "Content-Type", "value": "application/json" }
        ],
        "body": {
          "mode": "raw",
          "raw": "{\n  \"titulo\": \"Fundamentos de Bioquímica\",\n  \"autor\": \"Lehninger\",\n  \"anioPublicacion\": 2020,\n  \"editorial\": \"Omega\",\n  \"paginas\": 1240,\n  \"categoria\": \"Bioquímica\",\n  \"isbn\": \"ISBN-123456789\"\n}"
        },
        "url": {
          "raw": "{{baseUrl}}/api/libros",
          "host": ["{{baseUrl}}"],
          "path": ["api", "libros"]
        }
      }
    },
    {
      "name": "Actualizar libro (PUT)",
      "request": {
        "method": "PUT",
        "header": [
          { "key": "Content-Type", "value": "application/json" }
        ],
        "body": {
          "mode": "raw",
          "raw": "{\n  \"titulo\": \"Bioquímica Clínica Avanzada\",\n  \"autor\": \"Dr. Morales\",\n  \"anioPublicacion\": 2023,\n  \"editorial\": \"Pearson\",\n  \"paginas\": 980,\n  \"categoria\": \"Bioquímica\",\n  \"isbn\": \"ISBN-555888444\"\n}"
        },
        "url": {
          "raw": "{{baseUrl}}/api/libros/1",
          "host": ["{{baseUrl}}"],
          "path": ["api", "libros", "1"]
        }
      }
    },
    {
      "name": "Eliminar libro (DELETE)",
      "request": {
        "method": "DELETE",
        "url": {
          "raw": "{{baseUrl}}/api/libros/1",
          "host": ["{{baseUrl}}"],
          "path": ["api", "libros", "1"]
        }
      }
    }
  ]
}
```

### Cómo importarla en Postman

1. Abrir **Postman**.
2. Clic en **Import**.
3. Seleccionar el archivo `BibliotecaApi.postman_collection.json`.
4. (Opcional) Ajustar la variable `baseUrl` si tu puerto es distinto.

---

# 🔌 Endpoints de la API (Cómo consumirlos)

La API expone un CRUD completo sobre la entidad **Libro**, accesible desde el prefijo:

```text
/api/libros
```

A continuación se describen los endpoints con ejemplos.

---

## 📘 1. Listar libros activos

- **Método:** `GET`
- **URL:** `/api/libros`
- **Descripción:** Devuelve todos los libros activos usando la vista `vw_libros_activos`.

### Ejemplo de respuesta (200 OK)

```json
[
  {
    "id": 1,
    "titulo": "Bioquímica Molecular Avanzada",
    "autor": "Luis P. Gómez",
    "anioPublicacion": 2019,
    "editorial": "Elsevier",
    "paginas": 540,
    "categoria": "Bioquímica",
    "isbn": "ISBN-123"
  }
]
```

---

## 📘 2. Obtener libro por ID

- **Método:** `GET`
- **URL:** `/api/libros/{id}`

### Ejemplo de respuesta (200 OK)

```json
{
  "id": 1,
  "titulo": "Bioquímica Molecular Avanzada",
  "autor": "Luis P. Gómez",
  "anioPublicacion": 2019,
  "editorial": "Elsevier",
  "paginas": 540,
  "categoria": "Bioquímica",
  "isbn": "ISBN-123"
}
```

### Si no existe (404 Not Found)

```json
{
  "mensaje": "No existe un libro con ese ID"
}
```

---

## 🟢 3. Crear libro

- **Método:** `POST`
- **URL:** `/api/libros`
- **Content-Type:** `application/json`

### Body de ejemplo

```json
{
  "titulo": "Fundamentos de Bioquímica",
  "autor": "Lehninger",
  "anioPublicacion": 2020,
  "editorial": "Omega",
  "paginas": 1240,
  "categoria": "Bioquímica",
  "isbn": "ISBN-123456789"
}
```

### Respuesta de ejemplo (201 Created / 200 OK)

```json
{
  "mensaje": "✔ El libro fue registrado correctamente",
  "datos": {
    "id": 4,
    "titulo": "Fundamentos de Bioquímica",
    "autor": "Lehninger",
    "anioPublicacion": 2020,
    "editorial": "Omega",
    "paginas": 1240,
    "categoria": "Bioquímica",
    "isbn": "ISBN-123456789"
  }
}
```

---

## 🟡 4. Actualizar libro

- **Método:** `PUT`
- **URL:** `/api/libros/{id}`

### Body de ejemplo

```json
{
  "titulo": "Bioquímica Clínica Avanzada",
  "autor": "Dr. Morales",
  "anioPublicacion": 2023,
  "editorial": "Pearson",
  "paginas": 980,
  "categoria": "Bioquímica",
  "isbn": "ISBN-555888444"
}
```

### Respuesta de ejemplo (200 OK)

```json
{
  "mensaje": "✔ El libro se actualizó correctamente",
  "datos": {
    "id": 1,
    "titulo": "Bioquímica Clínica Avanzada",
    "autor": "Dr. Morales",
    "anioPublicacion": 2023,
    "editorial": "Pearson",
    "paginas": 980,
    "categoria": "Bioquímica",
    "isbn": "ISBN-555888444"
  }
}
```

### Si no existe (404 Not Found)

```json
{
  "mensaje": "No existe un libro con ese ID"
}
```

---

## 🔴 5. Eliminar libro (eliminado lógico)

- **Método:** `DELETE`
- **URL:** `/api/libros/{id}`
- **Descripción:** Marca el campo `activo = false` en lugar de eliminar físicamente el registro.

### Respuesta de ejemplo (200 OK)

```json
{
  "mensaje": "✔ El libro fue eliminado correctamente",
  "datos": null
}
```

### Si no existe (404 Not Found)

```json
{
  "mensaje": "No existe un libro con ese ID"
}
```

---

## 🧠 Resumen de la Arquitectura

Se aplica una **Arquitectura Limpia / en capas**, separando responsabilidades:

- **Domain (`BibliotecaApi.Domain`)**
  - Entidad `Libro`
  - Interfaces de repositorio
- **Application (`BibliotecaApi.Application`)**
  - DTOs (`LibroCrearDto`, `LibroActualizarDto`, `LibroLeerDto`, `RespuestaDto<T>`)
  - Servicio `ILibroServicio` / `LibroServicio`
  - Reglas de negocio y casos de uso (validaciones de entrada, mensajes, etc.)
- **Infrastructure (`BibliotecaApi.Infrastructure`)**
  - `AppDbContext` (EF Core + Npgsql)
  - `LibroRepositorio` (implementa `ILibroRepositorio`)
  - Uso de `vw_libros_activos` para listar solo registros activos
- **API (`BibliotecaApi`)**
  - `LibrosController` expone los endpoints REST
  - Configuración de `Program.cs`, middlewares, DI, Swagger
  - Lista para ser auto hospedada o desplegada en Lambda

---

## ☁️ Despliegue en AWS Lambda (Opcional)

El proyecto puede ejecutarse como API tradicional (Kestrel) o desplegarse en **AWS Lambda** usando `Amazon.Lambda.AspNetCoreServer.Hosting`.

### 1. Agregar hosting para Lambda

En `Program.cs`:

```csharp
using Amazon.Lambda.AspNetCoreServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
```

### 2. Publicar para Linux

```bash
dotnet publish BibliotecaApi/BibliotecaApi.csproj -c Release -r linux-x64 --self-contained false -o ./publish
```

### 3. Crear función Lambda en AWS

1. Crear una función Lambda con runtime `.NET 8`.
2. Subir el ZIP generado desde la carpeta `publish/`.
3. Configurar integración con **API Gateway HTTP API**.
4. Probar los mismos endpoints (`/api/libros`) ahora servidos desde AWS.

> Esta sección es opcional, pero deja documentado cómo se podría desplegar la API en la nube.

---

## 👤 Autor

**Alexander Omar Taquiri Paucar**  
Senior .NET / Arquitectura Limpia / APIs REST / PostgreSQL / AWS Básico
