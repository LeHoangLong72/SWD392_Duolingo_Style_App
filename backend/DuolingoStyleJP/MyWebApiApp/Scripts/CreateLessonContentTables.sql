-- Add columns to AspNetUsers table (if not exist)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'CurrentXP')
BEGIN
    ALTER TABLE AspNetUsers ADD CurrentXP INT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'TotalXP')
BEGIN
    ALTER TABLE AspNetUsers ADD TotalXP INT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'Level')
BEGIN
    ALTER TABLE AspNetUsers ADD [Level] INT NOT NULL DEFAULT 1;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'Hearts')
BEGIN
    ALTER TABLE AspNetUsers ADD Hearts INT NOT NULL DEFAULT 5;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'MaxHearts')
BEGIN
    ALTER TABLE AspNetUsers ADD MaxHearts INT NOT NULL DEFAULT 5;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'LastHeartRefillTime')
BEGIN
    ALTER TABLE AspNetUsers ADD LastHeartRefillTime DATETIME2 NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'Gems')
BEGIN
    ALTER TABLE AspNetUsers ADD Gems INT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'CurrentStreak')
BEGIN
    ALTER TABLE AspNetUsers ADD CurrentStreak INT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'LongestStreak')
BEGIN
    ALTER TABLE AspNetUsers ADD LongestStreak INT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'LastStudyDate')
BEGIN
    ALTER TABLE AspNetUsers ADD LastStudyDate DATETIME2 NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'StreakFreezeCount')
BEGIN
    ALTER TABLE AspNetUsers ADD StreakFreezeCount INT NOT NULL DEFAULT 0;
END

-- Create Questions table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Questions]') AND type in (N'U'))
BEGIN
    CREATE TABLE Questions (
        QuestionId INT PRIMARY KEY IDENTITY(1,1),
        LessonId INT NOT NULL,
        QuestionType NVARCHAR(50) NOT NULL,
        QuestionText NVARCHAR(MAX) NOT NULL,
        AudioUrl NVARCHAR(500) NULL,
        ImageUrl NVARCHAR(500) NULL,
        CorrectAnswer NVARCHAR(MAX) NOT NULL,
        OrderIndex INT NOT NULL,
        XPReward INT NOT NULL DEFAULT 10,
        CONSTRAINT FK_Questions_Lessons FOREIGN KEY (LessonId) REFERENCES Lessons(LessonId)
    );
END

-- Create QuestionOptions table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QuestionOptions]') AND type in (N'U'))
BEGIN
    CREATE TABLE QuestionOptions (
        OptionId INT PRIMARY KEY IDENTITY(1,1),
        QuestionId INT NOT NULL,
        OptionText NVARCHAR(MAX) NOT NULL,
        ImageUrl NVARCHAR(500) NULL,
        AudioUrl NVARCHAR(500) NULL,
        IsCorrect BIT NOT NULL,
        OrderIndex INT NOT NULL,
        CONSTRAINT FK_QuestionOptions_Questions FOREIGN KEY (QuestionId) 
            REFERENCES Questions(QuestionId) ON DELETE CASCADE
    );
END

-- Create UserAnswers table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserAnswers]') AND type in (N'U'))
BEGIN
    CREATE TABLE UserAnswers (
        UserAnswerId INT PRIMARY KEY IDENTITY(1,1),
        UserId NVARCHAR(450) NOT NULL,
        QuestionId INT NOT NULL,
        LessonId INT NOT NULL,
        AnswerGiven NVARCHAR(MAX) NOT NULL,
        IsCorrect BIT NOT NULL,
        XPEarned INT NOT NULL,
        AnsweredAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_UserAnswers_Users FOREIGN KEY (UserId) 
            REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
        CONSTRAINT FK_UserAnswers_Questions FOREIGN KEY (QuestionId) 
            REFERENCES Questions(QuestionId)
    );
END

-- Create LessonAttempts table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LessonAttempts]') AND type in (N'U'))
BEGIN
    CREATE TABLE LessonAttempts (
        AttemptId INT PRIMARY KEY IDENTITY(1,1),
        UserId NVARCHAR(450) NOT NULL,
        LessonId INT NOT NULL,
        StartedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CompletedAt DATETIME2 NULL,
        TotalQuestions INT NOT NULL,
        CorrectAnswers INT NOT NULL,
        TotalXPEarned INT NOT NULL,
        HeartsLost INT NOT NULL,
        IsCompleted BIT NOT NULL,
        AccuracyRate FLOAT NOT NULL,
        CONSTRAINT FK_LessonAttempts_Users FOREIGN KEY (UserId) 
            REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
        CONSTRAINT FK_LessonAttempts_Lessons FOREIGN KEY (LessonId) 
            REFERENCES Lessons(LessonId)
    );
END

PRINT 'Tables created successfully!'
