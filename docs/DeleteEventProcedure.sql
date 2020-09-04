USE TicketManagement;
GO
CREATE PROCEDURE DeleteEvent
    @Id INT
AS
DELETE FROM Event WHERE Id = @Id