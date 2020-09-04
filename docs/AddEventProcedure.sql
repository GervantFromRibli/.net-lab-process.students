USE TicketManagement;
GO
CREATE PROCEDURE AddEvent
    @Id INT,
    @Name NVARCHAR(120),
    @Descr NVARCHAR(400),
    @LayoutId INT
AS
INSERT INTO Event(Id, [Name], [Description], LayoutId) 
VALUES(@Id, @Name, @Descr, @LayoutId)