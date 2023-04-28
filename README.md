# Rewriting service

Rewriting service is a platform that connects clients who need content to be rewritten with freelance writers who can provide high-quality rewritten content.

## Setup

You will need _Docker_ or _Docker Desktop_ installed on your computer.  
System is configured to work on both _Linux_ and _Windows_ platforms.

1. Pull this repository to your local storage.
2. Ensure that docker deamon is running. Open folder in terminal and type `docker-compose build` (with `sudo` on Linux).
3. After build finishing, type `docker-compose up` (with `sudo` on Linux)

Starting up may take a time.  
When the system is running, you can access some services with browser.

- The main application at `http://localhost:10010`
- Open API documentation at `http://localhost:10001/swagger`
- RabbitMQ management panel at `http://localhost:15672`
- Identity Server discovery endpoint at `http://localhost:10001/.well-known/openid-configuration`

On the first startup an admin user will be added to database. You can login as an admin using this credentials:
> Login: admin@mail.ru  
> Password: Password123$
