  CREATE PROCEDURE [dbo].[SpGetALLBusinessCards]
    @Name NVARCHAR(50) = NULL,                  
    @Gender NVARCHAR(10) = NULL,                
    @DateofBirth DATE = NULL,                   
    @Email NVARCHAR(50) = NULL,                 
    @Phone NVARCHAR(15) = NULL,                 
    @SortColumn NVARCHAR(50) = 'Id',            
    @SortDirection INT = 2,                     
    @PageIndex INT = 1,                         
    @PageSize INT = 10                   
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [Id],
        [DateofBirth],
        [Email],
        [Gender],
        [Name],
        [Phone],
        [Photo],
        [Address_Country] AS Country,
        [Address_State] AS State,
        [Address_ZipCode] AS ZipCode,
        [Address_City] AS City,
        [Address_Street] AS Street
    FROM BusinessCard
    WHERE 
        (@Name IS NULL OR Name LIKE '%' + @Name + '%')               
        AND (@Gender IS NULL OR Gender = @Gender)                    
        AND (@DateofBirth IS NULL OR DateofBirth = @DateofBirth)     
        AND (@Email IS NULL OR Email LIKE '%' + @Email + '%')        
        AND (@Phone IS NULL OR Phone LIKE '%' + @Phone + '%')        
    ORDER BY 
        CASE WHEN @SortDirection = 1 AND @SortColumn = 'Name' THEN Name END ASC,
        CASE WHEN @SortDirection = 2 AND @SortColumn = 'Name' THEN Name END DESC,
        CASE WHEN @SortDirection = 1 AND @SortColumn = 'DateofBirth' THEN DateofBirth END ASC,
        CASE WHEN @SortDirection = 2 AND @SortColumn = 'DateofBirth' THEN DateofBirth END DESC,
        CASE WHEN @SortDirection = 1 AND @SortColumn = 'Gender' THEN Gender END ASC,
        CASE WHEN @SortDirection = 2 AND @SortColumn = 'Gender' THEN Gender END DESC,
        CASE WHEN @SortDirection = 1 AND @SortColumn = 'Id' THEN Id END ASC,
        CASE WHEN @SortDirection = 2 AND @SortColumn = 'Id' THEN Id END DESC
    OFFSET (@PageIndex - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;


	SELECT
		COUNT(Id) AS TotalRecord
    FROM BusinessCard
    WHERE 
        (@Name IS NULL OR Name LIKE '%' + @Name + '%')               
        AND (@Gender IS NULL OR Gender = @Gender)                    
        AND (@DateofBirth IS NULL OR DateofBirth = @DateofBirth)     
        AND (@Email IS NULL OR Email LIKE '%' + @Email + '%')        
        AND (@Phone IS NULL OR Phone LIKE '%' + @Phone + '%')        
END;