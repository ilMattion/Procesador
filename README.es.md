# Procesador

Este proyecto *beta* proporciona una biblioteca para administrar el procesamiento de documentos permitiendo al desarrollador definir su lógica de negocio sin preocuparse por:

* Almacenamiento de blobs
* Gestión de estados

# Desarrollo
## Actualización de la base de datos

	dotnet ef migrations add Init --startup-project ..\Procesador.WebApi\
	dotnet ef database update --startup-project ..\Procesador.WebApi\