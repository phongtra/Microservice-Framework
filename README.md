# IgniteSparkDownloader

**Must have**
`Docker Desktop` installed on the machine.

Link: https://hub.docker.com/?overlay=onboarding
`Consul CLI` https://www.consul.io/downloads.html

**To run Graylog**
In your command line, run `docker-compose up` and navigate to `127.0.0.1:9000` on your browser
and sign in as `admin/admin`

**To run Consul**
In your command line, run `consul agent -dev` and navigate to `127.0.0.1:8500`

**For the database**
DatabaseAPI uses Postgres, make sure that in your appsettings.json, you set your own connection string to your database, delete the Migration folder, and apply the Migration again.

**To start up the project**

1. `cd ReactAndIdentityServer/ClientApp`
2. `yarn install`
3. `yarn start`
4. If needed, go to each project and check if the reference path to Consul Extension and GenericWebInitializer is correct.
5. Check the Start up option in `Project Properties`, go to debug and make sure DatabaseAPI is on 5000, ReactAndIdentityServer is on 2100, and GatewayAPI is on 2000.
6. Check the running option, make sure that it is not IIS.
7. Run DatabaseAPI, GatewayAPI, and ReactAndIdentityServer.
8. Navigate to `http://localhost:2000`

**Weird Behavior**

There can be some weird behavior when trying to start up the server. You may have to reload the page twice in order to get it working because the Consul server cannot quite pick up the service. You may also have to rerun the project to get it work.

**RabbitMQ**

To start RabbitMQ server, run the command `docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management` and navigate to localhost:5672
