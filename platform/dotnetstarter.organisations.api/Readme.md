## dotnetstarter.organisations.api

This API handles all logic and persistance that should be common to all tenants,

### Local Development Setup

1. Add the Google application credentials .json file to the dotnetstarter.gateway.api directory
2. Edit the launch settings (right click project, Properties, Debug) and add the name of the .json file from 1 to the `GOOGLE_APPLICATION_CREDENTIALS` section
3. Add the required credentials to [appsettings.Development.json](./appsettings.Development.json) for the sections `GCSettings`, `SendGrid` and `Firebase`

### Steps to configure a new tenant build configuration

1. Add a new cloudbuild.yaml file eg. tenantname.env_stage.cloudbuild.yaml
2. Copy the contents of YOURTENANTNAME.staging.cloudbuild.yaml to the new file
3. Copy the GCP application credentials .json file to the project
4. Change the `docker build` command to the following: 
    `args: [ 'build', '-t', 'gcr.io/$PROJECT_ID/dotnetstarter.organisations.api', '.', '--build-arg', 'TENANTNAME=YOURTENANTNAME', '--build-arg', 'ASPNETCORE_ENV=Staging', '--build-arg', 'GCP_APP_CREDENTIALS=mytenant_gcp_project.json']`
5. Setup a new trigger in cloudbuild pointing to the cloudbuild.yaml file created.
6. Ensure you have a complete subsection in appsettings.Development and appsettings.\otherstages\ for the Tenant identified by the TENANTNAME build argument in step 3
