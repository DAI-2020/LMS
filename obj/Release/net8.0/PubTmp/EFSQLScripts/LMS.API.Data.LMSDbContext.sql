IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [FAQs] (
        [Id] int NOT NULL IDENTITY,
        [Question] nvarchar(500) NOT NULL,
        [Answer] nvarchar(2000) NOT NULL,
        CONSTRAINT [PK_FAQs] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [Roles] (
        [Id] int NOT NULL,
        [Name] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [Topics] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_Topics] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [FullName] nvarchar(100) NOT NULL,
        [Email] nvarchar(150) NOT NULL,
        [PasswordHash] nvarchar(256) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [Gender] nvarchar(max) NOT NULL,
        [DateOfBirth] date NULL,
        [Address] nvarchar(250) NULL,
        [PhoneNumber] nvarchar(20) NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [CommunityPosts] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_CommunityPosts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CommunityPosts_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [Courses] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(200) NOT NULL,
        [Description] nvarchar(500) NOT NULL,
        [InstructorId] int NOT NULL,
        CONSTRAINT [PK_Courses] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Courses_Users_InstructorId] FOREIGN KEY ([InstructorId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [GraduationProjectSubmissions] (
        [Id] int NOT NULL IDENTITY,
        [StudentId] int NOT NULL,
        [ProjectName] nvarchar(200) NOT NULL,
        [LeadProject] nvarchar(200) NOT NULL,
        [DescriptionProject] nvarchar(2000) NOT NULL,
        [UploadDocumentProject] nvarchar(500) NOT NULL,
        [ProjectStatus] nvarchar(max) NOT NULL,
        [SubmittedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_GraduationProjectSubmissions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_GraduationProjectSubmissions_Users_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [Notifications] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [NewLessons] bit NOT NULL DEFAULT CAST(1 AS bit),
        [LiveSessionReminders] bit NOT NULL DEFAULT CAST(1 AS bit),
        [AssignmentDeadlines] bit NOT NULL DEFAULT CAST(1 AS bit),
        [AssignmentGrading] bit NOT NULL DEFAULT CAST(1 AS bit),
        [QuizAlerts] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CommunityNotifications] bit NOT NULL DEFAULT CAST(1 AS bit),
        [AiRecommendations] bit NOT NULL DEFAULT CAST(0 AS bit),
        [SecurityAlerts] bit NOT NULL DEFAULT CAST(1 AS bit),
        CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Notifications_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [Tickets] (
        [Id] int NOT NULL IDENTITY,
        [StudentId] int NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Category] nvarchar(max) NOT NULL,
        [Priority] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [UserId] int NULL,
        CONSTRAINT [PK_Tickets] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Tickets_Users_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Tickets_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [user_devices] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [DeviceName] nvarchar(150) NOT NULL,
        [ClientInfo] nvarchar(250) NULL,
        [RefreshTokenHash] nvarchar(500) NOT NULL,
        [LastUsed] datetime2 NOT NULL,
        CONSTRAINT [PK_user_devices] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_user_devices_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [UserRoles] (
        [UserId] int NOT NULL,
        [RoleId] int NOT NULL,
        CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [AIAssistantChats] (
        [Id] int NOT NULL IDENTITY,
        [StudentId] int NOT NULL,
        [CourseId] int NOT NULL,
        [UserQuery] nvarchar(max) NOT NULL,
        [AIResponse] nvarchar(max) NOT NULL,
        [AskedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_AIAssistantChats] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AIAssistantChats_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AIAssistantChats_Users_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [LiveSessions] (
        [Id] int NOT NULL IDENTITY,
        [CourseId] int NOT NULL,
        [Title] nvarchar(200) NOT NULL,
        [ScheduledAt] datetime2 NOT NULL,
        [DurationMinutes] int NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [WeekNumber] int NOT NULL,
        [Type] nvarchar(max) NOT NULL,
        [Mode] nvarchar(max) NOT NULL,
        [RecordingUrl] nvarchar(max) NULL,
        CONSTRAINT [PK_LiveSessions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_LiveSessions_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [Quizzes] (
        [Id] int NOT NULL IDENTITY,
        [CourseId] int NOT NULL,
        [TopicId] int NOT NULL,
        [StudentId] int NOT NULL,
        [Score] float NOT NULL,
        [TakenAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Quizzes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Quizzes_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Quizzes_Topics_TopicId] FOREIGN KEY ([TopicId]) REFERENCES [Topics] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Quizzes_Users_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [TicketReplies] (
        [Id] int NOT NULL IDENTITY,
        [TicketId] int NOT NULL,
        [UserId] int NOT NULL,
        [Message] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_TicketReplies] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TicketReplies_Tickets_TicketId] FOREIGN KEY ([TicketId]) REFERENCES [Tickets] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_TicketReplies_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [AttendanceLogs] (
        [Id] int NOT NULL IDENTITY,
        [SessionId] int NOT NULL,
        [StudentId] int NOT NULL,
        [JoinTime] datetime2 NULL,
        [LeaveTime] datetime2 NULL,
        [MicrophoneUsageSeconds] int NOT NULL,
        [ParticipationLevel] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [EngagementScore] int NOT NULL,
        [UserId] int NULL,
        CONSTRAINT [PK_AttendanceLogs] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AttendanceLogs_LiveSessions_SessionId] FOREIGN KEY ([SessionId]) REFERENCES [LiveSessions] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AttendanceLogs_Users_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_AttendanceLogs_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [CourseTasks] (
        [Id] int NOT NULL IDENTITY,
        [CourseId] int NOT NULL,
        [SessionId] int NULL,
        [Title] nvarchar(max) NOT NULL,
        [Description] nvarchar(1000) NOT NULL,
        [TaskType] nvarchar(max) NOT NULL,
        [AssignmentStatus] nvarchar(max) NOT NULL,
        [DueDate] datetime2 NOT NULL,
        [AllowedExtensions] nvarchar(max) NULL,
        CONSTRAINT [PK_CourseTasks] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CourseTasks_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CourseTasks_LiveSessions_SessionId] FOREIGN KEY ([SessionId]) REFERENCES [LiveSessions] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [Materials] (
        [Id] int NOT NULL IDENTITY,
        [CourseId] int NOT NULL,
        [SessionId] int NULL,
        [Title] nvarchar(max) NOT NULL,
        [MaterialType] nvarchar(max) NOT NULL,
        [AttachmentType] nvarchar(max) NOT NULL,
        [FileUrl] nvarchar(max) NOT NULL,
        [LiveSessionId] int NULL,
        CONSTRAINT [PK_Materials] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Materials_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Materials_LiveSessions_LiveSessionId] FOREIGN KEY ([LiveSessionId]) REFERENCES [LiveSessions] ([Id]),
        CONSTRAINT [FK_Materials_LiveSessions_SessionId] FOREIGN KEY ([SessionId]) REFERENCES [LiveSessions] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE TABLE [TaskSubmissions] (
        [Id] int NOT NULL IDENTITY,
        [TaskId] int NOT NULL,
        [StudentId] int NOT NULL,
        [FileUrl] nvarchar(max) NOT NULL,
        [SubmittedAt] datetime2 NOT NULL,
        [AssignmentStatus] nvarchar(max) NOT NULL,
        [AIGrade] nvarchar(max) NULL,
        [AIFeedback] nvarchar(max) NULL,
        CONSTRAINT [PK_TaskSubmissions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TaskSubmissions_CourseTasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [CourseTasks] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_TaskSubmissions_Users_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_AIAssistantChats_CourseId] ON [AIAssistantChats] ([CourseId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_AIAssistantChats_StudentId] ON [AIAssistantChats] ([StudentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_AttendanceLogs_SessionId] ON [AttendanceLogs] ([SessionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_AttendanceLogs_StudentId] ON [AttendanceLogs] ([StudentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_AttendanceLogs_UserId] ON [AttendanceLogs] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_CommunityPosts_UserId] ON [CommunityPosts] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_Courses_InstructorId] ON [Courses] ([InstructorId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_CourseTasks_CourseId] ON [CourseTasks] ([CourseId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_CourseTasks_SessionId] ON [CourseTasks] ([SessionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_GraduationProjectSubmissions_StudentId] ON [GraduationProjectSubmissions] ([StudentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_LiveSessions_CourseId] ON [LiveSessions] ([CourseId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_Materials_CourseId] ON [Materials] ([CourseId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_Materials_LiveSessionId] ON [Materials] ([LiveSessionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_Materials_SessionId] ON [Materials] ([SessionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_Notifications_UserId] ON [Notifications] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_Quizzes_CourseId] ON [Quizzes] ([CourseId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_Quizzes_StudentId] ON [Quizzes] ([StudentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_Quizzes_TopicId] ON [Quizzes] ([TopicId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_TaskSubmissions_StudentId] ON [TaskSubmissions] ([StudentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_TaskSubmissions_TaskId] ON [TaskSubmissions] ([TaskId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_TicketReplies_TicketId] ON [TicketReplies] ([TicketId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_TicketReplies_UserId] ON [TicketReplies] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_Tickets_StudentId] ON [Tickets] ([StudentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_Tickets_UserId] ON [Tickets] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_user_devices_UserId] ON [user_devices] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714135103_final'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260714135103_final', N'8.0.18');
END;
GO

COMMIT;
GO

