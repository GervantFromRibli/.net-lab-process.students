# Welcome 

Hello! Welcome to repository of Shelyakhin Mihail. It contains project of ticket management application that was created for Epam .NET training course.

# SolutionTemplate

1. Clone solution from repository;
2. Use scripts in "docs" folder to create main and test databases;
3. Find connection strings for both main and test databases;
4. Change connection strings in repository classes and in tests to new connections(test database connection string - in tests, main database - in repository classes);
5. If solution or some project can not find NuGet packages, it is nessesary to rebuild solution.
6. Run tests from test explorer, from tests or some other tool. In integration tests methods must be runned in this order: AddEvent -> UpdateEvent -> DeleteEvent.
