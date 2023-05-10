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
- Open API documentation at `http://localhost:10000/swagger`
- RabbitMQ management panel at `http://localhost:15672`
- Identity Server discovery endpoint at `http://localhost:10001/.well-known/openid-configuration`

On the first startup an admin user will be added to database. You can login as an admin using this credentials:
> Login: admin@mail.ru  
> Password: Password123$

---

## How to use it?

- Anonimous user can look through available orders.
- User can register by clicking `Register` button in upper right corner and filling out the registration form, or login with registered credentials.
- Registered user can add own orders and add offers to existing ones, available for offers.
- Offers will be displayed on order deteils page. Order client can accept one of the offer by clicking on `Accept` button near the offer.
- When offer is accepted, contractor will see the contract in `My contracts` page. Contractor can add results to the contract.
- Client can accept or decline _all_ these results using corresponding button.
- If results are declined, client can decline contractor. In this case contract will be removed and client will able to accept an other offer.
- When results are accepted, the order is marked as done.
- Registered users can look through their own contracts, orders and offers on corresponding page
