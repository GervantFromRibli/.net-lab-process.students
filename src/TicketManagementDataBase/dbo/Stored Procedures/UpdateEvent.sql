CREATE PROCEDURE UpdateEvent
    @Id INT,
    @Name NVARCHAR(120),
    @Descr NVARCHAR(400),
    @LayoutId INT,
	@StartDate DATE,
	@EndDate DATE
AS
UPDATE Event SET [Name] = @Name, [Description] = @Descr, LayoutId = @LayoutId, StartDate = @StartDate, EndDate = @EndDate 
	WHERE Id = @Id 