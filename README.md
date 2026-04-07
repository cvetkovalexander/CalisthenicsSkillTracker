# 🚀 Calisthenics Skill Tracker

This app helps you track your workouts, discover new skills and exercises, and progress in calisthenics through structured training and data tracking.

---

## 📋 Table of Contents

* About the Project
* Technologies Used
* Prerequisites
* Getting Started
* Project Structure
* Features
* Usage
* Database Setup
* Data Seeding
* Database Diagram
* Configuration
* License
* Contact

---

## 📖 About the Project

Calisthenics Skill Tracker is a web application built with ASP.NET Core MVC that allows users to track workouts, manage exercises and skills, and monitor their progression over time.

The application focuses on providing a structured and efficient way to improve in calisthenics by combining workout logging, skill tracking, and curated exercise discovery in one place.

---

## 🛠️ Technologies Used

| Technology            | Version | Purpose                 |
| --------------------- | ------- | ----------------------- |
| ASP.NET Core MVC      | 6.0     | Web framework           |
| Entity Framework Core | 6.0     | ORM / Database access   |
| SQL Server            | -       | Database                |
| ASP.NET Identity      | -       | Authentication & roles  |
| Bootstrap             | 5.x     | Frontend styling        |
| jQuery & AJAX         | -       | Dynamic UI interactions |
| Razor Views           | -       | Server-side rendering   |

---

## ✅ Prerequisites

* .NET SDK 6.0+
* Visual Studio 2022 or VS Code
* SQL Server
* Git

---

## 🚀 Getting Started

1. Clone the repository
   git clone https://github.com/your-username/calisthenics-skill-tracker.git
   cd calisthenics-skill-tracker

2. Restore dependencies
   dotnet restore

3. Apply database migrations
   dotnet ef database update

4. Run the application
   dotnet run

App available at:
https://localhost:5001
http://localhost:5000

---

## 📁 Project Structure

```id="vmd3y4"
CalisthenicsSkillTracker/
├── CalisthenicsSkillTracker/                 # Main ASP.NET Core web project
├── CalisthenicsSkillTracker.Data/            # DbContext, repositories, configurations, seeding
├── CalisthenicsSkillTracker.Data.Models/     # Entity models
├── CalisthenicsSkillTracker.GCommon/         # Constants, messages, exceptions, validation attributes
├── CalisthenicsSkillTracker.Infrastructure/  # Middleware, extensions, utilities
├── CalisthenicsSkillTracker.Services.Common/ # Shared service-layer components
├── CalisthenicsSkillTracker.Services.Core/   # Business logic and service implementations
├── CalisthenicsSkillTracker.Services.Tests/  # Unit tests
├── CalisthenicsSkillTracker.Web.ViewModels/  # Input and output view models
└── CalisthenicsSkillTracker.sln              # Solution file
```

```


---

## ✨ Features

* User registration and login (ASP.NET Identity)
* Role-based access (Admin / Moderator / User)
* CRUD operations for Exercises and Skills
* Favorite exercises and skills (AJAX-based)
* Search functionality
* Sorting (A–Z / Z–A)
* Filtering by difficulty
* Keyset pagination for efficient navigation
* Workout management
* Log workout sets (reps or duration dynamically)
* Track skill progress over time
* Responsive UI with Bootstrap and custom CSS

---

## 💻 Usage

* Register and log into your account
* Browse, create, edit, and delete exercises and skills
* Mark favorites for quick access
* Create workouts and log sets
* Track your skill progress over time

---

## 🗄️ Database Setup

Connection string in appsettings.json:

"ConnectionStrings": {
"DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=CalisthenicsSkillTrackerDb;Trusted_Connection=True;"
}

Apply migrations:
dotnet ef database update

---

## 🌱 Data Seeding

The database is automatically seeded with:

* 11 Skills
* 11 Exercises
* Predefined relationships between them

---

## 🧱 Database Diagram

![Database Diagram](docs/diagram.png)
---

## ⚙️ Configuration

"ConnectionStrings": {
"DefaultConnection": "Server=(YourServerLocalServerName);Database=CalisthenicsSkillTracker2026;Trusted_Connection=True;Encrypt=False;"
}

---

## 📄 License

Apache-2.0 license.

---

## 📬 Contact

Alexander Cvetkov
https://github.com/cvetkovalexander

Project Link:
https://github.com/cvetkovalexander/CalisthenicsSkillTracker

Built as part of the SoftUni ASP.NET course
