-- Sample Data for Lesson Content Testing

-- Assuming you have at least one Lesson with LessonId = 1
-- If not, create one first

-- Check if lesson exists
IF NOT EXISTS (SELECT 1 FROM Lessons WHERE LessonId = 1)
BEGIN
    PRINT 'No lesson found. Please create a Lesson first!'
    -- You can uncomment below to create test data
    -- INSERT INTO Units (UnitNumber, Title) VALUES (1, 'Japanese Basics');
    -- INSERT INTO Nodes (UnitId, UserId, NodeType, Position) VALUES (1, 1, 'lesson', 1);
    -- INSERT INTO Lessons (NodeId, Title, BaseXP) VALUES (1, 'Hiragana Basics', 50);
END
ELSE
BEGIN
    -- Question 1: Multiple Choice - Hiragana Recognition
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (1, 'MultipleChoice', 'What is the hiragana character "あ" in romaji?', 'a', 1, 10);

    DECLARE @QuestionId1 INT = SCOPE_IDENTITY();

    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex)
    VALUES 
        (@QuestionId1, 'a', 1, 1),
        (@QuestionId1, 'i', 0, 2),
        (@QuestionId1, 'u', 0, 3),
        (@QuestionId1, 'e', 0, 4);

    -- Question 2: Translation
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (1, 'Translation', 'Translate to English: こんにちは', 'hello', 2, 15);

    DECLARE @QuestionId2 INT = SCOPE_IDENTITY();

    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex)
    VALUES 
        (@QuestionId2, 'hello', 1, 1),
        (@QuestionId2, 'goodbye', 0, 2),
        (@QuestionId2, 'thank you', 0, 3),
        (@QuestionId2, 'sorry', 0, 4);

    -- Question 3: Fill in the Blank
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (1, 'FillInBlank', 'Fill in: わたし___ アメリカ人です (I am American)', 'は', 3, 10);

    DECLARE @QuestionId3 INT = SCOPE_IDENTITY();

    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex)
    VALUES 
        (@QuestionId3, 'は', 1, 1),
        (@QuestionId3, 'が', 0, 2),
        (@QuestionId3, 'を', 0, 3),
        (@QuestionId3, 'に', 0, 4);

    -- Question 4: Multiple Choice - Vocabulary
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (1, 'MultipleChoice', 'What does "ありがとう" mean?', 'thank you', 4, 10);

    DECLARE @QuestionId4 INT = SCOPE_IDENTITY();

    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex)
    VALUES 
        (@QuestionId4, 'thank you', 1, 1),
        (@QuestionId4, 'sorry', 0, 2),
        (@QuestionId4, 'excuse me', 0, 3),
        (@QuestionId4, 'goodbye', 0, 4);

    -- Question 5: Matching
    INSERT INTO Questions (LessonId, QuestionType, QuestionText, CorrectAnswer, OrderIndex, XPReward)
    VALUES (1, 'Matching', 'Match the hiragana: か', 'ka', 5, 10);

    DECLARE @QuestionId5 INT = SCOPE_IDENTITY();

    INSERT INTO QuestionOptions (QuestionId, OptionText, IsCorrect, OrderIndex)
    VALUES 
        (@QuestionId5, 'ka', 1, 1),
        (@QuestionId5, 'ki', 0, 2),
        (@QuestionId5, 'ku', 0, 3),
        (@QuestionId5, 'ke', 0, 4);

    PRINT 'Sample questions created successfully for LessonId = 1!';
END

-- Verify data
SELECT 
    q.QuestionId,
    q.QuestionType,
    q.QuestionText,
    q.CorrectAnswer,
    COUNT(qo.OptionId) as OptionCount
FROM Questions q
LEFT JOIN QuestionOptions qo ON q.QuestionId = qo.QuestionId
WHERE q.LessonId = 1
GROUP BY q.QuestionId, q.QuestionType, q.QuestionText, q.CorrectAnswer
ORDER BY q.OrderIndex;
