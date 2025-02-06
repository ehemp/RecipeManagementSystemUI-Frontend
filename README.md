# RecipeManagementSystemUI

A Blazor WebAssembly application to create, compare, remove, update, delete semiconductor recipes (or any recipes) with authentication and authorization features. This project implements JWT-based authentication using .NET 8, Entity Framework Core, and Blazor components to manage user state and UI updates dynamically.

## Description

🚀 Features

User Authentication: JWT-based login/logout

Role-Based Authorization: Access control for different user roles

State Management: Uses AuthenticationStateProvider to update UI dynamically

Blazor Components: Modular design with AuthGuard for route protection

LocalStorage Integration: Stores authentication tokens securely with expiration

📦 Tech Stack

Frontend: Blazor WebAssembly, C#, Razor Components

Backend: .NET Core 8+, ASP.NET Web API, Entity Framework Core

Database: SQL Server / PostgreSQL

Authentication: JWT (JSON Web Token), BCrypt for password hashing

State Management: Blazored.LocalStorage, AuthenticationStateProvider

Compare: Choose 2 recipes to compare and UI will display differences

Export: Export any recipe or the list of recipes to csv

## UI Images
![image1](https://github.com/user-attachments/assets/c4da44c6-bd51-4879-a970-34838314927e)
![image2](https://github.com/user-attachments/assets/8b23ae2b-774c-41c6-bc91-af4c05c65f11)
![image3](https://github.com/user-attachments/assets/c5c3d7d4-c775-43a6-93c9-c3fe9db28167)
![image4](https://github.com/user-attachments/assets/df5807ec-16df-4833-abd2-09617a485d58)
![image5](https://github.com/user-attachments/assets/534eee10-b8f6-4e6d-9f2f-9f2adb6383c5)
![image6](https://github.com/user-attachments/assets/f250e8fb-73bc-4de7-a3b7-4c3645bdc0de)
![image7](https://github.com/user-attachments/assets/eced7b63-1bf1-4336-a24a-692b3599ef24)
![image8](https://github.com/user-attachments/assets/3c4c1a46-218d-49b3-af53-38044aca4cf2)
![image9](https://github.com/user-attachments/assets/4e13994a-2bfa-46e0-9986-0af647e98409)



## Landmarks and Roadblocks
I wanted to challenge myself and create a full-stack .Net web application. I plan on updating features and fixing bugs to further increase my knowledge and skills. I ran into some roadblocks here and there. For example, having the NavBar render or not depending on authentication state has me going in circles until I stepped away for a bit and realized I just needed to update a state according to the AuthService class and have it render dependent on that bool value. Getting the authorization to work correclty was somewhat challenging, but overall I am happy about the solution.
More updates to come...

## Version History

* 0.2
    * Various bug fixes and optimizations
    * See [commit change]() or See [release history]()
* 0.1
    * Initial Release
