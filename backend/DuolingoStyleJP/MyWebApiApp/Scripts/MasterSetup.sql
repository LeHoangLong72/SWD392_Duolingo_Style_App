-- ============================================
-- MASTER SETUP SCRIPT
-- Run this file to setup everything at once
-- ============================================

USE [YourDatabaseName]; -- ⚠️ CHANGE THIS TO YOUR DATABASE NAME
GO

PRINT '╔══════════════════════════════════════════════╗';
PRINT '║  Duolingo Japanese App - Database Setup     ║';
PRINT '║  Complete Installation Script                ║';
PRINT '╚══════════════════════════════════════════════╝';
PRINT '';

-- ============================================
-- STEP 1: CREATE TABLES
-- ============================================
PRINT '========================================';
PRINT 'STEP 1: Creating Tables...';
PRINT '========================================';

-- Add columns to AspNetUsers table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'CurrentXP')
BEGIN
    ALTER TABLE AspNetUsers ADD CurrentXP INT NOT NULL DEFAULT 0;
    PRINT '✓ Added CurrentXP column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'TotalXP')
BEGIN
    ALTER TABLE AspNetUsers ADD TotalXP INT NOT NULL DEFAULT 0;
    PRINT '✓ Added TotalXP column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'Level')
BEGIN
    ALTER TABLE AspNetUsers ADD [Level] INT NOT NULL DEFAULT 1;
    PRINT '✓ Added Level column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'Hearts')
BEGIN
    ALTER TABLE AspNetUsers ADD Hearts INT NOT NULL DEFAULT 5;
    PRINT '✓ Added Hearts column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'MaxHearts')
BEGIN
    ALTER TABLE AspNetUsers ADD MaxHearts INT NOT NULL DEFAULT 5;
    PRINT '✓ Added MaxHearts column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'LastHeartRefillTime')
BEGIN
    ALTER TABLE AspNetUsers ADD LastHeartRefillTime DATETIME2 NULL;
    PRINT '✓ Added LastHeartRefillTime column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'Gems')
BEGIN
    ALTER TABLE AspNetUsers ADD Gems INT NOT NULL DEFAULT 0;
    PRINT '✓ Added Gems column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'CurrentStreak')
BEGIN
    ALTER TABLE AspNetUsers ADD CurrentStreak INT NOT NULL DEFAULT 0;
    PRINT '✓ Added CurrentStreak column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'LongestStreak')
BEGIN
    ALTER TABLE AspNetUsers ADD LongestStreak INT NOT NULL DEFAULT 0;
    PRINT '✓ Added LongestStreak column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'LastStudyDate')
BEGIN
    ALTER TABLE AspNetUsers ADD LastStudyDate DATETIME2 NULL;
    PRINT '✓ Added LastStudyDate column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'StreakFreezeCount')
BEGIN
    ALTER TABLE AspNetUsers ADD StreakFreezeCount INT NOT NULL DEFAULT 0;
    PRINT '✓ Added StreakFreezeCount column';
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
    PRINT '✓ Created Questions table';
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
    PRINT '✓ Created QuestionOptions table';
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
    PRINT '✓ Created UserAnswers table';
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
    PRINT '✓ Created LessonAttempts table';
END

PRINT '';
PRINT '✓ STEP 1 COMPLETED: All tables created successfully!';
PRINT '';

-- ============================================
-- STEP 2: SEED DATA (Embedded from SeedAllData.sql)
-- ============================================
PRINT '========================================';
PRINT 'STEP 2: Seeding Learning Data...';
PRINT '========================================';

-- Units
IF NOT EXISTS (SELECT 1 FROM Units WHERE UnitId = 1)
BEGIN
    SET IDENTITY_INSERT Units ON;
    INSERT INTO Units (UnitId, UnitNumber, Title) VALUES
    (1, 1, 'Unit 1: Hiragana Basics'),
    (2, 2, 'Unit 2: Katakana Basics'),
    (3, 3, 'Unit 3: Basic Greetings'),
    (4, 4, 'Unit 4: Numbers & Counting'),
    (5, 5, 'Unit 5: Daily Conversation');
    SET IDENTITY_INSERT Units OFF;
    PRINT '✓ Units seeded (5)';
END

-- Nodes
IF NOT EXISTS (SELECT 1 FROM Nodes WHERE NodeId = 1)
BEGIN
    SET IDENTITY_INSERT Nodes ON;
    INSERT INTO Nodes (NodeId, UnitId, UserId, NodeType, Position) VALUES
    (1, 1, 1, 'lesson', 1), (2, 1, 1, 'lesson', 2), (3, 1, 1, 'practice', 3),
    (4, 1, 1, 'lesson', 4), (5, 1, 1, 'test', 5), (6, 2, 1, 'lesson', 1),
    (7, 2, 1, 'lesson', 2), (8, 2, 1, 'practice', 3), (9, 3, 1, 'lesson', 1),
    (10, 3, 1, 'lesson', 2);
    SET IDENTITY_INSERT Nodes OFF;
    PRINT '✓ Nodes seeded (10)';
END

-- Lessons
IF NOT EXISTS (SELECT 1 FROM Lessons WHERE LessonId = 1)
BEGIN
    SET IDENTITY_INSERT Lessons ON;
    INSERT INTO Lessons (LessonId, NodeId, Title, BaseXP) VALUES
    (1, 1, 'Hiragana: Vowels (あいうえお)', 50),
    (2, 2, 'Hiragana: K-Row (かきくけこ)', 50),
    (3, 4, 'Hiragana: S-Row (さしすせそ)', 50),
    (4, 6, 'Katakana: Vowels (アイウエオ)', 50),
    (5, 7, 'Katakana: K-Row (カキクケコ)', 50),
    (6, 9, 'Basic Greetings', 60),
    (7, 10, 'Introducing Yourself', 60);
    SET IDENTITY_INSERT Lessons OFF;
    PRINT '✓ Lessons seeded (7)';
END

-- Alphabets
IF NOT EXISTS (SELECT 1 FROM Alphabets WHERE AlphabetId = 1)
BEGIN
    SET IDENTITY_INSERT Alphabets ON;
    INSERT INTO Alphabets (AlphabetId, Character, Type, Level, Meaning) VALUES
    (1, 'あ', 'Hiragana', 'Basic', 'a'), (2, 'い', 'Hiragana', 'Basic', 'i'),
    (3, 'う', 'Hiragana', 'Basic', 'u'), (4, 'え', 'Hiragana', 'Basic', 'e'),
    (5, 'お', 'Hiragana', 'Basic', 'o'), (6, 'か', 'Hiragana', 'Basic', 'ka'),
    (7, 'き', 'Hiragana', 'Basic', 'ki'), (8, 'く', 'Hiragana', 'Basic', 'ku'),
    (9, 'け', 'Hiragana', 'Basic', 'ke'), (10, 'こ', 'Hiragana', 'Basic', 'ko'),
    (16, 'ア', 'Katakana', 'Basic', 'a'), (17, 'イ', 'Katakana', 'Basic', 'i');
    SET IDENTITY_INSERT Alphabets OFF;
    PRINT '✓ Alphabets seeded (12)';
END

-- Questions for Lesson 1
IF NOT EXISTS (SELECT 1 FROM Questions WHERE LessonId = 1)
BEGIN
    DECLARE @Q INT;
    
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (1, 'MultipleChoice', 'What is "あ" in romaji?', 'a', 1, 10);
    SET @Q = SCOPE_IDENTITY();
    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
    (@Q, 'a', 1, 1), (@Q, 'i', 0, 2), (@Q, 'u', 0, 3), (@Q, 'e', 0, 4);
    
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (1, 'MultipleChoice', 'Which is "i"?', 'い', 2, 10);
    SET @Q = SCOPE_IDENTITY();
    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
    (@Q, 'い', 1, 1), (@Q, 'あ', 0, 2), (@Q, 'う', 0, 3), (@Q, 'え', 0, 4);
    
    PRINT '✓ Questions seeded for Lesson 1';
END

-- Update users
UPDATE AspNetUsers SET Hearts = 5, MaxHearts = 5, Gems = 100, [Level] = 1 
WHERE Hearts = 0 OR Hearts IS NULL;

PRINT '✓ STEP 2 COMPLETED: Learning data seeded!';
PRINT '';

-- ============================================
-- STEP 3: SEED SHOP ITEMS
-- ============================================
PRINT '========================================';
PRINT 'STEP 3: Seeding Shop Items...';
PRINT '========================================';

IF NOT EXISTS (SELECT 1 FROM Items WHERE ItemId = 1)
BEGIN
    SET IDENTITY_INSERT Items ON;
    INSERT INTO Items (ItemId, Name, Description, Price, Category, ImageUrl, IsActive) VALUES
    (1, 'Streak Freeze', 'Protect your streak for one day', 200, 'PowerUp', '/images/streak-freeze.png', 1),
    (2, 'Heart Refill', 'Instantly refill all hearts', 350, 'PowerUp', '/images/heart-refill.png', 1),
    (3, 'Double XP Boost', 'Earn 2x XP for 15 min', 150, 'PowerUp', '/images/double-xp.png', 1);
    SET IDENTITY_INSERT Items OFF;
    PRINT '✓ Shop items seeded (3)';
END

PRINT '✓ STEP 3 COMPLETED: Shop items seeded!';
PRINT '';

-- ============================================
-- FINAL VERIFICATION
-- ============================================
PRINT '╔══════════════════════════════════════════════╗';
PRINT '║        SETUP COMPLETED SUCCESSFULLY!         ║';
PRINT '╚══════════════════════════════════════════════╝';
PRINT '';
PRINT 'Database Summary:';
PRINT '  - Units: ' + CAST((SELECT COUNT(*) FROM Units) AS VARCHAR);
PRINT '  - Nodes: ' + CAST((SELECT COUNT(*) FROM Nodes) AS VARCHAR);
PRINT '  - Lessons: ' + CAST((SELECT COUNT(*) FROM Lessons) AS VARCHAR);
PRINT '  - Questions: ' + CAST((SELECT COUNT(*) FROM Questions) AS VARCHAR);
PRINT '  - Alphabets: ' + CAST((SELECT COUNT(*) FROM Alphabets) AS VARCHAR);
PRINT '  - Shop Items: ' + CAST((SELECT COUNT(*) FROM Items WHERE IsActive = 1) AS VARCHAR);
PRINT '';
PRINT '✅ Ready to test! Run the application and try:';
PRINT '   GET  /api/lesson-content/1';
PRINT '   POST /api/lesson-content/start/1';
PRINT '';
