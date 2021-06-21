

# EF CORE MIGRATION
Open Nuget Package Manager Console
cd into dotnetstarter.gateway.api
set dotnetstarter.gateway.domain as active project in dropdown
dotnet ef --project ..\dotnetstarter.gateway.infrastructure --startup-project . migrations add InitialDBCommit --context dotnetstarter.gateway.infrastructure.PortalContext
