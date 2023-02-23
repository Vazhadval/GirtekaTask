* App applies migrations automatically

* To run app on docker:

1. make sure docker is running and is switched to linux containers.
2. change connection string from program.cs file
3. in the directory where the Dockerfile is run command : docker build -t web_api .
4. in the directory where the docker-compose.yml file is run command : docker-compose up
