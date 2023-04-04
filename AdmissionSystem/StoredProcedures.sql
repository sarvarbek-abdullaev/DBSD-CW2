CREATE OR ALTER PROCEDURE FilterStudents
    @FirstName varchar(50) = NULL,
    @LastName varchar(50) = NULL,
    @BirthDate date = NULL,
    @SortColumn varchar(50) = 'StudentId',
    @SortOrder varchar(4) = 'ASC',
    @PageSize int = 10,
    @PageNumber int = 1
AS
BEGIN
    SELECT  
        s.StudentId,
        s.FirstName, 
        s.LastName, 
        s.Email, 
        s.Phone, 
        s.BirthDate, 
        s.Level, 
        s.HasDebt, 
        c.Name as ClassName,
        t.FirstName as TeacherFirstName, 
        t.LastName as TeacherLastName
    FROM Student s JOIN Class c on s.ClassId = c.ClassId JOIN Teacher t on t.TeacherId = c.TeacherId
    WHERE 
        (@FirstName IS NULL OR s.FirstName LIKE @FirstName + '%') AND 
        (@LastName IS NULL OR s.LastName LIKE @LastName + '%') AND
        (@BirthDate IS NULL OR s.BirthDate >= @BirthDate)
    ORDER BY 
        CASE WHEN @SortOrder = 'ASC' THEN
            CASE @SortColumn
                WHEN 'StudentId' THEN s.StudentId
                WHEN 'BirthDate' THEN s.BirthDate
                ELSE s.StudentId
            END
        END ASC,
        CASE WHEN @SortOrder = 'DESC' THEN
            CASE @SortColumn
                WHEN 'StudentId' THEN s.StudentId
                WHEN 'BirthDate' THEN s.BirthDate
                ELSE s.StudentId
            END
        END DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY
END

GO

CREATE OR ALTER PROCEDURE FilteredStudentsXml
    @FirstName varchar(50) = NULL,
    @LastName varchar(50) = NULL,
    @BirthDate date = NULL,
    @SortColumn varchar(50) = 'StudentId',
    @SortOrder varchar(4) = 'ASC',
    @PageSize int = 10,
    @PageNumber int = 1
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @xml XML;

    WITH StudentData AS (
        SELECT  
            s.StudentId, 
            s.FirstName, 
            s.LastName, 
            s.Email, 
            s.Phone, 
            s.BirthDate, 
            s.Level, 
            s.HasDebt, 
            c.Name as ClassName,
            t.FirstName as TeacherFirstName, 
            t.LastName as TeacherLastName
        FROM Student s JOIN Class c on s.ClassId = c.ClassId JOIN Teacher t on t.TeacherId = c.TeacherId
        WHERE 
            (@FirstName IS NULL OR s.FirstName LIKE @FirstName + '%') AND 
            (@LastName IS NULL OR s.LastName LIKE @LastName + '%') AND
            (@BirthDate IS NULL OR s.BirthDate >= @BirthDate)
        ORDER BY 
            CASE WHEN @SortOrder = 'ASC' THEN
                CASE @SortColumn
                    WHEN 'StudentId' THEN s.StudentId
                    WHEN 'BirthDate' THEN s.BirthDate
                    ELSE StudentId
                END
            END ASC,
            CASE WHEN @SortOrder = 'DESC' THEN
                CASE @SortColumn
                    WHEN 'StudentId' THEN s.StudentId
                    WHEN 'BirthDate' THEN s.BirthDate
                    ELSE StudentId
                END
            END DESC
        OFFSET (@PageNumber - 1) * @PageSize ROWS
        FETCH NEXT @PageSize ROWS ONLY
    )
    SELECT @xml = (SELECT * FROM StudentData FOR XML PATH('Student'), ROOT('ArrayOfStudent'))

    SELECT @xml;
END

GO

CREATE OR ALTER PROCEDURE FilteredStudentsJson
    @FirstName varchar(50) = NULL,
    @LastName varchar(50) = NULL,
    @BirthDate date = NULL,
    @SortColumn varchar(50) = 'StudentId',
    @SortOrder varchar(4) = 'ASC',
    @PageSize int = 10,
    @PageNumber int = 1
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @json varchar(max) ;

    WITH StudentData AS (
        SELECT  
            s.StudentId,
            s.FirstName, 
            s.LastName, 
            s.Email, 
            s.Phone, 
            s.BirthDate, 
            s.Level, 
            s.HasDebt, 
            c.Name as ClassName,
            t.FirstName as TeacherFirstName, 
            t.LastName as TeacherLastName
        FROM Student s JOIN Class c on s.ClassId = c.ClassId JOIN Teacher t on t.TeacherId = c.TeacherId
        WHERE 
            (@FirstName IS NULL OR s.FirstName LIKE @FirstName + '%') AND 
            (@LastName IS NULL OR s.LastName LIKE @LastName + '%') AND
            (@BirthDate IS NULL OR s.BirthDate >= @BirthDate)
        ORDER BY 
            CASE WHEN @SortOrder = 'ASC' THEN
                CASE @SortColumn
                    WHEN 'StudentId' THEN s.StudentId
                    WHEN 'BirthDate' THEN s.BirthDate
                    ELSE StudentId
                END
            END ASC,
            CASE WHEN @SortOrder = 'DESC' THEN
                CASE @SortColumn
                    WHEN 'StudentId' THEN s.StudentId
                    WHEN 'BirthDate' THEN s.BirthDate
                    ELSE StudentId
                END
            END DESC
        OFFSET (@PageNumber - 1) * @PageSize ROWS
        FETCH NEXT @PageSize ROWS ONLY
    )
    SELECT @json = (SELECT * FROM StudentData FOR JSON PATH)

    SELECT @json;
END

GO

CREATE OR ALTER PROCEDURE ImportStudentsJson @jsonString NVARCHAR(MAX)
AS
BEGIN
  INSERT INTO Student (FirstName, LastName, BirthDate, Phone, Email, HasDebt, Level, ClassId)
  SELECT 
    JSON_VALUE(j.Value, '$.FirstName') AS FirstName,
    JSON_VALUE(j.Value, '$.LastName') AS LastName,
    JSON_VALUE(j.Value, '$.BirthDate') AS BirthDate,
    JSON_VALUE(j.Value, '$.Phone') AS Phone,
    JSON_VALUE(j.Value, '$.Email') AS Email,
    JSON_VALUE(j.Value, '$.HasDebt') AS HasDebt,
    JSON_VALUE(j.Value, '$.Level') AS Level,
    JSON_VALUE(j.Value, '$.ClassId') AS ClassId
  FROM OPENJSON(@jsonString) j
END

GO

CREATE OR ALTER PROCEDURE ImportStudentsXml @xmlString XML
AS
BEGIN
    INSERT INTO Student (FirstName, LastName, BirthDate, Phone, Email, HasDebt, Level, ClassId)
    SELECT
        c.value('(FirstName)[1]', 'nvarchar(50)') AS FirstName,
        c.value('(LastName)[1]', 'nvarchar(50)') AS LastName,
        c.value('(BirthDate)[1]', 'date') AS BirthDate,
        c.value('(Phone)[1]', 'nvarchar(20)') AS Phone,
        c.value('(Email)[1]', 'nvarchar(50)') AS Email,
        c.value('(HasDebt)[1]', 'bit') AS HasDebt,
        c.value('(Level)[1]', 'nvarchar(10)') AS Level,
        c.value('(ClassId)[1]', 'int') AS ClassId
    FROM @xmlString.nodes('/ArrayOfStudent/Student') AS t(c)
END

GO

CREATE OR ALTER PROCEDURE ImportStudentsCsv 
    @csvString NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL NVARCHAR(MAX)

    SET @SQL = 'CREATE TABLE #TempTable ('
              + 'RowID INT IDENTITY(1,1) PRIMARY KEY, '
              + 'CSVData NVARCHAR(MAX)'
              + ')'
    EXEC (@SQL)

    SET @SQL = 'INSERT INTO #TempTable (CSVData) VALUES ' + @csvString
    EXEC (@SQL)

    SET @SQL = 'BULK INSERT Student ' + ' 
                FROM ''' + 'SELECT CSVData FROM #TempTable' + ''' 
                WITH 
                (
                    FIRSTROW = 1, 
                    FIELDTERMINATOR = '','', 
                    ROWTERMINATOR = ''\n'', 
                    TABLOCK
                )'

    EXEC (@SQL)

    SET @SQL = 'DROP TABLE #TempTable'
    EXEC (@SQL)
END

GO

create type StudentsImport as table (
    [FirstName] NVARCHAR(40),
    [LastName] NVARCHAR(20),
    [BirthDate] DATETIME,
    [Phone] NVARCHAR(24),
    [Email] NVARCHAR(60),
    [HasDebt] BIT,
    [Level] INT,
    [ClassId] INT
)

GO

create or alter procedure ImportBulkStudents (@Students StudentsImport readonly) as
begin
  set nocount on

  insert into Student(
        [FirstName], 
        [LastName], 
        [BirthDate], 
        [Phone],
        [Email],
        [HasDebt],
        [Level],
        [ClassId]
    )
  select    [FirstName], 
            [LastName], 
            [BirthDate], 
            [Phone],
            [Email],
            [HasDebt],
            [Level],
            [ClassId] 
  from @Students
end

