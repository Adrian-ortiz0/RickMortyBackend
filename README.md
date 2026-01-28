# Rick & Morty API Backend

Es para mi prueba técnica

---

##  Arquitectura

Se utilizó la **Clean Architecture**, siguiendo estas capas:

- **Domain**: Contiene las entidades y reglas de negocio.
- **Application**: Contiene los casos de uso y servicios de aplicación.
- **Infrastructure**: Contiene la implementación de servicios externos, repositorios y acceso a datos.
- **API**: Exposición de endpoints mediante controllers en ASP.NET Core Web API.

Esta arquitectura permite que cada capa tenga una responsabilidad clara y facilita el testing y mantenimiento del código.

---

##  Endpoints disponibles

| Método | Ruta | Descripción |
|--------|-----|------------|
| GET    | `/api/character?page={n}` | Listar personajes con paginación y filtros opcionales (`name`, `status`, `species`) |
| GET    | `/api/character/{id}` | Obtener detalle completo de un personaje por ID |
| GET    | `/api/character/episode?url={episodeUrl}` | Obtener información de un episodio a partir de su URL externa |

---


##  Ejecución del proyecto

1. Clonar el repositorio:

```bash
git clone https://github.com/Adrian-ortiz0/RickMortyBackend
