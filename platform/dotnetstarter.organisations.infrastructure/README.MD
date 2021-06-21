

# EF CORE MIGRATION
1. Open Nuget Package Manager Console
2. cd into dotnetstarter.organisations.api
3. set dotnetstarter.organisations.domain as active project in dropdown
4. NB NB NB!!! 
   Set TENANT env variable in your shell `$env:TENANT="DEFAULT"`. Otherwise tenant-specific permissions will not be added to the migration!
5. dotnet ef --project ..\dotnetstarter.organisations.infrastructure --startup-project . migrations add InitialDBCommit --context dotnetstarter.organisations.infrastructure.OrganisationsContext
