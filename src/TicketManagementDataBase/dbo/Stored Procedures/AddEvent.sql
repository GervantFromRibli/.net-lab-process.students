CREATE PROCEDURE AddEvent
    @Id INT,
    @Name NVARCHAR(120),
    @Descr NVARCHAR(400),
    @LayoutId INT,
	@StartDate DATE,
	@EndDate DATE
AS
INSERT INTO Event(Id, [Name], [Description], LayoutId, StartDate, EndDate) 
VALUES(@Id, @Name, @Descr, @LayoutId, @StartDate, @EndDate)