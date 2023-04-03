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
    SELECT *
    FROM Student s
    WHERE 
        (@FirstName IS NULL OR s.FirstName LIKE @FirstName + '%') AND 
        (@LastName IS NULL OR s.LastName LIKE @LastName + '%') AND
        (@BirthDate IS NULL OR s.BirthDate >= @BirthDate)
    ORDER BY 
        CASE WHEN @SortOrder = 'ASC' THEN
            CASE @SortColumn
                WHEN 'StudentId' THEN StudentId
                WHEN 'BirthDate' THEN BirthDate
                ELSE StudentId
            END
        END ASC,
        CASE WHEN @SortOrder = 'DESC' THEN
            CASE @SortColumn
                WHEN 'StudentId' THEN StudentId
                WHEN 'BirthDate' THEN BirthDate
                ELSE StudentId
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
        SELECT *
        FROM Student s
        WHERE 
            (@FirstName IS NULL OR s.FirstName LIKE @FirstName + '%') AND 
            (@LastName IS NULL OR s.LastName LIKE @LastName + '%') AND
            (@BirthDate IS NULL OR s.BirthDate >= @BirthDate)
        ORDER BY 
            CASE WHEN @SortOrder = 'ASC' THEN
                CASE @SortColumn
                    WHEN 'StudentId' THEN StudentId
                    WHEN 'BirthDate' THEN BirthDate
                    ELSE StudentId
                END
            END ASC,
            CASE WHEN @SortOrder = 'DESC' THEN
                CASE @SortColumn
                    WHEN 'StudentId' THEN StudentId
                    WHEN 'BirthDate' THEN BirthDate
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
        SELECT *
        FROM Student s
        WHERE 
            (@FirstName IS NULL OR s.FirstName LIKE @FirstName + '%') AND 
            (@LastName IS NULL OR s.LastName LIKE @LastName + '%') AND
            (@BirthDate IS NULL OR s.BirthDate >= @BirthDate)
        ORDER BY 
            CASE WHEN @SortOrder = 'ASC' THEN
                CASE @SortColumn
                    WHEN 'StudentId' THEN StudentId
                    WHEN 'BirthDate' THEN BirthDate
                    ELSE StudentId
                END
            END ASC,
            CASE WHEN @SortOrder = 'DESC' THEN
                CASE @SortColumn
                    WHEN 'StudentId' THEN StudentId
                    WHEN 'BirthDate' THEN BirthDate
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
