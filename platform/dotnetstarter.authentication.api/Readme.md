## dotnetstarter.authentication.api

This API issues authentication tokens for internal inter-service communications

### Local Development Setup

1. Add the correct issuer and audience domains in [appsettings.Development.json](./appsettings.Development.json) under section `Authentication`

### Steps to configure a new tenant build configuration

1. Add a new cloudbuild.yaml file eg. tenantname.env_stage.cloudbuild.yaml
2. Copy the contents of dotnetstarter.staging.cloudbuild.yaml to the new file
3. Copy the GCP application credentials .json file to the project
4. Change the `docker build` command to the following: 
    `args: [ 'build', '-t', 'gcr.io/$PROJECT_ID/dotnetstarter.authentication.api', '.', '--build-arg', 'TENANTNAME=YOURTENANTNAME', '--build-arg', 'ASPNETCORE_ENV=Staging', '--build-arg', 'GCP_APP_CREDENTIALS=mytenant_gcp_project.json']`
5. Setup a new trigger in cloudbuild pointing to the cloudbuild.yaml file created.
6. Ensure you have a complete subsection in appsettings.Development and appsettings.\otherstages\ for the Tenant identified by the TENANTNAME build argument in step 3
