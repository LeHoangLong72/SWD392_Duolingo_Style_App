-- ============================================
-- STEP 1: Replace @UserId with your actual UserId
-- Run GetUserId.sql first to get your UserId
-- ============================================

DECLARE @UserId NVARCHAR(450) = 'YOUR_USER_ID_HERE'; -- ⚠️ REPLACE THIS!

-- Example: DECLARE @UserId NVARCHAR(450) = 'abc123-def456-ghi789';

-- Verify UserId exists
IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE Id = @UserId)
BEGIN
    PRINT '❌ ERROR: UserId not found!';
    PRINT 'Please run GetUserId.sql to get your real UserId';
    RETURN;
END

PRINT '✓ UserId verified: ' + @UserId;
PRINT 'Starting data insertion...';

-- Insert Units
INSERT INTO Units (UnitNumber, Title) VALUES
(1, 'Unit 1: Hiragana Basics'),
(2, 'Unit 2: Katakana Basics'),
(3, 'Unit 3: Basic Greetings'),
(4, 'Unit 4: Numbers & Counting'),
(5, 'Unit 5: Daily Conversation');

-- Insert Nodes (with real UserId)
INSERT INTO Nodes (UnitId, UserId, NodeType, Position) VALUES
(1, @UserId, 'lesson', 1),
(1, @UserId, 'lesson', 2),
(1, @UserId, 'practice', 3),
(1, @UserId, 'lesson', 4),
(1, @UserId, 'test', 5),
(2, @UserId, 'lesson', 1),
(2, @UserId, 'lesson', 2),
(2, @UserId, 'practice', 3),
(3, @UserId, 'lesson', 1),
(3, @UserId, 'lesson', 2);

-- Insert Lessons
INSERT INTO Lessons (NodeId, Title, BaseXP) VALUES
(1, 'Hiragana: Vowels (あいうえお)', 50),
(2, 'Hiragana: K-Row (かきくけこ)', 50),
(4, 'Hiragana: S-Row (さしすせそ)', 50),
(6, 'Katakana: Vowels (アイウエオ)', 50),
(7, 'Katakana: K-Row (カキクケコ)', 50),
(9, 'Basic Greetings', 60),
(10, 'Introducing Yourself', 60);

-- Insert Alphabets
INSERT INTO Alphabets (Character, Type, Level, Meaning) VALUES
('あ', 'Hiragana', 'Basic', 'a'),
('い', 'Hiragana', 'Basic', 'i'),
('う', 'Hiragana', 'Basic', 'u'),
('え', 'Hiragana', 'Basic', 'e'),
('お', 'Hiragana', 'Basic', 'o'),
('か', 'Hiragana', 'Basic', 'ka'),
('き', 'Hiragana', 'Basic', 'ki'),
('く', 'Hiragana', 'Basic', 'ku'),
('け', 'Hiragana', 'Basic', 'ke'),
('こ', 'Hiragana', 'Basic', 'ko'),
('さ', 'Hiragana', 'Basic', 'sa'),
('し', 'Hiragana', 'Basic', 'shi'),
('す', 'Hiragana', 'Basic', 'su'),
('せ', 'Hiragana', 'Basic', 'se'),
('そ', 'Hiragana', 'Basic', 'so'),
('ア', 'Katakana', 'Basic', 'a'),
('イ', 'Katakana', 'Basic', 'i'),
('ウ', 'Katakana', 'Basic', 'u'),
('エ', 'Katakana', 'Basic', 'e'),
('オ', 'Katakana', 'Basic', 'o');

-- Insert Questions for Lesson 1
INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward) VALUES
(1, 'MultipleChoice', 'What is the hiragana character "あ" in romaji?', 'a', 1, 10),
(1, 'MultipleChoice', 'Which character represents "i"?', 'い', 2, 10),
(1, 'MultipleChoice', 'What sound does "う" make?', 'u', 3, 10),
(1, 'Translation', 'Type the romaji for: え', 'e', 4, 15),
(1, 'MultipleChoice', 'Select the correct character for "o"', 'お', 5, 10);

-- Insert Question Options
INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
(1, 'a', 1, 1), (1, 'i', 0, 2), (1, 'u', 0, 3), (1, 'e', 0, 4),
(2, 'い', 1, 1), (2, 'あ', 0, 2), (2, 'う', 0, 3), (2, 'え', 0, 4),
(3, 'u', 1, 1), (3, 'o', 0, 2), (3, 'e', 0, 3), (3, 'a', 0, 4),
(4, 'e', 1, 1), (4, 'a', 0, 2), (4, 'i', 0, 3), (4, 'o', 0, 4),
(5, 'お', 1, 1), (5, 'あ', 0, 2), (5, 'い', 0, 3), (5, 'う', 0, 4);

-- Insert Questions for Lesson 2
INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward) VALUES
(2, 'MultipleChoice', 'What is "か" in romaji?', 'ka', 1, 10),
(2, 'MultipleChoice', 'Which character is "ki"?', 'き', 2, 10),
(2, 'FillInBlank', 'Complete: く = __', 'ku', 3, 10);

INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
(6, 'ka', 1, 1), (6, 'ki', 0, 2), (6, 'ku', 0, 3), (6, 'ke', 0, 4),
(7, 'き', 1, 1), (7, 'か', 0, 2), (7, 'く', 0, 3), (7, 'け', 0, 4),
(8, 'ku', 1, 1), (8, 'ko', 0, 2), (8, 'ke', 0, 3), (8, 'ka', 0, 4);

-- Insert Questions for Lesson 6
INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward) VALUES
(6, 'Translation', 'Translate to English: こんにちは', 'hello', 1, 15),
(6, 'MultipleChoice', 'What does "ありがとう" mean?', 'thank you', 2, 10),
(6, 'Translation', 'How do you say "goodbye" in Japanese?', 'さようなら', 3, 15),
(6, 'MultipleChoice', 'What is "good morning" in Japanese?', 'おはよう', 4, 10);

INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
(9, 'hello', 1, 1), (9, 'goodbye', 0, 2), (9, 'thank you', 0, 3), (9, 'sorry', 0, 4),
(10, 'thank you', 1, 1), (10, 'sorry', 0, 2), (10, 'excuse me', 0, 3), (10, 'goodbye', 0, 4),
(11, 'さようなら', 1, 1), (11, 'こんにちは', 0, 2), (11, 'おはよう', 0, 3), (11, 'ありがとう', 0, 4),
(12, 'おはよう', 1, 1), (12, 'こんにちは', 0, 2), (12, 'こんばんは', 0, 3), (12, 'さようなら', 0, 4);

-- Insert Questions for Lesson 7
INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward) VALUES
(7, 'FillInBlank', 'Fill in: わたし___ アメリカ人です', 'は', 1, 10),
(7, 'Translation', 'Translate: 私の名前はジョンです', 'my name is john', 2, 20),
(7, 'MultipleChoice', 'How do you say "nice to meet you"?', 'はじめまして', 3, 15);

INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex) VALUES
(13, 'は', 1, 1), (13, 'が', 0, 2), (13, 'を', 0, 3), (13, 'に', 0, 4),
(14, 'my name is john', 1, 1), (14, 'i am john', 0, 2), (14, 'hello john', 0, 3), (14, 'john is my friend', 0, 4),
(15, 'はじめまして', 1, 1), (15, 'ありがとう', 0, 2), (15, 'こんにちは', 0, 3), (15, 'すみません', 0, 4);

-- Insert Shop Items
INSERT INTO Items (Name, Description, Price, Category, ImageUrl, IsActive) VALUES
('Streak Freeze', 'Protect your streak for one day', 200, 'PowerUp', '/images/streak-freeze.png', 1),
('Heart Refill', 'Instantly refill all hearts', 350, 'PowerUp', '/images/heart-refill.png', 1),
('Double XP Boost', 'Earn 2x XP for 15 minutes', 150, 'PowerUp', '/images/double-xp.png', 1),
('Timer Boost', 'Extra time on challenges', 100, 'PowerUp', '/images/timer-boost.png', 1),
('Golden Owl Avatar', 'Premium avatar', 500, 'Cosmetic', '/images/golden-owl.png', 1);

-- Update user with default values
UPDATE AspNetUsers 
SET Hearts = 5, MaxHearts = 5, CurrentXP = 0, TotalXP = 0, [Level] = 1, 
    Gems = 100, CurrentStreak = 0, LongestStreak = 0, StreakFreezeCount = 0
WHERE Id = @UserId;

PRINT '';
PRINT '✓✓✓ Data inserted successfully! ✓✓✓';
PRINT 'UserId used: ' + @UserId;
