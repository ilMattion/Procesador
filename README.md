# Procesador

This *beta* project provide an library for manage document processing permitting the developer to define its business logic without worrying about:

* Storage of blobs
* Management of states

# Development
## Update database 

	dotnet ef migrations add Init --startup-project ..\Procesador.WebApi\
	dotnet ef database update --startup-project ..\Procesador.WebApi\