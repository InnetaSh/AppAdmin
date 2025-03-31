CREATE DATABASE TestProjectForWPF

USE TestProjectForWPF


---------------------------категории:----------------------------------------
CREATE TABLE Categories                  
(
   CategoryId    INT IDENTITY(1,1) PRIMARY KEY,
   CategoryName   NVARCHAR(100) NOT NULL
)

INSERT INTO Categories (CategoryName)
VALUES
('Фильмы');


SELECT * FROM Categories
DROP TABLE Categories
---------------------------тесты:----------------------------------------
CREATE TABLE Tests                  
(
   TestId    INT IDENTITY(1,1) PRIMARY KEY,
   CategoryId  INT NOT NULL, 
   FOREIGN KEY (CategoryId ) REFERENCES Categories(CategoryId) ON DELETE CASCADE,
   TestName   NVARCHAR(100) NOT NULL,
   TimeSec INT NOT NULL DEFAULT 300
)


INSERT INTO Tests (CategoryId, TestName)
VALUES
((SELECT CategoryId FROM Categories WHERE CategoryName  = 'Фильмы' ),
'Игра престолов');



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
   ImagePath NVARCHAR(255) 
)

INSERT INTO Questions (TestId, QuestionText, Weight)
VALUES
 ((SELECT TestId FROM Tests WHERE CategoryId = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Фильмы')),
    'Хаос – это ______. - Петир Бейлиш',
    20
);

ALTER TABLE Questions
ADD IsMultiAnswers BIT NOT NULL DEFAULT 0; 


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

INSERT INTO Answers (QuestionId, AnswerText, IsCorrect)
VALUES
(
    (SELECT QuestionId FROM Questions 
     WHERE TestId = (SELECT TestId FROM Tests 
                     WHERE CategoryId = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Фильмы'))),
    'возможность',  
    0               
);
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect)
VALUES
(
    (SELECT QuestionId FROM Questions 
     WHERE TestId = (SELECT TestId FROM Tests 
                     WHERE CategoryId = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Фильмы'))),
    'лестница',  
    1               
);
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect)
VALUES
(
    (SELECT QuestionId FROM Questions 
     WHERE TestId = (SELECT TestId FROM Tests 
                     WHERE CategoryId = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Фильмы'))),
    'шанс',  
    0               
);
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect)
VALUES
(
    (SELECT QuestionId FROM Questions 
     WHERE TestId = (SELECT TestId FROM Tests 
                     WHERE CategoryId = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Фильмы'))),
    'беспорядок',  
    0               
);


SELECT * FROM Answers
DROP TABLE Answers



----------------------------------админ:----------------------------------
CREATE TABLE Admins                 
(
   AdminId   INT IDENTITY(1,1) PRIMARY KEY,
   AdminName  NVARCHAR(100) NOT NULL,
   PasswordHash  NVARCHAR(100) NOT NULL, 
   Email NVARCHAR(100)  UNIQUE NOT NULL
)


SELECT * FROM Admins
DROP TABLE Admins

----------------------------------пользователи:----------------------------------
CREATE TABLE Users                 
(
   UserId   INT IDENTITY(1,1) PRIMARY KEY,
   Username  NVARCHAR(100) NOT NULL,
   PasswordHash  NVARCHAR(100) NOT NULL, 
   Email NVARCHAR(100)  UNIQUE NOT NULL,
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