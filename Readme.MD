# Content Platform

Welcome to the Content Platform project, a system designed to facilitate communication between microservices through a loosely coupled microservice communication messaging system. This project leverages RabbitMQ and MassTransit for messaging and employs a vertical slice architecture built on ASP.NET Core Web API.

## System Overview

The Content Platform system comprises two main services:

1. **Newsletter.Api**
   - **Create Article**: Allows you to create a new article.
   - **Get Article**: Fetch the details of an existing article using its ID.

2. **Newsletter.Reporting.Api**
   - **Aggregating Data**: Gathers data about newsletters and events related to specific articles.

A contract project is utilized to represent the messages being transmitted over RabbitMQ.

## Getting Started

To get started with using the Content Platform, follow the steps below:

### Prerequisites

Ensure you have the following installed:
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)

### Setup

Clone the repository to your local machine and navigate to the project directory:

```sh
git clone https://github.com/EziamakaNV/ContentPlatform.git
cd ContentPlatform
```

### Running the Services with Docker Compose

To facilitate the orchestration of the Docker containers during development, Docker Compose is used. The necessary files are included in the project repository.

In the project root directory, execute the following command to build and run the services:

```sh
docker-compose up --build
```

## Usage

### Newsletter.Api Service

Access the service's Swagger documentation at [Newsletter.Api Swagger](https://localhost:5001/swagger/index.html) to test the endpoints.

#### Create an Article

Make a POST request to `api/articles` to create a new article. Upon successful creation, the article ID will be returned.

#### Get Article Details

Use the GET endpoint `api/articles/{Id}` with the returned article ID to fetch the details of the article.

### Newsletter.Reporting.Api Service

Access the service's Swagger documentation at [Newsletter.Reporting.Api](https://localhost:5001/swagger/index.html) Swagger to test the endpoints.

#### Get Article Events Details

Using the article ID obtained from the Newsletter.Api service, you can fetch detailed information about the events related to that article.

## Testing

To test the functionalities of the services, use the Swagger UI to perform the following operations:

1. **Newsletter.Api**
   - Create an article and note down the returned ID.
   - Use the ID to get the article details.

2. **Newsletter.Reporting.Api**
   - Use the ID obtained from the Newsletter.Api service to get details about the events concerning that article.

