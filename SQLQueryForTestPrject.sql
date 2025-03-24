CREATE DATABASE TestProjectForWPF

USE TestProjectForWPF



---------------------------тесты:----------------------------------------
CREATE TABLE Tests                  
(
   TestId    INT IDENTITY(1,1) PRIMARY KEY,
   TestName   NVARCHAR(100) NOT NULL,
   TestDescription    NVARCHAR(100) NOT NULL
)


SELECT * FROM Tests
DROP TABLE Tests
---------------------------вопросы:----------------------------------------
CREATE TABLE Questions                 
(
   QuestionId   INT IDENTITY(1,1) PRIMARY KEY,
   TestId  INT NOT NULL, 
   FOREIGN KEY (TestId ) REFERENCES Tests(TestId) ON DELETE CASCADE,
   QuestionText  NVARCHAR(255) NOT NULL,
   Weight  INT NOT NULL,
   ImagePath NVARCHAR(255) NOT NULL
)


SELECT * FROM Questions
DROP TABLE Questions

-------------------------------варианты ответов:-------------------------------------
CREATE TABLE Answers                
(
   AnswerId  INT IDENTITY(1,1) PRIMARY KEY,
   QuestionId INT NOT NULL, 
   FOREIGN KEY (QuestionId) REFERENCES Questions (QuestionId ) ON DELETE CASCADE,
   AnswerText NVARCHAR(255) NOT NULL,
   IsCorrect BIT NOT NULL 
)


SELECT * FROM Answers
DROP TABLE Answers




----------------------------------пользователи:----------------------------------
CREATE TABLE Users                 
(
   UserId   INT IDENTITY(1,1) PRIMARY KEY,
   Username  NVARCHAR(100) NOT NULL,
   PasswordHash  NVARCHAR(100) NOT NULL, 
   MaxAttempts INT
)


SELECT * FROM Users
DROP TABLE Users

-------------------------попытки прохождения тестов:-------------------------------------------
CREATE TABLE TestAttempts                  
(
   AttemptId    INT IDENTITY(1,1) PRIMARY KEY,
   UserId  INT NOT NULL, 
   FOREIGN KEY (UserId ) REFERENCES Users (UserId) ON DELETE CASCADE,
   TestId   INT NOT NULL, 
   FOREIGN KEY (TestId) REFERENCES Tests (TestId) ON DELETE CASCADE,
   StartTime  DATETIME NOT NULL,
   EndTime   DATETIME NOT NULL, 
   Score  INT
)


SELECT * FROM TestAttempts
DROP TABLE TestAttempts

------------------------------ответы пользователей:--------------------------------------
CREATE TABLE UserAnswers                   
(
   UserAnswerId     INT IDENTITY(1,1) PRIMARY KEY,
   AttemptId   INT NOT NULL, 
   FOREIGN KEY (AttemptId) REFERENCES TestAttempts (AttemptId) ON DELETE CASCADE,
   QuestionId    INT NOT NULL, 
   FOREIGN KEY (QuestionId) REFERENCES Questions (QuestionId) ON DELETE CASCADE,
   AnswerId     INT NOT NULL, 
   FOREIGN KEY (AnswerId) REFERENCES Answers (AnswerId) ON DELETE CASCADE,
  
)


SELECT * FROM UserAnswers 
DROP TABLE UserAnswers 