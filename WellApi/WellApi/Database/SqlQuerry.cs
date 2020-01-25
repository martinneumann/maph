using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WellApi.Models;

namespace WellApi
{
    public class SqlQuerry
    {
        // Select

        public static SqlCommand SelectSmallWells()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Name, Status, Latitude, Longitude ");
            sb.Append("FROM [well].[dbo].[Well];");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            return sqlCommand;
        }
        public static SqlCommand SelectWell(int? wellId)
        {
            if (wellId == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT w.Id, w.Name, w.Status, w.Latitude, w.Longitude, w.Organisation, w.OpeningDate, w.Price, t.Id, t.Name, t.Particularity ");
            sb.Append("FROM [well].[dbo].[Well] w ");
            sb.Append("JOIN [well].[dbo].[WellType] t ");
            sb.Append("ON w.WellTypeId = t.Id ");
            sb.Append($"WHERE w.Id = @WellId;");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@WellId", wellId);
            return sqlCommand;
        }
        public static SqlCommand SelectRepairHelp(int? partId)
        {
            if (partId == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Description, Title, Number ");
            sb.Append("FROM [well].[dbo].[RepairHelp] ");
            sb.Append($"WHERE PartId = @PartId ");
            sb.Append($"ORDER BY Number ASC;");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@PartId", partId);
            return sqlCommand;
        }
        public static SqlCommand SelectMaintenanceLog(int? wellId)
        {
            if (wellId == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Description, Works, Confirmed, StatusChangedDate ");
            sb.Append("FROM [well].[dbo].[MaintenanceLog] ");
            sb.Append($"WHERE WellId = @WellId;");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@WellId", wellId);
            return sqlCommand;
        }
        public static SqlCommand SelectPart(int? partId)
        {
            if (partId == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Name, Description ");
            sb.Append("FROM [well].[dbo].[Part] ");
            sb.Append($"WHERE Id = @PartId;");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@PartId", partId);
            return sqlCommand;
        }
        public static SqlCommand SelectWellParts(int? wellTypeId)
        {
            if (wellTypeId == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT p.Id, p.Name, p.Description ");
            sb.Append("FROM [well].[dbo].[Part] p ");
            sb.Append("JOIN [well].[dbo].[WellParts] w ");
            sb.Append("On w.PartId = p.Id ");
            sb.Append($"WHERE w.WellTypeId = @WellTypeId;");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@WellTypeId", wellTypeId);
            return sqlCommand;
        }
        public static SqlCommand SelectSmallIssue()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, CreationDate, WellId ");
            sb.Append("FROM [well].[dbo].[Issue];");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            return sqlCommand;
        }
        public static SqlCommand SelectIssue(int? issueId)
        {
            if (issueId == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, WellId, Description, CreationDate, Status, [Open], ConfirmedBy, SolvedDate, RepairedBy, Works ");
            sb.Append("FROM [well].[dbo].[Issue] ");
            sb.Append($"WHERE Id = @IssueId;");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@IssueId", issueId);
            return sqlCommand;
        }
        public static SqlCommand SelectBrokenParts(int? issueId)
        {
            if (issueId == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT p.Id, p.Name, p.Description ");
            sb.Append("FROM [well].[dbo].[BrokenParts] b ");
            sb.Append("JOIN [well].[dbo].[Part] p ");
            sb.Append("ON b.PartId = p.id ");
            sb.Append($"WHERE b.IssueId = @IssueId;");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@IssueId", issueId);
            return sqlCommand;
        }
        public static SqlCommand SelectWellStatus(int? wellId)
        {
            if (wellId == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Status ");
            sb.Append("FROM [well].[dbo].[Well] ");
            sb.Append($"WHERE Id = @WellId;");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@WellId", wellId);
            return sqlCommand;
        }
        public static SqlCommand SelectWellTypes()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Name ");
            sb.Append("FROM [well].[dbo].[WellType];");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            return sqlCommand;
        }
        public static SqlCommand SelectIssuesFromWell(int? wellId)
        {
            if (wellId == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, WellId, Description, CreationDate, Status, [Open], ConfirmedBy, SolvedDate, RepairedBy, Works ");
            sb.Append("FROM [well].[dbo].[Issue] ");
            sb.Append($"WHERE WellId = @WellId;");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@WellId", wellId);
            return sqlCommand;
        }

        // Insert

        public static SqlCommand InsertPart(NewPart insertPart)
        {
            if (insertPart == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[Part] (Name, Description) ");
            sb.Append("OUTPUT inserted.Id ");
            sb.Append($"VALUES (@Name, @Description);");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@Name", (object)insertPart.Name ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@Description", (object)insertPart.Description ?? DBNull.Value);
            return sqlCommand;
        }
        public static SqlCommand InsertWellType(NewWellType insertWellType)
        {
            if (insertWellType == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[WellType] (Name, Particularity) ");
            sb.Append("OUTPUT INSERTED.Id ");
            sb.Append($"VALUES (@Name, @Particularity);");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@Name", (object)insertWellType.Name ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@Particularity", (object)insertWellType.Particularity ?? DBNull.Value);
            return sqlCommand;
        }
        public static SqlCommand InsertWellPart(int? wellTypeId, int? partId)
        {
            if (partId == null || wellTypeId == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[WellParts] (WellTypeId, PartId) ");
            sb.Append("OUTPUT INSERTED.Id ");
            sb.Append($"VALUES (@WellTypeId, @PartId);");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@WellTypeId", wellTypeId);
            sqlCommand.Parameters.AddWithValue("@PartId", partId);
            return sqlCommand;
        }
        public static SqlCommand InsertWell(NewWell newWell)
        {
            if (newWell == null || newWell.WellTypeId == null || newWell.FundingInfo == null || newWell.Location == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[Well] (Name, Status, Latitude, Longitude, Organisation, OpeningDate, Price , WellTypeId) ");
            sb.Append("OUTPUT INSERTED.Id ");
            sb.Append($"VALUES (@Name, @Status, @Latitude, @Longitude, @Organisation, @OpeningDate, @Price , @WellTypeId);");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@Name", (object)newWell.Name ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@Status", (object)newWell.Status ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@Latitude", (object)newWell.Location.Latitude ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@Longitude", (object)newWell.Location.Longitude ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@Organisation", (object)newWell.FundingInfo.Organisation ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@OpeningDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@Price", (object)newWell.FundingInfo.Price ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@WellTypeId", newWell.WellTypeId);
            return sqlCommand;
        }
        public static SqlCommand InsertMaintenanceLog(MaintenanceLog maintenanceLog, int? wellId)
        {
            if (maintenanceLog == null || wellId == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[MaintenanceLog] (Description, Works, Confirmed, StatusChangedDate, WellId) ");
            sb.Append("OUTPUT INSERTED.Id ");
            sb.Append($"VALUES (@Description, @Works, @Confirmed, @StatusChangedDate, @WellId);");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@Description", (object)maintenanceLog.Description ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@Works", (object)maintenanceLog.Works ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@Confirmed", (object)maintenanceLog.Confirmed ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@StatusChangedDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@WellId", wellId);
            return sqlCommand;
        }
        public static SqlCommand InsertIssue(NewIssue newIssue)
        {
            if (newIssue == null || newIssue.WellId == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append($"INSERT INTO [well].[dbo].[Issue] (WellId, Description, CreationDate, Status, [Open], ConfirmedBy, Works) ");
            sb.Append("OUTPUT inserted.Id ");
            sb.Append($"VALUES (@WellId, @Description, @CreationDate, @Status, @Open, @ConfirmedBy, @Works);");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@WellId", (object)newIssue.WellId ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@Description", (object)newIssue.Description ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@CreationDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@Status", "Issue created");
            sqlCommand.Parameters.AddWithValue("@Open", true);
            sqlCommand.Parameters.AddWithValue("@ConfirmedBy", (object)newIssue.ConfirmedBy ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@Works", (object)newIssue.Works ?? DBNull.Value);
            return sqlCommand;
        }
        public static SqlCommand InsertBrokenPart(int? partId, int? issueId)
        {
            if (issueId == null || partId == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[BrokenParts] (IssueId, PartId) ");
            sb.Append("OUTPUT inserted.Id ");
            sb.Append($"VALUES (@IssueId, @PartId);");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@IssueId", issueId);
            sqlCommand.Parameters.AddWithValue("@PartId", partId);
            return sqlCommand;
        }

        // Update

        public static SqlCommand UpdatePart(Part part)
        {
            if (part == null || part.Id == 0 || part.Name == null && part.Description == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [well].[dbo].[Part] ");
            SqlCommand sqlCommand = new SqlCommand();
            string values = "";
            if (part.Name != null)
            {
                values += "Name = @Name, ";
                sqlCommand.Parameters.AddWithValue("@Name", part.Name);
            }
            if (part.Description != null)
            {
                values += "Description = @Description, ";
                sqlCommand.Parameters.AddWithValue("@Description", part.Description);
            }
            values = values.Remove(values.Length - 2);
            sb.Append($"SET {values} ");
            sb.Append($"WHERE Id = @Id;");
            sqlCommand.Parameters.AddWithValue("@Id", part.Id);
            sqlCommand.CommandText = sb.ToString();
            return sqlCommand;
        }
        public static SqlCommand UpdateWellType(WellType wellType)
        {
            if (wellType == null || wellType.Id == null || wellType.Name == null && wellType.Particularity == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [well].[dbo].[WellType] ");
            SqlCommand sqlCommand = new SqlCommand();
            string values = "";
            if (wellType.Name != null)
            {
                values += "Name = @Name, ";
                sqlCommand.Parameters.AddWithValue("@Name", wellType.Name);
            }
            if (wellType.Particularity != null)
            {
                values += "Particularity = @Particularity, ";
                sqlCommand.Parameters.AddWithValue("@Particularity", wellType.Particularity);
            }
            values = values.Remove(values.Length - 2);
            sb.Append($"SET {values} ");
            sb.Append($"WHERE Id = @Id;");
            sqlCommand.Parameters.AddWithValue("@Id", wellType.Id);
            sqlCommand.CommandText = sb.ToString();
            return sqlCommand;
        }
        public static SqlCommand UpdateFundingInfo(FundingInfo fundingInfo, int? wellId)
        {
            if (fundingInfo == null || wellId == null || fundingInfo.OpeningDate == null && fundingInfo.Organisation == null && fundingInfo.Price == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [well].[dbo].[Well] ");
            SqlCommand sqlCommand = new SqlCommand();
            string values = "";
            if (fundingInfo.Organisation != null)
            {
                values += "Organisation = @Organisation, ";
                sqlCommand.Parameters.AddWithValue("@Organisation", fundingInfo.Organisation);
            }
            if (fundingInfo.OpeningDate != null)
            {
                values += "OpeningDate = @OpeningDate, ";
                sqlCommand.Parameters.AddWithValue("@OpeningDate", fundingInfo.OpeningDate);
            }
            if (fundingInfo.Price != null)
            {
                values += "Price = @Price, ";
                sqlCommand.Parameters.AddWithValue("@Price", fundingInfo.Price);
            }
            values = values.Remove(values.Length - 2);
            sb.Append($"SET {values} ");
            sb.Append("WHERE Id = @WellId;");
            sqlCommand.Parameters.AddWithValue("@WellId", wellId);
            return sqlCommand;
        }
        public static SqlCommand UpdateLocation(Location location, int? wellId)
        {
            if (location == null || wellId == null || location.Latitude == null && location.Longitude == null)
                return null;
            StringBuilder sb = new StringBuilder();
            SqlCommand sqlCommand = new SqlCommand();
            sb.Append("UPDATE [well].[dbo].[Well] ");
            string values = "";
            if (location.Latitude != null)
            {
                values += "Latitude = @Latitude, ";
                sqlCommand.Parameters.AddWithValue("@Latitude", location.Latitude);
            }
            if (location.Longitude != null)
            {
                values += "Longitude = @Longitude, ";
                sqlCommand.Parameters.AddWithValue("@Longitude", location.Longitude);
            }
            values = values.Remove(values.Length - 2);
            sb.Append($"SET {values} ");
            sb.Append($"WHERE Id = @WellId;");
            sqlCommand.Parameters.AddWithValue("@WellId", wellId);
            sqlCommand.CommandText = sb.ToString();
            return sqlCommand;
        }
        public static SqlCommand UpdateWell(ChangedWell changedWell)
        {
            if (changedWell == null || changedWell.Id == null || changedWell.WellTypeId == null && changedWell.Name == null && changedWell.Location == null && changedWell.FundingInfo == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [well].[dbo].[Well] ");
            SqlCommand sqlCommand = new SqlCommand();
            string values = "";
            if (changedWell.Location != null)
            {
                if (changedWell.Location.Latitude != null)
                {
                    values += "Latitude = @Latitude, ";
                    sqlCommand.Parameters.AddWithValue("@Latitude", changedWell.Location.Latitude);
                }
                if (changedWell.Location.Longitude != null)
                {
                    values += "Longitude = @Longitude, ";
                    sqlCommand.Parameters.AddWithValue("@Longitude", changedWell.Location.Longitude);
                }
            }
            if (changedWell.FundingInfo != null)
            {
                if (changedWell.FundingInfo.Organisation != null)
                {
                    values += "Organisation = @Organisation, ";
                    sqlCommand.Parameters.AddWithValue("@Organisation", changedWell.FundingInfo.Organisation);
                }
                if (changedWell.FundingInfo.OpeningDate != null)
                {
                    values += "OpeningDate = @OpeningDate, ";
                    sqlCommand.Parameters.AddWithValue("@OpeningDate", changedWell.FundingInfo.OpeningDate);
                }
                if (changedWell.FundingInfo.Price != null)
                {
                    values += "Price = @Price, ";
                    sqlCommand.Parameters.AddWithValue("@Price", changedWell.FundingInfo.Price);
                }
            }

            if (changedWell.WellTypeId != null)
            {
                values += "WellTypeId = @WellTypeId, ";
                sqlCommand.Parameters.AddWithValue("@WellTypeId", changedWell.WellTypeId);
            }
            if (changedWell.Name != null)
            {
                values += "Name = @Name, ";
                sqlCommand.Parameters.AddWithValue("@Name", changedWell.Name);
            }
            if (changedWell.Status != null)
            {
                values += "Status = @Status, ";
                sqlCommand.Parameters.AddWithValue("@Status", changedWell.Status);
            }
            values = values.Remove(values.Length - 2);
            sb.Append($"SET {values} ");
            sb.Append("WHERE Id = @Id;");
            sqlCommand.Parameters.AddWithValue("@Id", changedWell.Id);
            sqlCommand.CommandText = sb.ToString();
            return sqlCommand;
        }
        public static SqlCommand UpdateMaintenanceLog(MaintenanceLog maintenanceLog, int? wellId)
        {
            if (maintenanceLog == null || maintenanceLog.Id == null || wellId == null && maintenanceLog.Description == null && maintenanceLog.Works == null && maintenanceLog.Confirmed == null && maintenanceLog.StatusChangedDate == null)
                return null;
            StringBuilder sb = new StringBuilder();
            SqlCommand sqlCommand = new SqlCommand();
            sb.Append("UPDATE [well].[dbo].[MaintenanceLog] ");
            string values = "";
            if (maintenanceLog.Description != null)
            {
                values += "Description = @Description, ";
                sqlCommand.Parameters.AddWithValue("@Description", maintenanceLog.Description);
            }
            if (maintenanceLog.Works != null)
            {
                values += "Works = @Works, ";
                sqlCommand.Parameters.Add("@Works", SqlDbType.Bit).Value = maintenanceLog.Works;
            }
            if (maintenanceLog.Confirmed != null)
            {
                values += "Confirmed = @Confirmed, ";
                sqlCommand.Parameters.Add("@Confirmed", SqlDbType.Bit).Value = maintenanceLog.Confirmed;
            }
            if (maintenanceLog.StatusChangedDate != null)
            {
                values += "StatusChangedDate = @StatusChangedDate, ";
                sqlCommand.Parameters.AddWithValue("@StatusChangedDate", maintenanceLog.StatusChangedDate);
            }
            if (wellId != null)
            {
                values += "WellId = @WellId, ";
                sqlCommand.Parameters.AddWithValue("@WellId", wellId);
            }
            values = values.Remove(values.Length - 2);
            sb.Append($"SET {values} ");
            sb.Append($"WHERE Id = @Id;");
            sqlCommand.Parameters.AddWithValue("@Id", maintenanceLog.Id);
            sqlCommand.CommandText = sb.ToString();
            return sqlCommand;
        }
        public static SqlCommand UpdateIssue(UpdateIssue updateIssue)
        {
            if (updateIssue == null || updateIssue.Id == null || updateIssue.WellId == null && updateIssue.Description == null && updateIssue.CreationDate == null && updateIssue.Status == null && updateIssue.Open == null && updateIssue.ConfirmedBy == null && updateIssue.SolvedDate == null && updateIssue.RepairedBy == null && updateIssue.Works == null)
                return null;
            StringBuilder sb = new StringBuilder();
            SqlCommand sqlCommand = new SqlCommand();
            sb.Append("UPDATE [well].[dbo].[Issue] ");
            string values = "";
            if (updateIssue.WellId != null)
            {
                values += "WellId = @WellId, ";
                sqlCommand.Parameters.AddWithValue("@WellId", updateIssue.WellId);
            }
            if (updateIssue.Description != null)
            {
                values += "Description = @Description, ";
                sqlCommand.Parameters.AddWithValue("@Description", updateIssue.Description);
            }
            if (updateIssue.CreationDate != null)
            {
                values += "CreationDate = @CreationDate, ";
                sqlCommand.Parameters.AddWithValue("@CreationDate", updateIssue.CreationDate);
            }
            if (updateIssue.Status != null)
            {
                values += "Status = @Status, ";
                sqlCommand.Parameters.AddWithValue("@Status", updateIssue.Status);
            }
            if (updateIssue.Open != null)
            {
                values += "[Open] = @Open, ";
                sqlCommand.Parameters.Add("@Open", SqlDbType.Bit).Value = updateIssue.Open;
            }
            if (updateIssue.ConfirmedBy != null)
            {
                values += "ConfirmedBy = @ConfirmedBy, ";
                sqlCommand.Parameters.AddWithValue("@ConfirmedBy", updateIssue.ConfirmedBy);
            }
            if (updateIssue.SolvedDate != null)
            {
                values += "SolvedDate = @SolvedDate, ";
                sqlCommand.Parameters.AddWithValue("@SolvedDate", updateIssue.SolvedDate);
            }
            if (updateIssue.RepairedBy != null)
            {
                values += "RepairedBy = @RepairedBy, ";
                sqlCommand.Parameters.AddWithValue("@RepairedBy", updateIssue.RepairedBy);
            }
            if (updateIssue.Works != null)
            {
                values += "Works = @Works, ";
                sqlCommand.Parameters.Add("@Works", SqlDbType.Bit).Value = updateIssue.Works;
            }
            values = values.Remove(values.Length - 2);
            sb.Append($"SET {values} ");
            sb.Append($"WHERE Id = @Id;");
            sqlCommand.Parameters.AddWithValue("@Id", updateIssue.Id);
            sqlCommand.CommandText = sb.ToString();
            return sqlCommand;
        }
        public static SqlCommand UpdateWellStatus(int? wellId, string status)
        {
            if (status == null || wellId == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [well].[dbo].[Well] ");
            sb.Append($"SET Status = @Status ");
            sb.Append($"WHERE Id = @WellId;");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@Status", status);
            sqlCommand.Parameters.AddWithValue("@WellId", wellId);
            return sqlCommand;
        }

        // Delete
        public static SqlCommand DeleteWell(int? wellId)
        {
            if (wellId == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE ");
            sb.Append("FROM [well].[dbo].[Well] ");
            sb.Append($"WHERE Id = @WellId;");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@WellId", wellId);
            return sqlCommand;
        }
        public static SqlCommand DeleteIssue(int? issueId)
        {
            if (issueId == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE ");
            sb.Append("FROM [well].[dbo].[Issue] ");
            sb.Append($"WHERE Id = @IssueId;");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@IssueId", issueId);
            return sqlCommand;
        }
        public static SqlCommand DeleteBrokenPart(int? partId, int? issueId)
        {
            if (issueId == null || partId == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE ");
            sb.Append("FROM [well].[dbo].[BrokenParts] ");
            sb.Append($"WHERE (IssueId = @IssueId AND PartId = @PartId);");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@IssueId", issueId);
            sqlCommand.Parameters.AddWithValue("@PartId", partId);
            return sqlCommand;
        }
    }
}
