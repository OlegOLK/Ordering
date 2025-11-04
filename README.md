# Ordering Microservice

This repository contains a sample ordering microservice built with .NET 8. It demonstrates a robust, scalable, and resilient backend architecture using several modern design patterns, including Clean Architecture, CQRS, the Transactional Outbox pattern, and the Saga pattern for asynchronous processing.

The system is designed to handle order creation and processing in a decoupled, message-driven manner, ensuring reliability and data consistency between services.

## Design trade-offs

* Validation is done on the controller side to match with the concept fail-fast and let end-user know that something is not correct with his request
* Event publishing in TransactionBehaviour might affect overall user experience when submitting and order. This one can be processed separatelly in dedicated worker.
* Due to luck of time processing is simplified
* To speed up development process I use a lot of 3rd party libraries. In some cases this is not a good idea for production-grade projects due to often license changes in open source libraries. Ideally I should use bare minimum of packages.
* One of such packages in MassTransit and Mediatr (even though I replace original Mediatr library with some alternative)
* Luck of resiliense for critical integration points. Ideally application should have common strategies like: retry, circuit-braker, hadgin etc.
* Processing and API are running as a single application, but entire solution was designed with idea to cut them into independent pieces without any problems.
* Migration for DB is not ideal
* Unfortunatelly no tests: Unit\Itegration\E2E due to luck of time
* Outbox event cleanup is not designed.
* Dead-lettering is not addressed.

## Core Architectural Concepts

This project is a practical implementation of several key architectural patterns:

*   **Clean Architecture**: The solution is structured into distinct layers (`Domain`, `Application`, `Infrastructure`) to enforce separation of concerns, making the system more maintainable, testable, and independent of external frameworks.
*   **CQRS & Mediator Pattern**: Command Query Responsibility Segregation (CQRS) is used to separate read and write operations. The Mediator pattern (using `Cortex.Mediator`) decouples command senders from their handlers, simplifying the application logic.
*   **Transactional Outbox Pattern**: To ensure reliable message delivery in a distributed system, this pattern is used to atomically save business state (the `Order`) and the event to be published (`OutboxEvent`) in the same database transaction. A separate process then relays these events to the message broker, guaranteeing that a message is sent if and only if the core business transaction was successful.
*   **Asynchronous Message-Driven Processing**: Order creation via the API is fast, as it only persists the initial order and queues it for processing. The heavy lifting, like checking stock availability, is handled by a separate background service (`Ordering.Processing`) that consumes messages from RabbitMQ.
*   **Saga Pattern for Order Processing**: The `Ordering.Processing` service implements a simple Saga using a pipeline of processors (`IProcessor`). Each step in the order processing (e.g., `ItemsAvailabilityCheckProcessor`) is a distinct unit that can be executed sequentially. This pattern allows for complex, multi-step business transactions with compensation logic for rollbacks.
*   **Resilience & Retries**: A background worker (`RetryFailedOrdersWorker`) periodically scans for failed events and retries them, adding resilience to the system and handling transient failures gracefully.

## Technology Stack

*   **Backend**: .NET 8, ASP.NET Core
*   **Database**: PostgreSQL with Entity Framework Core
*   **Messaging**: RabbitMQ with MassTransit
*   **Containerization**: Docker & Docker Compose
*   **Architecture**: Clean Architecture, CQRS, Transactional Outbox, Saga
*   **Observability**: OpenTelemetry (Prometheus), ASP.NET Core Health Checks
*   **API Documentation**: Swashbuckle (Swagger)

## Getting Started

The entire application stack is containerized for easy setup.

### Prerequisites

*   Docker Desktop installed and running.

### Installation & Running

1.  Clone the repository:
    ```sh
    git clone https://github.com/olegolk/ordering.git
    cd ordering
    ```

2.  Run the application using Docker Compose:
    ```sh
    docker-compose up --build -d
    ```

This command will build the images for the API and processing services and start containers for the API, RabbitMQ, and PostgreSQL. The database schema will be created and migrations will be applied automatically on startup.

## Usage

Once the services are running, you can interact with the system through the following endpoints:

*   **API Base URL**: `http://localhost:8080`
*   **Swagger UI**: `http://localhost:8080/swagger`
*   **RabbitMQ Management UI**: `http://localhost:15672` (Username: `guest`, Password: `guest`)

### Create an Order

You can create a new order by sending a `POST` request to the `/api/orders` endpoint.

**Example using `curl`:**

```bash
curl -X 'POST' \
  'http://localhost:8080/api/Orders' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "customerId": "customer-123",
  "items": [
    {
      "productId": 1,
      "name": "Sample Product",
      "price": 9.99,
      "quantity": 2
    },
    {
      "productId": 2,
      "name": "Another Product",
      "price": 25.50,
      "quantity": 1
    }
  ]
}'
```

A successful request will return a `200 OK` with the created `orderId`.

## Observability

The service is configured with endpoints for monitoring its health and performance.

*   **Health (Liveness)**: `http://localhost:8080/healthz/alive`
*   **Health (Readiness)**: `http://localhost:8080/healthz/ready` (includes checks for database and RabbitMQ)
*   **Prometheus Metrics**: `http://localhost:8080/metrics` (Exposes counters for processed and failed orders)

## Project Structure

The solution is divided into several projects, following the principles of Clean Architecture.

| Project                         | Description                                                                                                    |
| ------------------------------- | -------------------------------------------------------------------------------------------------------------- |
| `Ordering.API`                  | The main entry point. Exposes the REST API, handles requests, and initiates commands.                          |
| `Ordering.Application`          | Contains the core business logic, command/query handlers, services, and application-level exceptions.          |
| `Ordering.Domain`               | The heart of the application. Contains domain entities (`Order`, `OrderItem`), enums, and domain exceptions.     |
| `Ordering.Persistance`          | Defines the data access interfaces (`IRepository`, `IUnitOfWork`).                                               |
| `Ordering.Persistance.Postgres` | Implements the persistence layer using Entity Framework Core with a PostgreSQL provider.                         |
| `Ordering.Messaging.RabbitMq`   | Implements the messaging infrastructure using MassTransit and RabbitMQ.                                        |
| `Ordering.Processing`           | A background service responsible for consuming order messages and executing the order processing logic (Saga).   |