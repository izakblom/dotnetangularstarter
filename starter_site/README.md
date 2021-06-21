# DotnetAngular Starter Site
 
## Local development setup

1. Add the correct firebase config in environment.ts and environment.dev.ts

## Tenants specifics
Each tenant should have separate configuration files in /environments for each deployment stage.
For each new configuration collection, the necessary `build` and `serve` configurations should also be added to angular.json
For ease, add shortcut commands for npm in package.json under scripts. eg. `"dev": "ng serve --host testingstartersite..exampledomain.co.za --configuration=_dev "` to launch a  tenant development server
IMPORTANT! Do not simply run `ng serve`. You must add the environment configuration to the `--configuration` argument

## Development server

Run `npm run dev` for a dev server with the  environment configuration.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build --configuration tenantconfig` to build the project. The tenantconfig argument must be specified in angular.json
The build artifacts will be stored in the `dist/` directory. 

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).

## Build container

docker build --build-arg TENANT_STAGE=build__staging -t startersite_site .

## RUN container

docker run -d -p 8080:8080 --name startersitesite startersite_site
