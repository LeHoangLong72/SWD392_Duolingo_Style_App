# Database Seeding Guide

## ⚠️ QUAN TRỌNG: UserId và Foreign Keys

Trong ASP.NET Identity, `AspNetUsers.Id` là kiểu `NVARCHAR(450)` (GUID string), **KHÔNG PHẢI** `INT`.

Các bảng có foreign key tới `AspNetUsers` cần dùng `UserId` thật từ database.

## 📋 Thứ tự chạy SQL Scripts (CẬP NHẬT)

### **OPTION 1: Dùng UserId thật (KHUYẾN NGHỊ)** ⭐

**Bước 1: Tạo user trước**
```
POST /api/account/register
Body: {
  "username": "testuser",
  "email": "test@example.com",
  "password": "Test@12345678"
}
```

**Bước 2: Lấy UserId**
```sql
-- Chạy file: GetUserId.sql
SELECT Id, UserName, Email FROM AspNetUsers;

-- Copy Id (ví dụ: "abc123-def456-ghi789...")
```

**Bước 3: Insert data với UserId**
```sql
-- Mở file: InsertSimpleWithUserId.sql
-- Sửa dòng 7:
DECLARE @UserId NVARCHAR(450) = 'YOUR_ACTUAL_USER_ID_HERE';

-- Execute file
```

### **OPTION 2: Dùng script tự động (Đã fix models)**

Chạy các file SQL theo thứ tự sau để setup database hoàn chỉnh:

### 1️⃣ **CreateLessonContentTables.sql**
Tạo các bảng mới cho Lesson Content Feature
- Questions
- QuestionOptions  
- UserAnswers
- LessonAttempts
- Thêm columns vào AspNetUsers (XP, Hearts, Level, Gems, Streak...)

**Cách chạy:**
```sql
-- Mở file trong SQL Server Management Studio hoặc Azure Data Studio
-- Chọn database đúng
-- Execute (F5)
```

### 2️⃣ **SeedAllData.sql** ⭐ QUAN TRỌNG
Seed toàn bộ data cần thiết:
- Units (5 units)
- Nodes (10 nodes)
- Lessons (7 lessons)
- Alphabets (20 characters)
- Questions (15+ questions)
- QuestionOptions (60+ options)
- Update user default values

**Data được tạo:**
- ✅ Unit 1: Hiragana Basics (5 nodes)
- ✅ Unit 2: Katakana Basics (3 nodes)
- ✅ Unit 3: Basic Greetings (2 nodes)
- ✅ Unit 4: Numbers & Counting
- ✅ Unit 5: Daily Conversation

- ✅ Lesson 1: Hiragana Vowels (5 questions)
- ✅ Lesson 2: K-Row (3 questions)
- ✅ Lesson 6: Basic Greetings (4 questions)
- ✅ Lesson 7: Introducing Yourself (3 questions)

### 3️⃣ **SeedShopItems.sql**
Seed items cho Shop system
- 15 items across different categories
- PowerUps, Cosmetics, Bundles, etc.

### 4️⃣ **SeedLessonContent.sql** (Legacy - Optional)
File cũ chỉ seed Lesson 1. Đã được thay thế bởi SeedAllData.sql
Không cần chạy nếu đã chạy SeedAllData.sql

---

## 🚀 Quick Start (Chạy tất cả)

```sql
-- Step 1: Create tables
:r CreateLessonContentTables.sql

-- Step 2: Seed all learning data
:r SeedAllData.sql

-- Step 3: Seed shop items
:r SeedShopItems.sql
```

---

## ✅ Verification

Sau khi chạy xong, verify bằng queries:

```sql
-- Check all tables
SELECT 'Units' as TableName, COUNT(*) as Count FROM Units
UNION ALL
SELECT 'Nodes', COUNT(*) FROM Nodes
UNION ALL
SELECT 'Lessons', COUNT(*) FROM Lessons
UNION ALL
SELECT 'Questions', COUNT(*) FROM Questions
UNION ALL
SELECT 'QuestionOptions', COUNT(*) FROM QuestionOptions
UNION ALL
SELECT 'Alphabets', COUNT(*) FROM Alphabets
UNION ALL
SELECT 'Items', COUNT(*) FROM Items WHERE IsActive = 1;

-- Check questions per lesson
SELECT 
    l.LessonId,
    l.Title,
    COUNT(q.QuestionId) as QuestionCount
FROM Lessons l
LEFT JOIN Questions q ON l.LessonId = q.LessonId
GROUP BY l.LessonId, l.Title
ORDER BY l.LessonId;

-- Check user data
SELECT 
    UserName,
    Hearts,
    MaxHearts,
    Level,
    CurrentXP,
    Gems,
    CurrentStreak
FROM AspNetUsers;
```

---

## 📊 Expected Results

Sau khi seed xong, bạn sẽ có:

| Table | Count |
|-------|-------|
| Units | 5 |
| Nodes | 10 |
| Lessons | 7 |
| Questions | 15+ |
| QuestionOptions | 60+ |
| Alphabets | 20 |
| Items | 15 |

---

## 🧹 Clear Data (Nếu cần reset)

```sql
-- ⚠️ CẢNH BÁO: Xóa toàn bộ data!
DELETE FROM UserAnswers;
DELETE FROM LessonAttempts;
DELETE FROM QuestionOptions;
DELETE FROM Questions;
DELETE FROM UserLessonProgresses;
DELETE FROM Lessons;
DELETE FROM Nodes;
DELETE FROM Units;
DELETE FROM Alphabets;
DELETE FROM UserItems;
DELETE FROM Transactions;
DELETE FROM Items;

-- Reset identity seeds
DBCC CHECKIDENT ('Questions', RESEED, 0);
DBCC CHECKIDENT ('QuestionOptions', RESEED, 0);
DBCC CHECKIDENT ('Lessons', RESEED, 0);
DBCC CHECKIDENT ('Nodes', RESEED, 0);
DBCC CHECKIDENT ('Units', RESEED, 0);
DBCC CHECKIDENT ('Alphabets', RESEED, 0);
DBCC CHECKIDENT ('Items', RESEED, 0);
```

---

## 🔧 Troubleshooting

### Lỗi: "Foreign key constraint"
**Giải pháp:** Chạy scripts theo đúng thứ tự (1 → 2 → 3)

### Lỗi: "Violation of PRIMARY KEY constraint"
**Giải pháp:** Data đã tồn tại. Dùng script Clear Data ở trên hoặc skip (script có check EXISTS)

### Lỗi: "Invalid column name"
**Giải pháp:** Chưa chạy CreateLessonContentTables.sql. Chạy script #1 trước.

---

## 📝 Notes

- Tất cả scripts đều **idempotent** (chạy nhiều lần không gây lỗi)
- Scripts tự động check EXISTS trước khi insert
- UserId trong Nodes đang hardcode = 1 (thay đổi nếu cần)
- Default values cho Users: 5 Hearts, 100 Gems, Level 1

---

## 🎯 Testing After Seeding

Sau khi seed xong, test các endpoints:

1. **GET** `/api/lesson-content/1` - Xem nội dung Lesson 1
2. **POST** `/api/lesson-content/start/1` - Bắt đầu lesson
3. **POST** `/api/lesson-content/submit-answer/{attemptId}` - Trả lời câu hỏi
4. **POST** `/api/lesson-content/complete/{attemptId}` - Hoàn thành lesson

Xem chi tiết trong `LessonContent.postman_collection.json`
