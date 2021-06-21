FROM microsoft/dotnet:2.2-sdk
ADD ./ /app
ARG TENANTNAME
ARG ASPNETCORE_ENV
ARG GCP_APP_CREDENTIALS
# This is important, the application must listen on the $PORT env variable specified by the container in order to not be terminated
ENV ASPNETCORE_URLS=http://*:${PORT}
ENV ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENV}
ENV GOOGLE_APPLICATION_CREDENTIALS=${GCP_APP_CREDENTIALS}
ENV TENANT=${TENANTNAME}
WORKDIR /app
ENTRYPOINT [ "dotnet", "dotnetstarter.authentication.api.dll" ]
