# LMS Arna Task - Mini Assignment & Knowledge Check Module

A full-stack Learning Management System built with .NET 8 and Blazor Server, featuring assignment management and quiz functionality.

## Features

### For Learners

- **View Active Assignments**: Browse available assignments with due dates and materials
- **Complete Assignments**: Access learning materials (PDF, Video, Text) and take quizzes
- **Submit Quizzes**: Answer multiple-choice questions and receive instant scoring
- **Track Progress**: View assignment completion status and scores

### For Managers

- **Progress Reports**: Monitor team member progress and performance
- **Team Overview**: View completion rates and average scores
- **Performance Analytics**: Identify high performers and areas for improvement

## Technology Stack

- **Backend**: .NET 8, ASP.NET Core Web API
- **Frontend**: Blazor Server
- **Database**: Microsoft SQL Server
- **Authentication**: Cookie-based authentication with BCrypt password hashing
- **ORM**: Entity Framework Core 8.0
- **Testing**: xUnit with In-Memory Database
- **Architecture**: Clean Architecture (Repository Pattern)

## Architecture

The application follows **Clean Architecture** principles with a clear separation of concerns:

```
Controllers (API & UI) → Services (Business Logic) → Repositories (Data Access) → Database
```

### Clean Layered Architecture

- **Controller Layer**: Handles HTTP requests, authentication, and API responses
- **Service Layer**: Contains business logic and coordinates between controllers and repositories
- **Repository Layer**: Manages data access and database operations
- **Entity Layer**: Domain models and data transfer objects

## Database Schema

### Core Tables

- **Users**: User accounts with roles (Learner/Manager) and hierarchical relationships
- **Assignments**: Learning assignments with materials, questions, and metadata
- **Questions**: Multiple-choice questions linked to assignments
- **AssignmentProgress**: User progress tracking with scores and completion status
- **UserAnswers**: Individual quiz responses linked to progress records

### DTOs (Data Transfer Objects)

- **ProgressReport**: Simplified data structure for API responses, avoiding circular references and providing clean serialization for progress reports
- **UserAnswerDetails**: Comprehensive answer details with question-by-question breakdown for manager review
- **QuestionDetail**: Individual question response data with correct/selected answer comparison

### Key Relationships

- Users can have managers (hierarchical structure)
- Assignments contain multiple questions
- Users have progress records for each assignment
- Progress records contain user answers

## Setup Instructions

### Prerequisites

- .NET 8 SDK
- Microsoft SQL Server (2019 or later) or SQL Server Express
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd lms-task-arna
   ```

2. **Setup SQL Server Database**

   Make sure you have SQL Server running on `localhost,1433` with the credentials configured in `appsettings.json`:

   - Server: localhost,1433
   - Database: lmsarna (will be created automatically)
   - User: sa
   - Password: admin

   **Docker SQL Server Option:**

   ```bash
   docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=admin" \
   -p 1433:1433 --name sqlserver2022 \
   -d mcr.microsoft.com/mssql/server:2022-latest
   ```

3. **Restore packages**

   ```bash
   dotnet restore
   ```

4. **Update connection string** (if needed)
   Edit `appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost,1433;Database=lmsarna;User ID=sa;Password=admin;Encrypt=True;TrustServerCertificate=True"
     }
   }
   ```

5. **Run database migrations**

   ```bash
   cd lms-arna-task
   dotnet ef database update
   ```

6. **Run the application**

   ```bash
   dotnet run
   ```

7. **Access the application**
   Open your browser and navigate to `http://localhost:5219`

## Default User Accounts

The application comes with pre-seeded user accounts:

### Manager Account

- **Username**: manager
- **Password**: password123
- **Email**: manager@example.com
- **Role**: Manager

### Learner Accounts

Ten learner accounts are pre-seeded (learner1 through learner10):

- **Username**: learner1, learner2, ..., learner10
- **Password**: password123
- **Email**: learner1@example.com, learner2@example.com, ..., learner10@example.com
- **Role**: Learner
- **Manager**: All learners report to the manager account

### Sample Progress Data

The application includes sample progress data:

- **6 completed assignments** with various scores (40-100 points)
- **1 in-progress assignment** (learner8)
- **3 not-started assignments** (learner1, learner7, learner9)

## Business Rules

- **One Submission Rule**: Users can only submit answers once per assignment
- **Scoring System**: 1 correct answer = 20 points (5 questions × 20 points = 100 points total)
- **Manager Hierarchy**: Managers can only view progress of their direct subordinates
- **Assignment Visibility**: Only active assignments within the date range are visible
- **Progress Tracking**: All learners appear in progress reports, including those who haven't started assignments ("Not Started" status)
- **Responsive Design**: Navigation and UI adapt to different screen sizes for optimal user experience
- **Quiz Answer Details**: Managers can view detailed quiz answers and analysis for completed assignments

## New Features Added

### Quiz Answer Details for Managers

- **View Button**: Managers can now click a "View" button in the Actions column of the Progress Report table for completed assignments
- **Answer Details Modal**: Shows comprehensive quiz answer details including:
  - Student information (name, email)
  - Assignment information (title, description)
  - Score breakdown (final score, correct/incorrect answers, total questions)
  - Completion timestamp
  - Detailed answer analysis with accordion-style question breakdown
  - Color-coded options showing selected vs correct answers
  - Per-question results with clear visual indicators

### Enhanced Progress Report

- **Complete Team View**: Shows all 10 learners including those who haven't started assignments
- **Status Badges**: Clear visual indicators for Completed, In Progress, and Not Started
- **Score Visualization**: Color-coded performance indicators with icons
- **Statistics Cards**: Summary metrics including completion rates, average scores, and high performers
- **Responsive Design**: Mobile-friendly layout with proper Bootstrap styling

### API Enhancements

- **New Endpoint**: `/api/quiz/answers/{userId}/{assignmentId}` for retrieving detailed quiz answers
- **ProgressReport**: Structured data transfer object for consistent API responses
- **UserAnswerDetails**: Comprehensive answer details with question-by-question breakdown
- **Manager Authorization**: Secure access to sensitive learner data

### UI/UX Improvements

- **Interactive Modal**: Bootstrap modal with detailed answer visualization
- **Accordion Interface**: Expandable question details for better user experience
- **Visual Feedback**: Icons and color coding for immediate understanding
- **Responsive Navigation**: Fixed mobile menu issues and improved accessibility

## API Endpoints

### Authentication

- `POST /api/auth/login` - User login
- `POST /api/auth/logout` - User logout
- `GET /api/auth/me` - Get current user info

### Assignments

- `GET /api/assignment` - Get active assignments
- `GET /api/assignment/{id}` - Get assignment details
- `GET /api/assignment/{id}/with-questions` - Get assignment with questions

### Quiz Management

- `GET /api/quiz/progress/{assignmentId}` - Get user progress
- `POST /api/quiz/start/{assignmentId}` - Start assignment
- `POST /api/quiz/submit/{assignmentId}` - Submit quiz answers
- `GET /api/quiz/report` - Get progress report (Manager only) - Returns ProgressReport array
- `GET /api/quiz/answers/{userId}/{assignmentId}` - Get detailed quiz answers (Manager only) - Returns UserAnswerDetails
- `GET /api/quiz/check-completion/{assignmentId}` - Check completion status

## Testing the Application

### Quick Start Testing

1. **Login as Manager**

   - Navigate to `http://localhost:5219/login`
   - Username: `manager`, Password: `password123`
   - Access Progress Report to view all 10 learners

2. **Login as Learner**
   - Username: `learner1` to `learner10`, Password: `password123`
   - View assignments and complete quizzes

### API Testing with Postman

1. Import the included Postman collection: `LMS-Arna-Task-API.postman_collection.json`
2. Set the base URL to `http://localhost:5219`
3. Test authentication endpoints first
4. Use the Progress Report endpoint to verify all learners are returned

### Expected Progress Report Data

The Progress Report should show:

- **10 total learners** (learner1 through learner10)
- **6 completed assignments** with scores ranging from 40-100
- **1 in-progress assignment** (learner8)
- **3 not-started assignments** (learner1, learner7, learner9)

## Development

### Running in Development Mode

```bash
dotnet watch run
```

### Database Migrations

The application uses EF Core with automatic database creation for SQL Server. To create migrations manually:

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

**Note**: The application will automatically create the `lmsarna` database on SQL Server if it doesn't exist during the first migration.

### Testing

Run comprehensive unit tests for the scoring logic:

```bash
dotnet test
```

The test suite includes:

- ✅ **8 comprehensive unit tests** for quiz scoring logic
- ✅ Edge cases (empty answers, non-existent assignments)
- ✅ Various score scenarios (0%, 20%, 60%, 80%, 100%)
- ✅ In-memory database testing with realistic data

Basic unit tests can be added for the service layer logic, particularly the scoring algorithm.

## Project Structure

```
lms-task-arna/
├── lms-arna-task/                    # Main application
│   ├── Components/
│   │   ├── Layout/
│   │   │   ├── MainLayout.razor
│   │   │   └── NavMenu.razor
│   │   └── Pages/
│   │       ├── Home.razor            # Dashboard
│   │       ├── Login.razor           # Authentication
│   │       ├── Assignments.razor    # Assignment list
│   │       ├── AssignmentDetail.razor # Quiz interface
│   │       ├── Report.razor          # Progress report
│   │       └── AnswerDetails.razor   # Detailed answer view
│   ├── Controllers/
│   │   ├── AuthController.cs         # Authentication API
│   │   ├── AssignmentController.cs   # Assignment API
│   │   └── QuizController.cs         # Quiz & progress API
│   ├── Services/                     # Business logic layer
│   │   ├── Interfaces/
│   │   │   ├── IUserService.cs
│   │   │   ├── IAssignmentService.cs
│   │   │   └── IQuizService.cs
│   │   ├── UserService.cs
│   │   ├── AssignmentService.cs
│   │   ├── QuizService.cs
│   │   └── CustomAuthenticationStateProvider.cs
│   ├── Repositories/                 # Data access layer
│   │   ├── Interfaces/
│   │   │   ├── IUserRepository.cs
│   │   │   ├── IAssignmentRepository.cs
│   │   │   └── IQuizRepository.cs
│   │   ├── UserRepository.cs
│   │   ├── AssignmentRepository.cs
│   │   └── QuizRepository.cs
│   ├── Data/
│   │   └── ApplicationDbContext.cs   # EF Core context
│   ├── Models/                       # Database entities
│   │   ├── User.cs
│   │   ├── Assignment.cs
│   │   ├── Question.cs
│   │   ├── AssignmentProgress.cs
│   │   └── UserAnswer.cs
│   ├── DTOs/                         # Data transfer objects
│   │   ├── ProgressReport.cs
│   │   ├── UserAnswerDetails.cs
│   │   └── QuestionDetail.cs
│   ├── Migrations/                   # EF Core migrations
│   └── Program.cs                    # Application startup
└── lms-arna-task.Tests/             # Unit tests
    └── QuizScoringTests.cs          # Comprehensive scoring tests
```

## Recent Improvements

### Clean Architecture Implementation

- **Repository Pattern**: Complete refactoring to implement repository pattern
- **Dependency Injection**: All services and repositories properly registered
- **Separation of Concerns**: Clear separation between Controllers, Services, and Repositories
- **Testability**: Services can now be easily unit tested with mocked repositories

### Unit Testing Suite

- **Comprehensive Test Coverage**: 8 unit tests covering all scoring scenarios
- **In-Memory Database**: Tests use Entity Framework In-Memory provider
- **Edge Case Testing**: Tests for empty answers, non-existent assignments, partial answers
- **Scoring Validation**: Tests verify correct percentage calculations (0%, 20%, 60%, 80%, 100%)

### Enhanced Answer Details

- **Manager-Only Access**: Detailed answer breakdown for completed assignments
- **Question-by-Question Analysis**: Visual breakdown of correct/incorrect answers
- **Performance Indicators**: Color-coded feedback for answer accuracy
- **Navigation Improvements**: Fixed button navigation issues with anchor-based routing

### Progress Report Enhancements

- **Complete Learner Coverage**: Fixed issue where only 7 out of 10 learners were shown in progress reports
- **Virtual Progress Entries**: Learners who haven't started assignments now appear with "Not Started" status
- **Data Integrity**: All learners in the manager's team are now guaranteed to appear in progress reports

### Model Architecture Refactoring

- **Clean Separation**: DTOs moved from `Models/` to dedicated `DTOs/` folder
- **Consistent Naming**: Removed "Dto" suffix from class names for cleaner code
- **Namespace Organization**: DTOs now use `lms_arna_task.DTOs` namespace
- **Better Maintainability**: Clear distinction between database entities and API data transfer objects

### UI/UX Improvements

- **Responsive Navigation**: Welcome message in navigation bar now adapts to different screen sizes
- **Mobile Optimization**: Improved text handling on mobile devices with proper ellipsis and sizing
- **Bootstrap Icons**: Added complete icon set for all navigation items
- **Clean Interface**: Removed debug output and polished the user interface

### Technical Improvements

- **API Optimization**: Streamlined data transfer with purpose-built DTOs
- **Error Handling**: Improved error handling and data validation
- **Performance**: Optimized queries and reduced data transfer overhead
- **Code Quality**: Enhanced maintainability with better separation of concerns

## Success Criteria ✅

- ✅ **Blazor project runs locally** (localhost:5219)
- ✅ **Clean, functional UI** with Bootstrap styling
- ✅ **Authentication & Authorization** with role-based access
- ✅ **Clean Architecture** with Repository pattern implementation
- ✅ **Modular CRUD logic** separated into services and repositories
- ✅ **Quiz/Scoring functionality** working correctly with comprehensive testing
- ✅ **Unit Tests** - 8 comprehensive tests for scoring logic (all passing)
- ✅ **Clean database structure** with proper relationships
- ✅ **Assignment workflow** complete (view → material → quiz → submit)
- ✅ **Manager reporting** with progress tracking and detailed answer analysis
- ✅ **Git Repository** initialized with comprehensive .gitignore

## License

This project is for educational/demonstration purposes.
