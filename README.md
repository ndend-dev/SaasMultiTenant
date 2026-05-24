# Saas Multi-Tenant 

[![Angular](https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white)](https://angular.io/)
[![.NET 9](https://img.shields.io/badge/.NET_9-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-CC292B?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/sql-server)
[![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/)

Este repositorio contiene la configuración para desplegar el entorno de desarrollo del proyecto SaasMultiTenant utilizando Docker Compose. Levanta el Frontend (Angular), el Backend (Web API .NET 9) y la Base de Datos (SQL Server 2022) inicializada automáticamente.

---

## Requisitos Previos

* Git
* Docker Desktop (con el motor de Docker ejecutándose)
* SQL Server Management Studio (SSMS) u otra herramienta externa (Opcional)

---

## Estructura del Proyecto

```text
SaasMultiTenant/
├── Backend/               # Código de la Web API (.NET 9)
├── Frontend/              # Código de Angular
├── Database/              # Inicialización de la Base de Datos
│   ├── Dockerfile         # Imagen personalizada de SQL Server
│   ├── script.sql         # Script SQL inicial y datos semilla
│   └── import-data.sh     # Script de validación e importación
└── docker-compose.yml     # Orquestador de contenedores

```

## Instalación y Despliegue

### 1. Clonar el repositorio:
```bash
git clone https://github.com/ndend-dev/SaasMultiTenant.git
cd SaasMultiTenant

```

### 2. Levantar el entorno:
```bash
docker-compose down -v && docker-compose up --build

```



El contenedor de la base de datos verificará el estado del motor internamente. Una vez que SQL Server esté listo para recibir conexiones, ejecutará de forma automática el archivo `script.sql`.

---

## Proceso Importante de Arranque Inicial

Al ejecutar el entorno por primera vez, se inicia un proceso secuencial automatizado en la base de datos. Para poder interactuar con la aplicación y probar las funcionalidades, es obligatorio esperar a que concluya la inserción de datos:

1. Arranque del motor: El contenedor de SQL Server levanta sus servicios internos (tarda entre 10 y 15 segundos).

2. Validación e Inyección: El script `import-data.sh` detecta que el motor está listo y ejecuta el archivo `script.sql`.

Confirmación: Sabrás que el proceso ha terminado cuando veas el siguiente mensaje en los logs de la terminal:
`Database creation completed successfully!`

**Nota:** Si intentas ingresar a la aplicación web o realizar peticiones a la API antes de ver este mensaje, obtendrás errores de conexión o fallos de autenticación (500 Internal Server Error) debido a que las tablas y el usuario de prueba aún no existen.

---

## Puertos de Acceso

| Componente | Dirección Local | Puerto Externo (PC) | Puerto Interno (Contenedor) |
| --- | --- | --- | --- |
| Frontend (Angular) | http://localhost:4200 | 4200 | 80 |
| Backend API (.NET 9) | http://localhost:5000 | 5000 | 8080 |
| Base de Datos (MSSQL) | localhost,1433 | 1433 | 1433 |

---

## Credenciales de Prueba

Para interactuar con el sistema y realizar pruebas una vez desplegado el entorno, use las siguientes credenciales de acceso:

- Correo electrónico: yeissonr@prueba.com

- Contraseña: password123%

---

## Conexión desde SSMS (Herramienta Externa)

Para administrar la base de datos externamente, usa los siguientes datos:

1. Server type: Database Engine
2. Server name: `localhost,1433` (Usa coma para separar el puerto)
3. Authentication: SQL Server Authentication
4. Login: `sa`
5. Password: `SaasMultiTenant123%`

Nota para SSMS v19+: En la ventana de conexión, ve a `Options >>` -> `Connection Properties` y activa la casilla **Trust server certificate**.

---

## Configuración del AppDbContext

El archivo `AppDbContext.cs` está configurado para permitir flexibilidad entre el desarrollo local y el contenedor:

```csharp
// Configuration strategy in SaasMultiTenant.DAL.Context.AppDbContext
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        // Default fallback connection string for standalone execution inside Docker
        optionsBuilder.UseSqlServer("Server=sqlserver;Database=SaasMultiTenant;User Id=sa;Password=SaasMultiTenant123%;Encrypt=False;TrustServerCertificate=True;");
    }
}

```

---

## Resolución de Problemas

### Los cambios en el código no se aplican

Si modificas configuraciones o código de C# y Docker no los toma debido a la caché, fuerza la reconstrucción total:

```bash
docker-compose down --rmi local -v
docker-compose up --build --force-recreate

```

### Error de formato en script Linux (\r: command not found)

Si editas `import-data.sh` en Windows, el archivo puede guardarse con caracteres invisibles CRLF. El Dockerfile ejecuta la herramienta `dos2unix` internamente durante el build para limpiar el archivo a formato LF antes de correrlo.

---

## Licencia

Este proyecto está bajo la Licencia MIT.

```text
MIT License

Copyright (c) 2026

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

```

```

```