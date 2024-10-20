# BizCardManagementSystem

## Table of Contents
- [Project Overview](#project-overview)
- [Project Structure](#project-structure)
- [Setup Instructions](#setup-instructions)
- [Dependencies](#dependencies)
- [Usage Guidelines](#usage-guidelines)
- [Running Tests](#running-tests)

## Project Overview
**BizCardManagementSystem** is a .NET-based solution designed to manage business card information efficiently. The system allows users to store, retrieve, update, and organize business card details.

## Project Structure

- **BizCardManagementSystem/src**: Contains the source code for the project.
  - **BizCardManagementSystem/src/Api**: Handles all API operations and business logic for managing business cards.
  - **BizCardManagementSystem/src/Application**: Contains the use cases (application logic).
  - **BizCardManagementSystem/src/Domain**: Defines the entities (business models) and core business rules.
  - **BizCardManagementSystem/src/Infrastructure**: Implements the interfaces defined in the application layer and manages database interactions, including context and migrations with Entity Framework Core.
- **BizCardManagementSystem/test**: Contains the unit tests for the project.
- **BizCardManagementSystem/src/Api/ClientApp**: Front-end project built with Angular.

## Setup Instructions

1. **Clone the Repository**:
   ```sh
   git clone https://github.com/anasmajdoub/BusinessCardSystem.git
   cd BusinessCardSystem
   ```

2. **Install Dependencies**:
    - Ensure you have [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed.
    - Install the required NuGet packages by restoring the project dependencies:
    ```sh
    dotnet restore
    ```

3. **Set Up the Database**:
    - Configure the connection string in `appsettings.json` for deployment by adding your server name and database name.
    - Configure the connection string in `appsettings.Development.json` for development by adding your server name and database name.
    - Run the migration to set up the database:
      - In Visual Studio, use the Package Manager Console.
      - Set the Default Project to `BizCardManagementSystem/src/Infrastructure`.
      - Run the following command:
      ```sh
      Update-Database
      ```

4. **Run the Application**:
    ```sh
    dotnet run
    ```

5. **Access the API**:
    - Once the application is running, you can access the API at `https://localhost:7019/swagger/index.html`.

## Dependencies

- **.NET 8 SDK**: Required to build and run the application.
- **Entity Framework Core**: Used for data access.
- **FluentValidation**: For validating business models.
- **Dapper**: Used for data access.
- **FluentValidation.DependencyInjectionExtensions**: For dependency injection of validation services.
- **AutoMapper**: To map between DTOs and domain models.
- **NSubstitute**: Used for mocking in unit tests.
- **xUnit**: The testing framework used in this project.
- **CsvHelper**: For reading and writing CSV files.
- **Newtonsoft.Json**: For handling JSON.
- **System.Drawing.Common**: For image-related tasks.
- **ZXing.Net**: For barcode and QR code scanning functionality.
- **Microsoft.Extensions.Configuration.Abstractions**: For configuration management.

## Usage Guidelines

- **API Endpoints**:
  - `/api/businesscards/getall` - Retrieve all business cards.
  - `/api/businesscards/getbyid/{id}` - Retrieve a specific business card by ID.
  - `/api/businesscards/create` - Create a new business card.
  - `/api/businesscards/createbusinesscardbyfile` - Create a business card by uploading a file.
  - `/api/businesscards/deletebyid/{id}` - Delete a business card by ID.
  - `/api/businesscards/exporttocsv/{id}` - Export a business card to a CSV file.
  - `/api/businesscards/exporttoxml/{id}` - Export a business card to an XML file.

## Running Tests

This project includes unit tests for the controllers and services.

- To run the tests, use the following command:
  ```sh
  dotnet test
  ```

