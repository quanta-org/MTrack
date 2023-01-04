# Pull node base image
FROM node:19-alpine

# Install node dependencies
WORKDIR /svelte
COPY ./package.json .
RUN npm install

# Set perms
# RUN mkdir node_modules/.cache && chmod -R 777 node_modules/.cache

# Move to container
COPY . .

# Launch app
CMD ["npm", "run", "dev"]
EXPOSE 5173