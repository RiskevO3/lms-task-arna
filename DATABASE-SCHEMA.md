# Database Schema - Entity Relationship Diagram

## Tables Overview

### 1. Users

- **Primary Key**: Id (int, auto-increment)
- **Columns**:
  - Id (int, PK)
  - Username (string, unique, required)
  - Email (string, unique, required)
  - PasswordHash (string, required)
  - Role (string, required) - "Manager" or "Learner"
  - ManagerId (int, nullable, FK to Users.Id)
  - CreatedAt (datetime)
  - UpdatedAt (datetime)

### 2. Assignments

- **Primary Key**: Id (int, auto-increment)
- **Columns**:
  - Id (int, PK)
  - Title (string, required)
  - Description (string, required)
  - MaterialContent (string, required) - URL or content
  - MaterialType (string, required) - "PDF", "Video", "Text"
  - StartDate (datetime)
  - EndDate (datetime)
  - IsActive (boolean)
  - CreatedById (int, FK to Users.Id)
  - CreatedAt (datetime)
  - UpdatedAt (datetime)

### 3. Questions

- **Primary Key**: Id (int, auto-increment)
- **Columns**:
  - Id (int, PK)
  - QuestionText (string, required)
  - OptionA (string, required)
  - OptionB (string, required)
  - OptionC (string, required)
  - OptionD (string, required)
  - CorrectAnswer (string, required) - "A", "B", "C", or "D"
  - AssignmentId (int, FK to Assignments.Id)
  - CreatedAt (datetime)
  - UpdatedAt (datetime)

### 4. AssignmentProgresses

- **Primary Key**: Id (int, auto-increment)
- **Columns**:
  - Id (int, PK)
  - UserId (int, FK to Users.Id)
  - AssignmentId (int, FK to Assignments.Id)
  - Score (int, 0-100)
  - IsCompleted (boolean)
  - CompletedAt (datetime, nullable)
  - StartedAt (datetime)
  - UpdatedAt (datetime)
- **Unique Constraint**: (UserId, AssignmentId)

### 5. UserAnswers

- **Primary Key**: Id (int, auto-increment)
- **Columns**:
  - Id (int, PK)
  - UserId (int, FK to Users.Id)
  - QuestionId (int, FK to Questions.Id)
  - AssignmentProgressId (int, FK to AssignmentProgresses.Id)
  - SelectedAnswer (string, required) - "A", "B", "C", or "D"
  - IsCorrect (boolean)
  - AnsweredAt (datetime)

## Relationships

### Users Relationships

- **Self-referencing**: Users → Users (Manager relationship)

  - One manager can have many subordinates
  - One subordinate has one manager (optional)
  - Relationship: `ManagerId` → `Users.Id`

- **One-to-Many**: Users → Assignments (Created by)

  - One user can create many assignments
  - One assignment is created by one user
  - Relationship: `CreatedById` → `Users.Id`

- **One-to-Many**: Users → AssignmentProgresses

  - One user can have many assignment progresses
  - One assignment progress belongs to one user
  - Relationship: `UserId` → `Users.Id`

- **One-to-Many**: Users → UserAnswers
  - One user can have many user answers
  - One user answer belongs to one user
  - Relationship: `UserId` → `Users.Id`

### Assignments Relationships

- **One-to-Many**: Assignments → Questions

  - One assignment can have many questions
  - One question belongs to one assignment
  - Relationship: `AssignmentId` → `Assignments.Id`

- **One-to-Many**: Assignments → AssignmentProgresses
  - One assignment can have many progresses
  - One progress belongs to one assignment
  - Relationship: `AssignmentId` → `Assignments.Id`

### Questions Relationships

- **One-to-Many**: Questions → UserAnswers
  - One question can have many user answers
  - One user answer belongs to one question
  - Relationship: `QuestionId` → `Questions.Id`

### AssignmentProgresses Relationships

- **One-to-Many**: AssignmentProgresses → UserAnswers
  - One progress can have many user answers
  - One user answer belongs to one progress
  - Relationship: `AssignmentProgressId` → `AssignmentProgresses.Id`

## ERD Diagram (Text Format)

```
┌─────────────────┐     ┌─────────────────┐
│     Users       │     │   Assignments   │
├─────────────────┤     ├─────────────────┤
│ Id (PK)         │────┐│ Id (PK)         │
│ Username (UQ)   │    ││ Title           │
│ Email (UQ)      │    ││ Description     │
│ PasswordHash    │    ││ MaterialContent │
│ Role            │    ││ MaterialType    │
│ ManagerId (FK)  │─┐  ││ StartDate       │
│ CreatedAt       │ │  ││ EndDate         │
│ UpdatedAt       │ │  ││ IsActive        │
└─────────────────┘ │  ││ CreatedById (FK)│─┘
        │           │  ││ CreatedAt       │
        │           │  ││ UpdatedAt       │
        │           │  │└─────────────────┘
        │           │  │         │
        │           │  │         │
        │           │  │         ▼
        │           │  │┌─────────────────┐
        │           │  ││   Questions     │
        │           │  │├─────────────────┤
        │           │  ││ Id (PK)         │
        │           │  ││ QuestionText    │
        │           │  ││ OptionA         │
        │           │  ││ OptionB         │
        │           │  ││ OptionC         │
        │           │  ││ OptionD         │
        │           │  ││ CorrectAnswer   │
        │           │  ││ AssignmentId(FK)│─┘
        │           │  ││ CreatedAt       │
        │           │  ││ UpdatedAt       │
        │           │  │└─────────────────┘
        │           │  │         │
        │           │  │         │
        │           │  │         │
        │           │  │         ▼
        │           │  │┌─────────────────┐
        │           │  ││  UserAnswers    │
        │           │  │├─────────────────┤
        │           │  ││ Id (PK)         │
        │           ├──┼┤ UserId (FK)     │
        │           │  ││ QuestionId (FK) │─┘
        │           │  ││ AssignmentProgressId(FK)│─┐
        │           │  ││ SelectedAnswer  │         │
        │           │  ││ IsCorrect       │         │
        │           │  ││ AnsweredAt      │         │
        │           │  │└─────────────────┘         │
        │           │  │                            │
        │           │  │                            │
        │           │  │ ┌─────────────────┐        │
        │           │  │ │AssignmentProgresses│     │
        │           │  │ ├─────────────────┤        │
        │           │  │ │ Id (PK)         │◄───────┘
        │           └──┼─┤ UserId (FK)     │
        │              │ │ AssignmentId(FK)│─┐
        │              │ │ Score           │ │
        │              │ │ IsCompleted     │ │
        │              │ │ CompletedAt     │ │
        │              │ │ StartedAt       │ │
        │              │ │ UpdatedAt       │ │
        │              │ └─────────────────┘ │
        │              │                     │
        │              └─────────────────────┘
        │
        └─── (Self-referencing: Manager relationship)
```

## Data Transfer Objects (DTOs)

### ProgressReportDto

Used for API responses to provide clean, serializable data without circular references:

- **Properties**:
  - Id (int) - AssignmentProgress ID (0 for virtual entries)
  - UserId (int) - User ID
  - Username (string) - User's username
  - Email (string) - User's email address
  - AssignmentId (int) - Assignment ID
  - AssignmentTitle (string) - Assignment title
  - AssignmentDescription (string) - Assignment description
  - Score (int) - User's score (0-100)
  - IsCompleted (boolean) - Completion status
  - CompletedAt (datetime, nullable) - Completion timestamp
  - StartedAt (datetime) - Start timestamp (DateTime.MinValue for "Not Started")
  - UpdatedAt (datetime) - Last update timestamp

**Note**: Virtual entries are created for users who haven't started assignments, ensuring all team members appear in progress reports.

## Business Rules

1. **User Hierarchy**: Users can have a manager (self-referencing relationship)
2. **Assignment Access**: Only active assignments within date range are visible
3. **Single Submission**: Each user can only submit once per assignment (unique constraint)
4. **Scoring**: Each correct answer = 20 points (5 questions × 20 = 100 points total)
5. **Manager Visibility**: Managers can only see progress of their direct subordinates
6. **Complete Team Coverage**: All team members appear in progress reports, including those who haven't started
7. **Virtual Progress Entries**: Users without progress records appear as "Not Started" in reports
8. **Question Dependency**: Questions belong to assignments and are deleted when assignment is deleted
9. **Progress Tracking**: AssignmentProgress is created when user starts an assignment
10. **Answer Recording**: UserAnswers are created when user submits the quiz

## Sample Data

### Users (11 total)

- **1 Manager**: `manager` (manages all learners)
- **10 Learners**: `learner1` through `learner10` (all report to manager)

### Assignments

- **1 Active Assignment**: "Introduction to Programming" with 5 multiple-choice questions

### Progress Distribution

- **6 Completed**: learner2 (100%), learner3 (40%), learner4 (80%), learner5 (60%), learner6 (40%), learner10 (60%)
- **1 In Progress**: learner8 (started but not completed)
- **3 Not Started**: learner1, learner7, learner9 (will show as virtual entries with "Not Started" status)

## Indexes

- **Users**: Username (unique), Email (unique)
- **AssignmentProgresses**: UserId + AssignmentId (unique)
- **Foreign Key Indexes**: All foreign key relationships have indexes for performance
