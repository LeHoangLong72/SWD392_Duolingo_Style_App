-- ============================================
-- Complete Seed Data for Duolingo Japanese App
-- ============================================

-- Clear existing data (optional - uncomment if needed)
-- DELETE FROM UserAnswers;
-- DELETE FROM LessonAttempts;
-- DELETE FROM QuestionOptions;
-- DELETE FROM Questions;
-- DELETE FROM UserLessonProgresses;
-- DELETE FROM Lessons;
-- DELETE FROM Nodes;
-- DELETE FROM Units;
-- DELETE FROM Alphabets;

PRINT 'Starting data seeding...';

-- ============================================
-- 1. SEED UNITS (Learning Units)
-- ============================================
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
    PRINT '✓ Units seeded successfully';
END
ELSE
BEGIN
    PRINT '✓ Units already exist';
END

-- ============================================
-- 2. SEED NODES (Learning Path Nodes)
-- ============================================
IF NOT EXISTS (SELECT 1 FROM Nodes WHERE NodeId = 1)
BEGIN
    SET IDENTITY_INSERT Nodes ON;
    
    INSERT INTO Nodes (NodeId, UnitId, UserId, NodeType, Position) VALUES
    -- Unit 1 Nodes
    (1, 1, 1, 'lesson', 1),
    (2, 1, 1, 'lesson', 2),
    (3, 1, 1, 'practice', 3),
    (4, 1, 1, 'lesson', 4),
    (5, 1, 1, 'test', 5),
    
    -- Unit 2 Nodes
    (6, 2, 1, 'lesson', 1),
    (7, 2, 1, 'lesson', 2),
    (8, 2, 1, 'practice', 3),
    
    -- Unit 3 Nodes
    (9, 3, 1, 'lesson', 1),
    (10, 3, 1, 'lesson', 2);
    
    SET IDENTITY_INSERT Nodes OFF;
    PRINT '✓ Nodes seeded successfully';
END
ELSE
BEGIN
    PRINT '✓ Nodes already exist';
END

-- ============================================
-- 3. SEED LESSONS
-- ============================================
IF NOT EXISTS (SELECT 1 FROM Lessons WHERE LessonId = 1)
BEGIN
    SET IDENTITY_INSERT Lessons ON;
    
    INSERT INTO Lessons (LessonId, NodeId, Title, BaseXP) VALUES
    -- Unit 1 Lessons
    (1, 1, 'Hiragana: Vowels (あいうえお)', 50),
    (2, 2, 'Hiragana: K-Row (かきくけこ)', 50),
    (3, 4, 'Hiragana: S-Row (さしすせそ)', 50),
    
    -- Unit 2 Lessons
    (4, 6, 'Katakana: Vowels (アイウエオ)', 50),
    (5, 7, 'Katakana: K-Row (カキクケコ)', 50),
    
    -- Unit 3 Lessons
    (6, 9, 'Basic Greetings', 60),
    (7, 10, 'Introducing Yourself', 60);
    
    SET IDENTITY_INSERT Lessons OFF;
    PRINT '✓ Lessons seeded successfully';
END
ELSE
BEGIN
    PRINT '✓ Lessons already exist';
END

-- ============================================
-- 4. SEED ALPHABETS (Hiragana & Katakana)
-- ============================================
IF NOT EXISTS (SELECT 1 FROM Alphabets WHERE AlphabetId = 1)
BEGIN
    SET IDENTITY_INSERT Alphabets ON;
    
    INSERT INTO Alphabets (AlphabetId, Character, Type, Level, Meaning) VALUES
    -- Hiragana Vowels
    (1, 'あ', 'Hiragana', 'Basic', 'a'),
    (2, 'い', 'Hiragana', 'Basic', 'i'),
    (3, 'う', 'Hiragana', 'Basic', 'u'),
    (4, 'え', 'Hiragana', 'Basic', 'e'),
    (5, 'お', 'Hiragana', 'Basic', 'o'),
    
    -- Hiragana K-Row
    (6, 'か', 'Hiragana', 'Basic', 'ka'),
    (7, 'き', 'Hiragana', 'Basic', 'ki'),
    (8, 'く', 'Hiragana', 'Basic', 'ku'),
    (9, 'け', 'Hiragana', 'Basic', 'ke'),
    (10, 'こ', 'Hiragana', 'Basic', 'ko'),
    
    -- Hiragana S-Row
    (11, 'さ', 'Hiragana', 'Basic', 'sa'),
    (12, 'し', 'Hiragana', 'Basic', 'shi'),
    (13, 'す', 'Hiragana', 'Basic', 'su'),
    (14, 'せ', 'Hiragana', 'Basic', 'se'),
    (15, 'そ', 'Hiragana', 'Basic', 'so'),
    
    -- Katakana Vowels
    (16, 'ア', 'Katakana', 'Basic', 'a'),
    (17, 'イ', 'Katakana', 'Basic', 'i'),
    (18, 'ウ', 'Katakana', 'Basic', 'u'),
    (19, 'エ', 'Katakana', 'Basic', 'e'),
    (20, 'オ', 'Katakana', 'Basic', 'o');
    
    SET IDENTITY_INSERT Alphabets OFF;
    PRINT '✓ Alphabets seeded successfully';
END
ELSE
BEGIN
    PRINT '✓ Alphabets already exist';
END

-- ============================================
-- 5. SEED QUESTIONS FOR LESSON 1 (Hiragana Vowels)
-- ============================================
IF NOT EXISTS (SELECT 1 FROM Questions WHERE LessonId = 1)
BEGIN
    -- Question 1: Multiple Choice
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (1, 'MultipleChoice', 'What is the hiragana character "あ" in romaji?', 'a', 1, 10);
    
    DECLARE @Q1 INT = SCOPE_IDENTITY();
    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
    (@Q1, 'a', 1, 1), (@Q1, 'i', 0, 2), (@Q1, 'u', 0, 3), (@Q1, 'e', 0, 4);

    -- Question 2: Multiple Choice
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (1, 'MultipleChoice', 'Which character represents "i"?', 'い', 2, 10);
    
    DECLARE @Q2 INT = SCOPE_IDENTITY();
    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
    (@Q2, 'い', 1, 1), (@Q2, 'あ', 0, 2), (@Q2, 'う', 0, 3), (@Q2, 'え', 0, 4);

    -- Question 3: Multiple Choice
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (1, 'MultipleChoice', 'What sound does "う" make?', 'u', 3, 10);
    
    DECLARE @Q3 INT = SCOPE_IDENTITY();
    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
    (@Q3, 'u', 1, 1), (@Q3, 'o', 0, 2), (@Q3, 'e', 0, 3), (@Q3, 'a', 0, 4);

    -- Question 4: Translation
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (1, 'Translation', 'Type the romaji for: え', 'e', 4, 15);
    
    DECLARE @Q4 INT = SCOPE_IDENTITY();
    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
    (@Q4, 'e', 1, 1), (@Q4, 'a', 0, 2), (@Q4, 'i', 0, 3), (@Q4, 'o', 0, 4);

    -- Question 5: Multiple Choice
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (1, 'MultipleChoice', 'Select the correct character for "o"', 'お', 5, 10);
    
    DECLARE @Q5 INT = SCOPE_IDENTITY();
    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
    (@Q5, 'お', 1, 1), (@Q5, 'あ', 0, 2), (@Q5, 'い', 0, 3), (@Q5, 'う', 0, 4);

    PRINT '✓ Questions for Lesson 1 seeded successfully';
END

-- ============================================
-- 6. SEED QUESTIONS FOR LESSON 2 (K-Row)
-- ============================================
IF NOT EXISTS (SELECT 1 FROM Questions WHERE LessonId = 2)
BEGIN
    -- Question 1
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (2, 'MultipleChoice', 'What is "か" in romaji?', 'ka', 1, 10);
    
    DECLARE @Q1 INT = SCOPE_IDENTITY();
    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
    (@Q1, 'ka', 1, 1), (@Q1, 'ki', 0, 2), (@Q1, 'ku', 0, 3), (@Q1, 'ke', 0, 4);

    -- Question 2
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (2, 'MultipleChoice', 'Which character is "ki"?', 'き', 2, 10);
    
    DECLARE @Q2 INT = SCOPE_IDENTITY();
    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
    (@Q2, 'き', 1, 1), (@Q2, 'か', 0, 2), (@Q2, 'く', 0, 3), (@Q2, 'け', 0, 4);

    -- Question 3
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (2, 'FillInBlank', 'Complete: く = __', 'ku', 3, 10);
    
    DECLARE @Q3 INT = SCOPE_IDENTITY();
    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
    (@Q3, 'ku', 1, 1), (@Q3, 'ko', 0, 2), (@Q3, 'ke', 0, 3), (@Q3, 'ka', 0, 4);

    PRINT '✓ Questions for Lesson 2 seeded successfully';
END

-- ============================================
-- 7. SEED QUESTIONS FOR LESSON 6 (Greetings)
-- ============================================
IF NOT EXISTS (SELECT 1 FROM Questions WHERE LessonId = 6)
BEGIN
    -- Question 1
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (6, 'Translation', 'Translate to English: こんにちは', 'hello', 1, 15);
    
    DECLARE @Q1 INT = SCOPE_IDENTITY();
    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
    (@Q1, 'hello', 1, 1), (@Q1, 'goodbye', 0, 2), (@Q1, 'thank you', 0, 3), (@Q1, 'sorry', 0, 4);

    -- Question 2
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (6, 'MultipleChoice', 'What does "ありがとう" mean?', 'thank you', 2, 10);
    
    DECLARE @Q2 INT = SCOPE_IDENTITY();
    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
    (@Q2, 'thank you', 1, 1), (@Q2, 'sorry', 0, 2), (@Q2, 'excuse me', 0, 3), (@Q2, 'goodbye', 0, 4);

    -- Question 3
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (6, 'Translation', 'How do you say "goodbye" in Japanese?', 'さようなら', 3, 15);
    
    DECLARE @Q3 INT = SCOPE_IDENTITY();
    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
    (@Q3, 'さようなら', 1, 1), (@Q3, 'こんにちは', 0, 2), (@Q3, 'おはよう', 0, 3), (@Q3, 'ありがとう', 0, 4);

    -- Question 4
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (6, 'MultipleChoice', 'What is "good morning" in Japanese?', 'おはよう', 4, 10);
    
    DECLARE @Q4 INT = SCOPE_IDENTITY();
    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
    (@Q4, 'おはよう', 1, 1), (@Q4, 'こんにちは', 0, 2), (@Q4, 'こんばんは', 0, 3), (@Q4, 'さようなら', 0, 4);

    PRINT '✓ Questions for Lesson 6 seeded successfully';
END

-- ============================================
-- 8. SEED QUESTIONS FOR LESSON 7 (Introducing Yourself)
-- ============================================
IF NOT EXISTS (SELECT 1 FROM Questions WHERE LessonId = 7)
BEGIN
    -- Question 1
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (7, 'FillInBlank', 'Fill in: わたし___ アメリカ人です (I am American)', 'は', 1, 10);
    
    DECLARE @Q1 INT = SCOPE_IDENTITY();
    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
    (@Q1, 'は', 1, 1), (@Q1, 'が', 0, 2), (@Q1, 'を', 0, 3), (@Q1, 'に', 0, 4);

    -- Question 2
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (7, 'Translation', 'Translate: 私の名前はジョンです', 'my name is john', 2, 20);
    
    DECLARE @Q2 INT = SCOPE_IDENTITY();
    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
    (@Q2, 'my name is john', 1, 1), (@Q2, 'i am john', 0, 2), (@Q2, 'hello john', 0, 3), (@Q2, 'john is my friend', 0, 4);

    -- Question 3
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (7, 'MultipleChoice', 'How do you say "nice to meet you"?', 'はじめまして', 3, 15);
    
    DECLARE @Q3 INT = SCOPE_IDENTITY();
    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
    (@Q3, 'はじめまして', 1, 1), (@Q3, 'ありがとう', 0, 2), (@Q3, 'こんにちは', 0, 3), (@Q3, 'すみません', 0, 4);

    PRINT '✓ Questions for Lesson 7 seeded successfully';
END

-- ============================================
-- 9. UPDATE USER DEFAULT VALUES
-- ============================================
-- Update existing users with default values
UPDATE AspNetUsers 
SET 
    Hearts = 5,
    MaxHearts = 5,
    CurrentXP = 0,
    TotalXP = 0,
    [Level] = 1,
    Gems = 100,
    CurrentStreak = 0,
    LongestStreak = 0,
    StreakFreezeCount = 0
WHERE Hearts = 0 OR Hearts IS NULL;

PRINT '✓ User default values updated';

-- ============================================
-- VERIFICATION QUERIES
-- ============================================
PRINT '';
PRINT '========== DATA VERIFICATION ==========';

PRINT 'Total Units: ' + CAST((SELECT COUNT(*) FROM Units) AS VARCHAR);
PRINT 'Total Nodes: ' + CAST((SELECT COUNT(*) FROM Nodes) AS VARCHAR);
PRINT 'Total Lessons: ' + CAST((SELECT COUNT(*) FROM Lessons) AS VARCHAR);
PRINT 'Total Questions: ' + CAST((SELECT COUNT(*) FROM Questions) AS VARCHAR);
PRINT 'Total Question Options: ' + CAST((SELECT COUNT(*) FROM QuestionOptions) AS VARCHAR);
PRINT 'Total Alphabets: ' + CAST((SELECT COUNT(*) FROM Alphabets) AS VARCHAR);

PRINT '';
PRINT 'Questions per Lesson:';
SELECT 
    l.LessonId,
    l.Title,
    COUNT(q.QuestionId) as QuestionCount
FROM Lessons l
LEFT JOIN Questions q ON l.LessonId = q.LessonId
GROUP BY l.LessonId, l.Title
ORDER BY l.LessonId;

PRINT '';
PRINT '========== SEEDING COMPLETED! ==========';
