# 🚀 Calisthenics Skill Tracker

This app helps you track your workouts, discover new skills and
exercises, and progress in calisthenics through structured training and
data tracking.

------------------------------------------------------------------------

## 📋 Table of Contents

-   About the Project\
-   Technologies Used\
-   Prerequisites\
-   Getting Started\
-   Project Structure\
-   Features\
-   Usage\
-   Database Setup\
-   Data Seeding\
-   Database Diagram\
-   Configuration\
-   License\
-   Contact

------------------------------------------------------------------------

## 📖 About the Project

Calisthenics Skill Tracker is a web application built with ASP.NET Core
MVC that allows users to track workouts, manage exercises and skills,
and monitor their progression over time.

------------------------------------------------------------------------

## 🛠️ Technologies Used

  Technology              Version   Purpose
  ----------------------- --------- ----------------
  ASP.NET Core MVC        6.0       Web framework
  Entity Framework Core   6.0       ORM / Database
  SQL Server              \-        Database
  ASP.NET Identity        \-        Authentication
  Bootstrap               5.x       Styling
  jQuery & AJAX           \-        Dynamic UI
  Razor Views             \-        Rendering

------------------------------------------------------------------------

## ✅ Prerequisites

-   .NET 6 SDK\
-   Visual Studio / VS Code\
-   SQL Server\
-   Git

------------------------------------------------------------------------

## 🚀 Getting Started

``` bash
git clone https://github.com/cvetkovalexander/CalisthenicsSkillTracker.git
cd CalisthenicsSkillTracker
dotnet restore
dotnet ef database update
dotnet run
```

------------------------------------------------------------------------

## 📁 Project Structure

    CalisthenicsSkillTracker/
    ├── CalisthenicsSkillTracker/
    ├── CalisthenicsSkillTracker.Data/
    ├── CalisthenicsSkillTracker.Data.Models/
    ├── CalisthenicsSkillTracker.GCommon/
    ├── CalisthenicsSkillTracker.Infrastructure/
    ├── CalisthenicsSkillTracker.Services.Common/
    ├── CalisthenicsSkillTracker.Services.Core/
    ├── CalisthenicsSkillTracker.Services.Tests/
    ├── CalisthenicsSkillTracker.Web.ViewModels/
    └── CalisthenicsSkillTracker.sln

------------------------------------------------------------------------

## ✨ Features

-   Authentication & roles\
-   CRUD for exercises & skills\
-   Favorites system\
-   Search, sorting, filtering\
-   Keyset pagination\
-   Workout logging\
-   Dynamic reps/duration input\
-   Skill progress tracking

------------------------------------------------------------------------

## 💻 Usage

-   Register & login\
-   Manage exercises & skills\
-   Create workouts\
-   Log sets\
-   Track progress

------------------------------------------------------------------------

## 🗄️ Database Setup

``` json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CalisthenicsSkillTrackerDb;Trusted_Connection=True;"
}
```

Run:

``` bash
dotnet ef database update
```

------------------------------------------------------------------------

## 🌱 Data Seeding

-   11 Skills\
-   11 Exercises\
-   Linked relationships

------------------------------------------------------------------------

## 🧱 Database Diagram

![Database
Diagram](https://raw.githubusercontent.com/cvetkovalexander/CalisthenicsSkillTracker/main/docs/diagram.png)

------------------------------------------------------------------------

## ⚙️ Configuration

``` json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(YourServer);Database=CalisthenicsSkillTracker;Trusted_Connection=True;Encrypt=False;"
  }
}
```

------------------------------------------------------------------------

## 📄 License

Apache-2.0

------------------------------------------------------------------------

## 📬 Contact

Alexander Cvetkov\
https://github.com/cvetkovalexander

Project:\
https://github.com/cvetkovalexander/CalisthenicsSkillTracker

Built as part of the SoftUni ASP.NET course
