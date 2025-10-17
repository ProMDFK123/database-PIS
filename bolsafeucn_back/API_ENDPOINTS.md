# 📚 Documentación Completa de API - BolsaFE UCN

> **Versión:** 1.0  
> **Framework:** ASP.NET Core 9.0  
> **Fecha:** Octubre 17, 2025  
> **Base URL:** `http://localhost:5185/api` o `https://your-domain.com/api`

---

## 📑 Tabla de Contenidos

1. [🔐 Autenticación (AuthController)](#-autenticación-authcontroller)
2. [📰 Publicaciones (PublicationController)](#-publicaciones-publicationcontroller)
3. [📝 Postulaciones (JobApplicationController)](#-postulaciones-jobapplicationcontroller)
4. [🔒 Seguridad y JWT](#-seguridad-y-jwt)
5. [📊 Códigos de Estado HTTP](#-códigos-de-estado-http)
6. [🧪 Colección de Postman](#-colección-de-postman)

---

## 🔐 Autenticación (AuthController)

**Base URL:** `/api/auth`

Todos los endpoints de autenticación y registro de usuarios.

### 1. Registrar Estudiante

```http
POST /api/auth/register/student
Content-Type: application/json

Body:
{
  "email": "juan.perez@alumnos.ucn.cl",
  "userName": "juanperez",
  "password": "Password123!",
  "confirmPassword": "Password123!",
  "rut": "12.345.678-9",
  "name": "Juan",
  "lastName": "Pérez",
  "phone": "+56912345678",
  "career": "Ingeniería Civil en Computación"
}

Response 200 OK:
{
  "message": "Estudiante registrado exitosamente. Por favor verifica tu email."
}

Response 400 Bad Request:
{
  "message": "El email ya está registrado"
}
```

**Validaciones:**
- ✅ Email debe ser `@alumnos.ucn.cl`
- ✅ Password mínimo 8 caracteres, 1 mayúscula, 1 minúscula, 1 número
- ✅ Password y ConfirmPassword deben coincidir
- ✅ RUT válido con formato chileno

---

### 2. Registrar Particular/Individual

```http
POST /api/auth/register/individual
Content-Type: application/json

Body:
{
  "email": "maria.gonzalez@gmail.com",
  "userName": "mariaglez",
  "password": "Password123!",
  "confirmPassword": "Password123!",
  "rut": "15.678.234-5",
  "name": "María",
  "lastName": "González",
  "phone": "+56987654321"
}

Response 200 OK:
{
  "message": "Particular registrado exitosamente. Por favor verifica tu email."
}
```

**Validaciones:**
- ✅ Email válido (cualquier dominio)
- ✅ Password mínimo 8 caracteres
- ✅ RUT válido

---

### 3. Registrar Empresa

```http
POST /api/auth/register/company
Content-Type: application/json

Body:
{
  "email": "rrhh@techcorp.cl",
  "userName": "techcorp",
  "password": "Password123!",
  "confirmPassword": "Password123!",
  "rut": "76.123.456-7",
  "companyName": "TechCorp SpA",
  "businessSector": "Tecnología",
  "phone": "+56222334455",
  "address": "Av. Angamos 0610, Antofagasta"
}

Response 200 OK:
{
  "message": "Empresa registrada exitosamente. Por favor verifica tu email."
}
```

**Validaciones:**
- ✅ Email válido
- ✅ RUT de empresa (formato chileno)
- ✅ Nombre de empresa requerido

---

### 4. Registrar Administrador

```http
POST /api/auth/register/admin
Content-Type: application/json

Body:
{
  "email": "admin@bolsafe.ucn.cl",
  "userName": "adminucn",
  "password": "SuperSecure123!",
  "confirmPassword": "SuperSecure123!",
  "rut": "18.234.567-8"
}

Response 200 OK:
{
  "message": "Administrador registrado exitosamente. Por favor verifica tu email."
}
```

**⚠️ Nota:** Este endpoint debería estar protegido en producción.

---

### 5. Verificar Email

```http
POST /api/auth/verify-email
Content-Type: application/json

Body:
{
  "email": "juan.perez@alumnos.ucn.cl",
  "verificationCode": "123456"
}

Response 200 OK:
{
  "message": "Email verificado exitosamente. Ya puedes iniciar sesión."
}

Response 400 Bad Request:
{
  "message": "Código de verificación inválido o expirado"
}
```

**Validaciones:**
- ✅ Código de 6 dígitos
- ✅ Código válido y no expirado (15 minutos)
- ✅ Email debe existir y no estar verificado

---

### 6. Iniciar Sesión (Login)

```http
POST /api/auth/login
Content-Type: application/json

Body:
{
  "email": "juan.perez@alumnos.ucn.cl",
  "password": "Password123!"
}

Response 200 OK:
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6Ikp1YW4gUGVyZXoiLCJpYXQiOjE1MTYyMzkwMjJ9.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"
}

Response 401 Unauthorized:
{
  "message": "Email o contraseña incorrectos"
}

Response 403 Forbidden:
{
  "message": "Debes verificar tu email antes de iniciar sesión"
}
```

**Validaciones:**
- ✅ Email verificado
- ✅ Usuario no baneado
- ✅ Credenciales correctas

**JWT Token incluye:**
```json
{
  "sub": "123",                          // User ID
  "email": "juan.perez@alumnos.ucn.cl",
  "nameid": "123",                       // ClaimTypes.NameIdentifier
  "role": "Student",                      // User role
  "iat": 1697558400,                     // Issued at
  "exp": 1697644800                      // Expiration (24 hours)
}
```

---

## 📰 Publicaciones (PublicationController)

**Base URL:** `/api/publications`

Todos los endpoints relacionados con publicaciones (ofertas laborales, voluntariados y compra/venta).

---

## 📋 Endpoints Disponibles


### 🔐 Crear Publicaciones (Requiere autenticación JWT)

#### 1. Crear Oferta Laboral/Voluntariado

```http
POST /api/publications/offers
Authorization: Bearer {JWT_TOKEN}
Content-Type: application/json

Body:
{
  "title": "Desarrollador Backend",
  "description": "Buscamos desarrollador con experiencia en .NET",
  "endDate": "2025-12-31T23:59:59Z",
  "deadlineDate": "2025-11-30T23:59:59Z",
  "remuneration": 1500000,
  "offerType": 0,
  "location": "Antofagasta, Chile",
  "requirements": "2 años de experiencia en C#",
  "contactInfo": "rrhh@empresa.cl",
  "imagesURL": ["https://example.com/image1.jpg"]
}

Response 200 OK:
{
  "message": "Oferta creada exitosamente",
  "data": "Oferta ID: 15"
}

Response 401 Unauthorized:
{
  "message": "Usuario no autenticado",
  "data": null
}
```

**OfferType:**
- `0` = Trabajo (puede tener remuneración)
- `1` = Voluntariado (remuneración DEBE ser 0)

**Validaciones:**
- ✅ `title`: 5-200 caracteres
- ✅ `description`: 10-2000 caracteres
- ✅ `deadlineDate` debe ser futura
- ✅ `endDate` debe ser posterior a `deadlineDate`
- ✅ Voluntariados (`offerType = 1`) deben tener `remuneration = 0`
- ✅ `remuneration`: 0 - $100.000.000
- ✅ `imagesURL`: máximo 10 imágenes

---

#### 2. Crear Publicación de Compra/Venta

```http
POST /api/publications/buysells
Authorization: Bearer {JWT_TOKEN}
Content-Type: application/json

Body:
{
  "title": "Notebook HP Pavilion",
  "description": "Laptop en excelente estado, poco uso",
  "category": "Electrónica",
  "price": 450000,
  "imagesURL": ["https://example.com/laptop1.jpg"],
  "location": "Antofagasta",
  "contactInfo": "+56912345678"
}

Response 200 OK:
{
  "message": "Publicación de compra/venta creada exitosamente",
  "data": "Publicación ID: 8"
}
```

**Validaciones:**
- ✅ `title`: 5-200 caracteres
- ✅ `description`: 10-2000 caracteres
- ✅ `category`: requerido, máximo 100 caracteres
- ✅ `price`: 0 - $100.000.000
- ✅ `imagesURL`: máximo 10 imágenes
- ✅ `location`: opcional, máximo 200 caracteres
- ✅ `contactInfo`: opcional, máximo 200 caracteres

---

### 🌐 Obtener Ofertas Laborales (Público - sin autenticación)

#### 3. Listar Ofertas Activas

```http
GET /api/publications/offers

Response 200 OK:
{
  "message": "Ofertas recuperadas exitosamente",
  "data": [
    {
      "id": 1,
      "title": "Desarrollador Frontend",
      "description": "Buscamos...",
      "remuneration": 1200000,
      "location": "Antofagasta",
      "offerType": 0,
      "deadlineDate": "2025-11-30T23:59:59Z",
      "endDate": "2025-12-31T23:59:59Z",
      "companyName": "Tech Corp",
      "isActive": true
    }
  ]
}
```

---

#### 4. Ver Detalles de Oferta

```http
GET /api/publications/offers/{id}

Ejemplo: GET /api/publications/offers/1

Response 200 OK:
{
  "message": "Detalles de oferta recuperados exitosamente",
  "data": {
    "id": 1,
    "title": "Desarrollador Frontend",
    "description": "Descripción completa...",
    "remuneration": 1200000,
    "location": "Antofagasta",
    "requirements": "React, TypeScript, 2 años exp",
    "contactInfo": "jobs@techcorp.cl",
    "offerType": 0,
    "endDate": "2025-12-31T23:59:59Z",
    "deadlineDate": "2025-11-30T23:59:59Z",
    "publicationDate": "2025-10-17T10:00:00Z",
    "companyName": "Tech Corp",
    "companyEmail": "contact@techcorp.cl",
    "imageUrls": ["https://..."],
    "isActive": true
  }
}

Response 404 Not Found:
{
  "message": "Oferta no encontrada",
  "data": null
}
```

---

### 🛒 Obtener Publicaciones de Compra/Venta (Público)

#### 5. Listar Publicaciones de Compra/Venta Activas

```http
GET /api/publications/buysells

Response 200 OK:
{
  "message": "Publicaciones de compra/venta recuperadas exitosamente",
  "data": [
    {
      "id": 1,
      "title": "Notebook HP Pavilion",
      "category": "Electrónica",
      "price": 450000,
      "location": "Antofagasta",
      "publicationDate": "2025-10-15T14:30:00Z",
      "firstImageUrl": "https://example.com/laptop.jpg",
      "userId": 5,
      "userName": "JuanPerez123"
    }
  ]
}
```

---

#### 6. Ver Detalles de Publicación de Compra/Venta

```http
GET /api/publications/buysells/{id}

Ejemplo: GET /api/publications/buysells/1

Response 200 OK:
{
  "message": "Detalles de publicación recuperados exitosamente",
  "data": {
    "id": 1,
    "title": "Notebook HP Pavilion",
    "description": "Laptop en excelente estado, 8GB RAM, 256GB SSD",
    "category": "Electrónica",
    "price": 450000,
    "location": "Antofagasta",
    "contactInfo": "+56912345678",
    "publicationDate": "2025-10-15T14:30:00Z",
    "isActive": true,
    "imageUrls": [
      "https://example.com/laptop1.jpg",
      "https://example.com/laptop2.jpg"
    ],
    "userId": 5,
    "userName": "JuanPerez123",
    "userEmail": "juan.perez@alumnos.ucn.cl"
  }
}

Response 404 Not Found:
{
  "message": "Publicación no encontrada",
  "data": null
}
```

---

### 📝 Postulaciones a Ofertas (JWT Required - Solo estudiantes)

#### 7. Postular a una Oferta

```http
POST /api/publications/offers/{id}/apply
Authorization: Bearer {JWT_TOKEN}
Content-Type: application/json

Ejemplo: POST /api/publications/offers/15/apply

Body:
{
  "motivationLetter": "Me interesa esta posición porque tengo experiencia en .NET y busco crecer profesionalmente..."
}

Response 200 OK:
{
  "message": "Postulación creada exitosamente",
  "data": {
    "id": 42,
    "studentName": "Juan Pérez González",
    "studentEmail": "juan.perez@alumnos.ucn.cl",
    "offerTitle": "Desarrollador Frontend",
    "status": "Pendiente",
    "applicationDate": "2025-10-17T16:30:00Z",
    "curriculumVitae": "https://storage.com/cv/juan_perez.pdf",
    "motivationLetter": "Me interesa esta posición porque..."
  }
}

Response 400 Bad Request:
{
  "message": "Ya has postulado a esta oferta",
  "data": null
}

Response 401 Unauthorized:
{
  "message": "El estudiante no es elegible para postular",
  "data": null
}

Response 404 Not Found:
{
  "message": "La oferta no existe o no está activa",
  "data": null
}

Response 409 Conflict:
{
  "message": "La fecha límite para postular a esta oferta ha expirado",
  "data": null
}
```

**Validaciones automáticas:**
- ✅ Usuario debe ser estudiante
- ✅ Email `@alumnos.ucn.cl`
- ✅ No puede estar baneado
- ✅ Debe tener CV cargado
- ✅ La oferta debe estar activa
- ✅ No debe haber vencido la `deadlineDate`
- ✅ La oferta no debe haber finalizado (`endDate`)
- ✅ No puede haber postulado anteriormente

---

#### 8. Ver Mis Postulaciones

```http
GET /api/publications/offers/my-applications
Authorization: Bearer {JWT_TOKEN}

Response 200 OK:
{
  "message": "Postulaciones recuperadas exitosamente",
  "data": [
    {
      "id": 42,
      "studentName": "Juan Pérez González",
      "studentEmail": "juan.perez@alumnos.ucn.cl",
      "offerTitle": "Desarrollador Frontend",
      "status": "Pendiente",
      "applicationDate": "2025-10-17T16:30:00Z",
      "curriculumVitae": "https://storage.com/cv/juan_perez.pdf",
      "motivationLetter": "Me interesa..."
    },
    {
      "id": 38,
      "studentName": "Juan Pérez González",
      "studentEmail": "juan.perez@alumnos.ucn.cl",
      "offerTitle": "Asistente de Investigación",
      "status": "Aceptado",
      "applicationDate": "2025-10-10T10:00:00Z",
      "curriculumVitae": "https://storage.com/cv/juan_perez.pdf",
      "motivationLetter": null
    }
  ]
}

Response 401 Unauthorized:
{
  "message": "No autenticado o token inválido",
  "data": null
}
```

---

## � Postulaciones (JobApplicationController)

**Base URL:** `/api/job-applications`

Gestión avanzada de postulaciones para estudiantes y empresas.

---

### Para Estudiantes

#### 1. Postular a Oferta (Alternativa)

```http
POST /api/job-applications/apply/{offerId}
Authorization: Bearer {JWT_TOKEN}
Content-Type: application/json

Body:
{
  "motivationLetter": "Carta de motivación opcional..."
}

Response 200 OK:
{
  "message": "Postulación creada exitosamente",
  "data": {
    "id": 42,
    "studentName": "Juan Pérez",
    "studentEmail": "juan.perez@alumnos.ucn.cl",
    "offerTitle": "Desarrollador Backend",
    "status": "Pendiente",
    "applicationDate": "2025-10-17T16:30:00Z",
    "curriculumVitae": "https://...",
    "motivationLetter": "..."
  }
}
```

---

#### 2. Ver Mis Postulaciones (Alternativa)

```http
GET /api/job-applications/my-applications
Authorization: Bearer {JWT_TOKEN}

Response 200 OK:
{
  "message": "Postulaciones recuperadas exitosamente",
  "data": [...]
}
```

---

### Para Empresas

#### 3. Ver Postulaciones de una Oferta

```http
GET /api/job-applications/offer/{offerId}
Authorization: Bearer {JWT_TOKEN}

Ejemplo: GET /api/job-applications/offer/15

Response 200 OK:
{
  "message": "Postulaciones de la oferta 15 recuperadas exitosamente",
  "data": [
    {
      "id": 42,
      "studentName": "Juan Pérez González",
      "studentEmail": "juan.perez@alumnos.ucn.cl",
      "offerTitle": "Desarrollador Backend",
      "status": "Pendiente",
      "applicationDate": "2025-10-17T16:30:00Z",
      "curriculumVitae": "https://storage.com/cv/juan_perez.pdf",
      "motivationLetter": "Me interesa..."
    },
    {
      "id": 43,
      "studentName": "María González",
      "studentEmail": "maria.gonzalez@alumnos.ucn.cl",
      "offerTitle": "Desarrollador Backend",
      "status": "Pendiente",
      "applicationDate": "2025-10-17T17:00:00Z",
      "curriculumVitae": "https://storage.com/cv/maria_gonzalez.pdf",
      "motivationLetter": null
    }
  ]
}
```

**Seguridad:** Solo el creador de la oferta puede ver las postulaciones.

---

#### 4. Ver Todas las Postulaciones de Mis Ofertas

```http
GET /api/job-applications/my-offers-applications
Authorization: Bearer {JWT_TOKEN}

Response 200 OK:
{
  "message": "Postulaciones recibidas recuperadas exitosamente",
  "data": [
    {
      "id": 42,
      "studentName": "Juan Pérez González",
      "studentEmail": "juan.perez@alumnos.ucn.cl",
      "offerTitle": "Desarrollador Backend",
      "status": "Pendiente",
      "applicationDate": "2025-10-17T16:30:00Z",
      "curriculumVitae": "https://...",
      "motivationLetter": "..."
    },
    {
      "id": 43,
      "studentName": "María González",
      "studentEmail": "maria.gonzalez@alumnos.ucn.cl",
      "offerTitle": "Diseñador UX/UI",
      "status": "Aceptado",
      "applicationDate": "2025-10-15T10:00:00Z",
      "curriculumVitae": "https://...",
      "motivationLetter": null
    }
  ]
}
```

**Nota:** Retorna todas las postulaciones de TODAS las ofertas creadas por la empresa autenticada.

---

#### 5. Actualizar Estado de Postulación

```http
PATCH /api/job-applications/{applicationId}/status
Authorization: Bearer {JWT_TOKEN}
Content-Type: application/json

Ejemplo: PATCH /api/job-applications/42/status

Body:
{
  "newStatus": "Aceptado"
}

Response 200 OK:
{
  "message": "Estado de postulación actualizado a 'Aceptado' exitosamente",
  "data": null
}

Response 401 Unauthorized:
{
  "message": "No tienes permiso para modificar esta postulación",
  "data": null
}

Response 400 Bad Request:
{
  "message": "Estado inválido. Debe ser uno de: Pendiente, Aceptado, Rechazado",
  "data": null
}
```

**Estados válidos:**
- `Pendiente`
- `Aceptado`
- `Rechazado`

**Seguridad:** Solo el creador de la oferta puede actualizar el estado.

---

## 🔒 Seguridad y JWT

---

## 🔒 Seguridad y JWT

### Obtención del Token

Utiliza el endpoint `/api/auth/login` con credenciales válidas:

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "usuario@alumnos.ucn.cl",
  "password": "TuPassword123!"
}

Response:
{
  "message": "Login exitoso",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": 5,
      "email": "usuario@alumnos.ucn.cl",
      "role": "Student",
      "name": "Juan Pérez"
    }
  }
}
```

### Usar el Token en Postman

1. **Copiar el token JWT** de la respuesta del login
2. **Ir al tab "Authorization"** en Postman
3. **Seleccionar Type:** `Bearer Token`
4. **Pegar el token** en el campo Token
5. **Hacer la petición** normalmente

**Formato del Header (generado automáticamente):**
```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Información contenida en el JWT

El token contiene claims con información del usuario:
- `NameIdentifier`: ID del usuario en la base de datos
- `Email`: Email del usuario autenticado
- `Role`: Rol del usuario (`Student`, `Individual`, `Company`, `Admin`)

**Expiración:** El token es válido por **7 días** desde la emisión.

### Endpoints Protegidos (Requieren JWT)

| Endpoint | Método | Requiere Auth | Rol Permitido |
|----------|--------|---------------|---------------|
| `/api/publications/offers` | POST | ✅ Sí | Company |
| `/api/publications/buysells` | POST | ✅ Sí | Cualquiera |
| `/api/publications/offers/{id}/apply` | POST | ✅ Sí | Student |
| `/api/publications/offers/my-applications` | GET | ✅ Sí | Student |
| `/api/job-applications/*` | * | ✅ Sí | Según endpoint |
| `/api/publications/offers` | GET | ❌ No | Público |
| `/api/publications/buysells` | GET | ❌ No | Público |
| `/api/publications/offers/{id}` | GET | ❌ No | Público |
| `/api/publications/buysells/{id}` | GET | ❌ No | Público |
| `/api/auth/*` | POST | ❌ No | Público |

---

## 🧪 Cómo Probar en Postman

### Paso 1: Registrar Usuario

```http
POST /api/auth/register/student
Content-Type: application/json

{
  "email": "test.student@alumnos.ucn.cl",
  "password": "Test123!",
  "confirmPassword": "Test123!",
  "userName": "TestStudent",
  "firstName": "Test",
  "lastName": "Student",
  "phoneNumber": "+56912345678",
  "career": "Ingeniería Civil en Computación",
  "curriculumVitae": "https://example.com/cv.pdf"
}
```

### Paso 2: Verificar Email

```http
POST /api/auth/verify-email
Content-Type: application/json

{
  "email": "test.student@alumnos.ucn.cl",
  "token": "{token_recibido_por_email}"
}
```

### Paso 3: Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "test.student@alumnos.ucn.cl",
  "password": "Test123!"
}
```

**Copiar el campo `token` de la respuesta.**

### Paso 4: Configurar Authorization en Postman

1. Click en el tab **"Authorization"**
2. Seleccionar **Type: Bearer Token**
3. Pegar el token copiado
4. ✅ Listo para usar endpoints protegidos

### Paso 5: Probar Endpoint Protegido

```http
GET /api/publications/offers/my-applications
Authorization: Bearer {tu_token_aquí}
```

---

## 🆘 Errores Comunes y Soluciones

### Error 401: Unauthorized

```json
{
  "message": "Usuario no autenticado",
  "data": null
}
```

**Causas:**
- ❌ No incluiste el header `Authorization`
- ❌ Token JWT inválido
- ❌ Token expirado (más de 7 días)
- ❌ Formato incorrecto (debe ser `Bearer {token}`)

**Soluciones:**
1. Verifica que el token esté en el header
2. Verifica el formato: `Authorization: Bearer eyJhbG...`
3. Genera un nuevo token con `/api/auth/login`

---

### Error 400: Bad Request (Validación)

```json
{
  "message": "Errores de validación",
  "data": [
    {
      "field": "title",
      "error": "El título debe tener entre 5 y 200 caracteres"
    },
    {
      "field": "remuneration",
      "error": "Los voluntariados deben tener remuneración 0"
    }
  ]
}
```

**Solución:** Revisa los datos enviados y corrige según las validaciones documentadas.

---

### Error 404: Not Found

```json
{
  "message": "Oferta no encontrada",
  "data": null
}
```

**Solución:** Verifica que el ID del recurso sea correcto y que el recurso exista.

---

### Error 409: Conflict

```json
{
  "message": "Ya has postulado a esta oferta",
  "data": null
}
```

**Solución:** Este estudiante ya tiene una postulación activa para esta oferta.

---

### Error 403: Forbidden

```json
{
  "message": "No tienes permiso para realizar esta acción",
  "data": null
}
```

**Causas:**
- Intentaste crear una oferta sin ser Company
- Intentaste postular sin ser Student
- Intentaste modificar una postulación que no es tuya

**Solución:** Verifica que tu usuario tenga el rol correcto para esta acción.

---

## 🧩 Colección de Postman

### Importar Colección (JSON)

Copia y pega este JSON en Postman (Import → Raw text):

```json
{
  "info": {
    "name": "BolsaFE UCN API",
    "description": "Colección completa de endpoints para BolsaFE UCN Backend",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "variable": [
    {
      "key": "baseUrl",
      "value": "https://localhost:7169",
      "type": "string"
    },
    {
      "key": "jwt_token",
      "value": "",
      "type": "string"
    }
  ],
  "item": [
    {
      "name": "Auth",
      "item": [
        {
          "name": "Register Student",
          "request": {
            "method": "POST",
            "header": [{"key": "Content-Type", "value": "application/json"}],
            "url": "{{baseUrl}}/api/auth/register/student",
            "body": {
              "mode": "raw",
              "raw": "{\n  \"email\": \"test@alumnos.ucn.cl\",\n  \"password\": \"Test123!\",\n  \"confirmPassword\": \"Test123!\",\n  \"userName\": \"TestUser\",\n  \"firstName\": \"Test\",\n  \"lastName\": \"User\",\n  \"phoneNumber\": \"+56912345678\",\n  \"career\": \"Ing. Civil\",\n  \"curriculumVitae\": \"https://cv.com/test.pdf\"\n}"
            }
          }
        },
        {
          "name": "Login",
          "request": {
            "method": "POST",
            "header": [{"key": "Content-Type", "value": "application/json"}],
            "url": "{{baseUrl}}/api/auth/login",
            "body": {
              "mode": "raw",
              "raw": "{\n  \"email\": \"test@alumnos.ucn.cl\",\n  \"password\": \"Test123!\"\n}"
            }
          },
          "event": [
            {
              "listen": "test",
              "script": {
                "exec": [
                  "var jsonData = pm.response.json();",
                  "pm.collectionVariables.set('jwt_token', jsonData.data.token);"
                ]
              }
            }
          ]
        }
      ]
    },
    {
      "name": "Publications - Offers",
      "item": [
        {
          "name": "Create Offer",
          "request": {
            "method": "POST",
            "header": [
              {"key": "Content-Type", "value": "application/json"},
              {"key": "Authorization", "value": "Bearer {{jwt_token}}"}
            ],
            "url": "{{baseUrl}}/api/publications/offers",
            "body": {
              "mode": "raw",
              "raw": "{\n  \"title\": \"Desarrollador Backend\",\n  \"description\": \"Buscamos desarrollador .NET\",\n  \"endDate\": \"2025-12-31T23:59:59Z\",\n  \"deadlineDate\": \"2025-11-30T23:59:59Z\",\n  \"remuneration\": 1500000,\n  \"offerType\": 0,\n  \"location\": \"Antofagasta\",\n  \"requirements\": \"2 años C#\",\n  \"contactInfo\": \"hr@company.cl\",\n  \"imagesURL\": []\n}"
            }
          }
        },
        {
          "name": "Get All Offers",
          "request": {
            "method": "GET",
            "url": "{{baseUrl}}/api/publications/offers"
          }
        },
        {
          "name": "Get Offer By ID",
          "request": {
            "method": "GET",
            "url": "{{baseUrl}}/api/publications/offers/1"
          }
        },
        {
          "name": "Apply to Offer",
          "request": {
            "method": "POST",
            "header": [
              {"key": "Content-Type", "value": "application/json"},
              {"key": "Authorization", "value": "Bearer {{jwt_token}}"}
            ],
            "url": "{{baseUrl}}/api/publications/offers/1/apply",
            "body": {
              "mode": "raw",
              "raw": "{\n  \"motivationLetter\": \"Me interesa porque...\"\n}"
            }
          }
        },
        {
          "name": "My Applications",
          "request": {
            "method": "GET",
            "header": [
              {"key": "Authorization", "value": "Bearer {{jwt_token}}"}
            ],
            "url": "{{baseUrl}}/api/publications/offers/my-applications"
          }
        }
      ]
    },
    {
      "name": "Publications - Buy/Sell",
      "item": [
        {
          "name": "Create Buy/Sell",
          "request": {
            "method": "POST",
            "header": [
              {"key": "Content-Type", "value": "application/json"},
              {"key": "Authorization", "value": "Bearer {{jwt_token}}"}
            ],
            "url": "{{baseUrl}}/api/publications/buysells",
            "body": {
              "mode": "raw",
              "raw": "{\n  \"title\": \"Notebook HP\",\n  \"description\": \"Laptop excelente estado\",\n  \"category\": \"Electrónica\",\n  \"price\": 450000,\n  \"imagesURL\": [],\n  \"location\": \"Antofagasta\",\n  \"contactInfo\": \"+56912345678\"\n}"
            }
          }
        },
        {
          "name": "Get All Buy/Sells",
          "request": {
            "method": "GET",
            "url": "{{baseUrl}}/api/publications/buysells"
          }
        },
        {
          "name": "Get Buy/Sell By ID",
          "request": {
            "method": "GET",
            "url": "{{baseUrl}}/api/publications/buysells/1"
          }
        }
      ]
    },
    {
      "name": "Job Applications",
      "item": [
        {
          "name": "Get My Offers Applications (Company)",
          "request": {
            "method": "GET",
            "header": [
              {"key": "Authorization", "value": "Bearer {{jwt_token}}"}
            ],
            "url": "{{baseUrl}}/api/job-applications/my-offers-applications"
          }
        },
        {
          "name": "Get Applications By Offer (Company)",
          "request": {
            "method": "GET",
            "header": [
              {"key": "Authorization", "value": "Bearer {{jwt_token}}"}
            ],
            "url": "{{baseUrl}}/api/job-applications/offer/1"
          }
        },
        {
          "name": "Update Application Status",
          "request": {
            "method": "PATCH",
            "header": [
              {"key": "Content-Type", "value": "application/json"},
              {"key": "Authorization", "value": "Bearer {{jwt_token}}"}
            ],
            "url": "{{baseUrl}}/api/job-applications/1/status",
            "body": {
              "mode": "raw",
              "raw": "{\n  \"newStatus\": \"Aceptado\"\n}"
            }
          }
        }
      ]
    }
  ]
}
```

### Configurar Variables de Postman

1. Importa la colección
2. Variables pre-configuradas:
   - `baseUrl`: `https://localhost:7169` (cambiar según tu entorno)
   - `jwt_token`: Se auto-completa al hacer login

3. **Automático:** El token se guarda automáticamente al hacer login exitoso

---

## 🧪 Testing y Validación

### Flujo de Testing Recomendado

1. **Registrar usuarios de diferentes tipos**
   - Student: `student@alumnos.ucn.cl`
   - Company: `company@empresa.cl`
   - Individual: `individual@ucn.cl`

2. **Verificar emails** (si SMTP configurado)

3. **Login con cada usuario** y guardar tokens

4. **Como Company:**
   - Crear ofertas laborales
   - Crear ofertas de voluntariado
   - Ver postulaciones recibidas
   - Actualizar estados de postulaciones

5. **Como Student:**
   - Ver ofertas disponibles
   - Postular a ofertas
   - Ver mis postulaciones

6. **Como cualquier usuario:**
   - Crear publicaciones de compra/venta
   - Ver publicaciones activas

7. **Probar casos de error:**
   - Postular dos veces a la misma oferta
   - Crear oferta sin autenticación
   - Postular con usuario Company
   - Actualizar postulación de oferta ajena

---

### JWT Token
Los endpoints que requieren autenticación necesitan un token JWT válido en el header:

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

El token debe contener el claim `ClaimTypes.NameIdentifier` con el ID del usuario.

---

## 📋 Estructura de Respuesta Genérica

Todos los endpoints retornan el mismo formato:

```json
{
  "message": "Descripción del resultado (en español)",
  "data": {
    // Datos específicos del endpoint
    // Puede ser: objeto, array, string, number o null
  }
}
```

**Ejemplos:**

```json
// Éxito con datos
{
  "message": "Ofertas recuperadas exitosamente",
  "data": [...]
}

// Éxito sin datos adicionales
{
  "message": "Estado actualizado exitosamente",
  "data": null
}

// Error
{
  "message": "Email ya registrado",
  "data": null
}
```

---

## 📊 Códigos de Estado HTTP

| Código | Icono | Descripción | Cuándo ocurre |
|--------|-------|-------------|---------------|
| 200 | ✅ | OK - Petición exitosa | Request procesado correctamente |
| 201 | ✅ | Created - Recurso creado | POST exitoso (creación) |
| 400 | ⚠️ | Bad Request - Solicitud inválida | Errores de validación, datos incorrectos |
| 401 | 🔒 | Unauthorized - No autenticado | Token JWT inválido, expirado o ausente |
| 403 | 🚫 | Forbidden - Sin permisos | Usuario autenticado pero sin permisos |
| 404 | ❌ | Not Found - No encontrado | Recurso no existe |
| 409 | ⚡ | Conflict - Conflicto | Ya existe (ej: ya postulado) |
| 500 | 💥 | Internal Server Error | Error inesperado del servidor |

---

## 🎨 Estructura de Respuestas

Todas las respuestas siguen el formato `GenericResponse<T>`:

```typescript
{
  "message": string,    // Mensaje descriptivo
  "data": T | null      // Datos de respuesta (tipo genérico)
}
```

---

## 📝 Notas Importantes

### Ofertas Laborales vs Voluntariados
- **Trabajo** (`offerType = 0`): Puede tener cualquier remuneración ≥ $0
- **Voluntariado** (`offerType = 1`): Remuneración DEBE ser $0

### Diferencias entre Publicaciones
- **Ofertas (`/offers`)**: Para trabajos y voluntariados. Incluyen fechas límite, remuneración, y permiten postulaciones de estudiantes.
- **Compra/Venta (`/buysells`)**: Para vender productos o servicios. Incluyen precio, categoría, y son publicaciones simples sin sistema de postulación.

### Seguridad
- ✅ El `studentId` se obtiene automáticamente del token JWT
- ✅ No es posible postular como otro usuario
- ✅ Los endpoints de creación requieren autenticación
- ✅ Los endpoints de listado son públicos

---

## 🚀 Ejemplo de Flujo Completo

### 1. Usuario crea una oferta laboral
```http
POST /api/publications/offers
Authorization: Bearer {token}
→ Response: Oferta ID: 15
```

### 2. Estudiantes consultan ofertas disponibles
```http
GET /api/publications/offers
→ Response: Lista de 10 ofertas activas
```

### 3. Estudiante ve detalles de oferta
```http
GET /api/publications/offers/15
→ Response: Detalles completos de la oferta
```

### 4. Estudiante postula a la oferta
```http
POST /api/publications/offers/15/apply
Authorization: Bearer {student_token}
→ Response: Postulación creada exitosamente
```

### 5. Estudiante verifica sus postulaciones
```http
GET /api/publications/offers/my-applications
Authorization: Bearer {student_token}
→ Response: Lista con todas sus postulaciones
```

---

## � Recursos Adicionales

- **Swagger UI:** `https://localhost:7169/swagger` (solo en Development)
- **Logs:** Archivos JSON estructurados en carpeta `logs/`
- **Base de Datos:** PostgreSQL con Entity Framework Core
- **Migraciones:** Ver carpeta `Migrations/`
- **Autenticación:** JWT Bearer tokens (HS256)
- **Tiempo de expiración token:** 7 días

---

## 📝 Limitaciones Actuales (V1.0)

| Característica | Estado | Notas |
|----------------|--------|-------|
| Paginación | ❌ No implementado | Endpoints retornan todos los registros activos |
| Filtros | ❌ No implementado | Futuro: filtrar por categoría, precio, ubicación |
| Búsqueda | ❌ No implementado | Futuro: búsqueda por texto/keywords |
| Ordenamiento | ✅ Por fecha | Más reciente primero |
| Soft Delete | ✅ Implementado | `isActive = false` en lugar de eliminación física |
| Rate Limiting | ❌ No implementado | Sin protección contra spam |
| File Upload | ❌ No implementado | URLs de imágenes/CV son strings |
| Email Service | ⚠️ Pendiente | Verificación de email configurada pero requiere SMTP |

### Próximas Características (Roadmap)

- [ ] Paginación y límite de resultados
- [ ] Búsqueda full-text en publicaciones
- [ ] Filtros avanzados (precio, ubicación, categoría)
- [ ] Subida de archivos (imágenes, CV)
- [ ] Notificaciones por email
- [ ] Sistema de favoritos/guardados
- [ ] Estadísticas y analytics
- [ ] Rate limiting por IP/usuario
- [ ] Moderación de contenido
- [ ] Sistema de reportes

---

## �📞 Contacto y Soporte

Para más información sobre la API, consultar la documentación Swagger en:
```
https://localhost:7169/swagger
```

---

**📅 Última actualización:** 17 de Octubre 2025  
**🔢 Versión API:** 1.0  
**👨‍💻 Desarrollado con:** ASP.NET Core 9.0 + PostgreSQL + Entity Framework Core
