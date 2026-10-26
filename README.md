# CineAPI

CineAPI es una aplicación web MVC en C# y ASP.NET Core que permite buscar películas y consultar sus detalles usando la API de [TMDB](https://www.themoviedb.org/).

## Objetivo

Practicar el patrón MVC, el consumo seguro de una API externa y la creación de una interfaz Razor responsive. La aplicación no expone una API REST propia: presenta los resultados de TMDB mediante páginas web.

## Tecnologías

- .NET 10 y ASP.NET Core MVC
- C# y Razor
- Bootstrap, HTML y CSS
- TMDB API v3
- `HttpClient` y User Secrets

## Requisitos

- .NET SDK 10
- Un Read Access Token de TMDB
- Conexión a Internet

## Instalación y configuración

Desde la carpeta del proyecto:

```bash
dotnet restore
dotnet user-secrets set "Tmdb:ReadAccessToken" "TU_TOKEN_REAL"
dotnet build
dotnet run --launch-profile http
```

El token se almacena fuera del repositorio con User Secrets. Nunca debe escribirse en `appsettings.json`, en una vista, en el código fuente ni en un commit.

Abre `http://localhost:5072`.

## Funcionalidades

- Página de inicio con buscador.
- Búsqueda de títulos en TMDB.
- Tarjetas con póster, fecha, puntuación y sinopsis.
- Página de detalles con géneros, duración y fondo cuando TMDB los proporciona.
- Mensajes para búsquedas vacías, resultados inexistentes, token inválido, fallos de conexión y datos incompletos.
- Diseño adaptable a computadora y pantallas pequeñas.

## Estructura

`Controllers` contiene las acciones MVC; `Models` contiene modelos de vista y DTOs de TMDB; `Services` encapsula la integración HTTP; `Views` contiene las páginas Razor; `wwwroot` contiene estilos y recursos estáticos.

## Pruebas realizadas

La validación local realizada en este entorno incluye `dotnet build`, comprobación HTTP de la página inicial, estado vacío de `/Movies`, respuesta 404 para un identificador de detalles inválido y mensaje controlado cuando falta el token. Las búsquedas con resultados reales, búsquedas sin resultados y la página de detalles deben ejecutarse después de configurar un token válido localmente.

## Declaración de uso de IA

Se utilizó IA como apoyo para comprender ASP.NET Core MVC, diseñar la estructura de archivos, proponer código inicial, analizar errores, redactar documentación y mejorar los mensajes de la interfaz. Los cambios fueron ejecutados, revisados y probados por la persona responsable del proyecto.

## Licencia y atribución

Los datos e imágenes de películas son proporcionados por TMDB. Este proyecto no está respaldado ni certificado por TMDB.
