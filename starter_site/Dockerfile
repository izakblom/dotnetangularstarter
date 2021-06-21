FROM gcr.io/cloud-builders/npm as npm
ARG TENANT_STAGE
# This is important for other commands which use the relative path
WORKDIR /usr/src/app
# copy both, the package.json and the package.lock.json into the image
COPY package*.json ./
# run npm install to install all dependencies of the project
RUN npm install
# copy the rest of the project into the image
COPY . .
# Run the build script, tenant stage should be eg. build__staging
RUN npm run ${TENANT_STAGE}

FROM nginx:alpine as nginx

COPY --from=npm /usr/src/app/dist/startersitesite /usr/share/nginx/html
COPY ./nginx.conf /etc/nginx/conf.d/default.conf
# Exposing ports.
EXPOSE 80
EXPOSE 8080

# Starting server.
CMD ["nginx", "-g", "daemon off;"]
