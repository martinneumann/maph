using System;
using System.Collections.Generic;
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
        public static string SelectWell(int wellId)
        {
            if (wellId == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT w.Id, w.Name, w.Status, w.Latitude, w.Longitude, w.Organisation, w.OpeningDate, w.Price, t.Id, t.Name, t.Particularity, t.Depth ");
            sb.Append("FROM [well].[dbo].[Well] w ");
            sb.Append("JOIN [well].[dbo].[WellType] t ");
            sb.Append("ON w.WellTypeId = t.Id ");
            sb.Append($"WHERE w.Id = {wellId};");
            return sb.ToString();
        }
        public static string SelectMaintenanceLog(int wellId)
        {
            if (wellId == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Description, Works, Confirmed, StatusChangedDate ");
            sb.Append("FROM [well].[dbo].[StatusHistory] ");
            sb.Append($"WHERE Id = {wellId};");
            return sb.ToString();
        }
        public static string SelectWellParts(int wellTypeId)
        {
            if (wellTypeId == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT p.Id, p.Name, p.Description ");
            sb.Append("FROM [well].[dbo].[Part] p ");
            sb.Append("JOIN [well].[dbo].[WellParts] w ");
            sb.Append("On w.PartId = p.Id ");
            sb.Append($"WHERE w.WellTypeId = {wellTypeId};");
            return sb.ToString();
        }
        public static string SelectSmallIssue()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, CreationDate, WellId ");
            sb.Append("FROM [well].[dbo].[Issue];");
            return sb.ToString();
        }
        public static string SelectIssue(int issueId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, WellId, Description, CreationDate, Status, [Open], ConfirmedBy, SolvedDate, RepairedBy, Works ");
            sb.Append("FROM [well].[dbo].[Issue] ");
            sb.Append($"WHERE Id = {issueId};");
            return sb.ToString();
        }
        public static string SelectBrokenParts(int issueId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT p.Id, p.Name, p.Description ");
            sb.Append("FROM [well].[dbo].[BrokenParts] b ");
            sb.Append("JOIN [well].[dbo].[Part] p ");
            sb.Append("ON b.PartId = p.id ");
            sb.Append($"WHERE b.IssueId = {issueId};");
            return sb.ToString();
        }
        public static string SelectWellStatus(int wellId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Status ");
            sb.Append("FROM [well].[dbo].[Well] ");
            sb.Append($"WHERE Id = {wellId};");
            return sb.ToString();
        }
        public static SqlCommand SelectWellTypes()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Name ");
            sb.Append("FROM [well].[dbo].[WellType];");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            return sqlCommand;
        }

        // Insert

        public static string InsertParts(Part[] parts)
        {
            if (parts == null || parts.Length == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[Part] (Name, Description) ");
            sb.Append("OUTPUT inserted.Id ");
            for (int i = 0; i < parts.Length - 1; i++)
            {
                sb.Append($"VALUES ('{parts[i].Name}','{parts[i].Description}'),");
            }
            sb.Append($"VALUES ('{parts[parts.Length - 1].Name}','{parts[parts.Length - 1].Description}');");
            return sb.ToString();
        }
        public static string InsertWellType(WellType wellType)
        {
            if (wellType == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[WellType] (Name, Particularity,Depth) ");
            sb.Append("OUTPUT INSERTED.Id ");
            sb.Append($"VALUES ('{wellType.Name}','{wellType.Particularity}',{wellType.Depth.ToString(System.Globalization.CultureInfo.InvariantCulture)});");
            return sb.ToString();
        }
        public static string InsertWellParts(int wellTypeId, int[] partId)
        {
            if (partId == null)
                return null;
            if (partId.Length == 0 || wellTypeId == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[WellParts] (WellTypeId, PartId) VALUES ");
            for (int i = 0; i < partId.Length - 1; i++)
            {
                sb.Append($"({wellTypeId},{partId[i]}),");
            }
            sb.Append($" ({wellTypeId},{partId[partId.Length - 1]});");
            return sb.ToString();
        }
        public static SqlCommand InsertWell(NewWell NewWell)
        {
            if (NewWell == null || NewWell.WellTypeId == 0 || NewWell.FundingInfo == null || NewWell.Location == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[Well] (Name, Status, Latitude, Longitude, Organisation, OpeningDate, Price , WellTypeId) ");
            sb.Append("OUTPUT INSERTED.Id ");
            sb.Append($"VALUES (@Name, @Status, @Latitude, @Longitude, @Organisation, @OpeningDate, @Price , @WellTypeId);");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@Name", NewWell.Name);
            sqlCommand.Parameters.AddWithValue("@Status", NewWell.Status);
            sqlCommand.Parameters.AddWithValue("@Latitude", NewWell.Location.Latitude);
            sqlCommand.Parameters.AddWithValue("@Longitude", NewWell.Location.Longitude);
            sqlCommand.Parameters.AddWithValue("@Organisation", NewWell.FundingInfo.Organisation);
            sqlCommand.Parameters.AddWithValue("@OpeningDate", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@Price", NewWell.FundingInfo.Price);
            sqlCommand.Parameters.AddWithValue("@WellTypeId", NewWell.WellTypeId);
            return sqlCommand;
        }
        public static string InsertMaintenanceLogs(MaintenanceLog[] maintenanceLogs, int wellId)
        {
            if (maintenanceLogs == null || wellId == 0 || maintenanceLogs.Length == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[StatusHistory] (Description, Works, Confirmed, StatusChangedDate, WellId) ");
            List<string> values = new List<string>();
            foreach (MaintenanceLog maintenanceLog in maintenanceLogs)
            {
                if (maintenanceLog.StatusChangedDate == null || maintenanceLog.StatusChangedDate == new DateTime())
                    values.Add($"('{maintenanceLog.Description}',{Convert.ToInt32(maintenanceLog.Works)},{Convert.ToInt32(maintenanceLog.Confirmed)},CURRENT_TIMESTAMP, {wellId}),");
                else
                    values.Add($"('{maintenanceLog.Description}',{Convert.ToInt32(maintenanceLog.Works)},{Convert.ToInt32(maintenanceLog.Confirmed)},'{maintenanceLog.StatusChangedDate.ToString("yyyyMMdd HH:mm:ss")}', {wellId}),");
            }
            if (values.Count == 0)
                return null;
            values[0] = "VALUES " + values[0];
            values[values.Count - 1] = values[values.Count - 1].TrimEnd(',');
            values[values.Count - 1] += ";";
            foreach (string value in values)
            {
                sb.Append(value);
            }
            return sb.ToString();
        }
        public static string InsertIssue(NewIssue newIssue)
        {
            if (newIssue == null || newIssue.WellId == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            string insertColumns = "";
            string insertColumnValues = "";
            insertColumns += "WellId, ";
            insertColumnValues += $"{newIssue.WellId}, ";
            if (newIssue.Description != null)
            {
                insertColumns += "Description, ";
                insertColumnValues += $"'{newIssue.Description}', ";
            }
            insertColumns += "CreationDate, ";
            insertColumnValues += $"'{DateTime.Now.ToString("yyyyMMdd HH:mm:ss")}', ";
            insertColumns += "Status, ";
            insertColumnValues += $"'Issue created', ";
            insertColumns += "[Open], ";
            insertColumnValues += $"{1}, ";
            if (newIssue.ConfirmedBy != null)
            {
                insertColumns += "ConfirmedBy, ";
                insertColumnValues += $"'{newIssue.ConfirmedBy}', ";
            }
            insertColumns += "Works";
            insertColumnValues += $"{Convert.ToInt32(newIssue.Works)}";
            sb.Append($"INSERT INTO [well].[dbo].[Issue] ({insertColumns}) ");
            sb.Append("OUTPUT inserted.Id ");
            sb.Append($"VALUES ({insertColumnValues});");
            return sb.ToString();
        }
        public static string InsertBrokenParts(int[] partIds, int issueId)
        {
            if (issueId == 0 || partIds.Length == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[BrokenParts] (IssueId, PartId) ");
            List<string> values = new List<string>();
            foreach (int partId in partIds)
            {
                if (partId != 0)
                    values.Add($"({issueId}, {partId}),");
            }
            if (values.Count == 0)
                return null;
            values[0] = "VALUES " + values[0];
            values[values.Count - 1].TrimEnd(',');
            values[values.Count - 1].Append(';');
            foreach (string value in values)
            {
                sb.Append(value);
            }
            return sb.ToString();
        }

        // Update

        public static string UpdatePart(Part part)
        {
            if (part == null || part.Id == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [well].[dbo].[Part] ");
            sb.Append($"SET Name = '{part.Name}', Description = '{part.Description}' ");
            sb.Append($"WHERE Id = {part.Id};");
            return sb.ToString();
        }
        public static string UpdateWellType(WellType wellType)
        {
            if (wellType == null || wellType.Id == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [well].[dbo].[WellType] ");
            sb.Append($"SET Name = '{wellType.Name}', Particularity = '{wellType.Particularity}', Depth = {wellType.Depth.ToString(System.Globalization.CultureInfo.InvariantCulture)} ");
            sb.Append($"WHERE Id = {wellType.Id};");
            return sb.ToString();
        }
        public static SqlCommand UpdateFundingInfo(FundingInfo fundingInfo, int wellId)
        {
            if (fundingInfo == null || wellId == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [well].[dbo].[Well] ");
            sb.Append("SET Organisation = @Organisation, OpeningDate = @OpeningDate, Price = @Price ");
            sb.Append("WHERE Id = @WellId;");
            SqlCommand sqlCommand = new SqlCommand(sb.ToString());
            sqlCommand.Parameters.AddWithValue("@Organisation", fundingInfo.Organisation);
            if (fundingInfo.OpeningDate == new DateTime())
                sqlCommand.Parameters.AddWithValue("@OpeningDate", DateTime.Now);
            else
                sqlCommand.Parameters.AddWithValue("@OpeningDate", fundingInfo.OpeningDate);
            sqlCommand.Parameters.AddWithValue("@Price", fundingInfo.Price);
            sqlCommand.Parameters.AddWithValue("@WellId", wellId);
            return sqlCommand;
        }
        public static string UpdateLocation(Location location, int wellId)
        {
            if (location == null || wellId == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [well].[dbo].[Well] ");
            sb.Append($"SET Latitude = {location.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}, Longitude = {location.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)} ");
            sb.Append($"WHERE Id = {wellId};");
            return sb.ToString();
        }
        public static SqlCommand UpdateWell(ChangedWell changedWell)
        {
            if (changedWell == null || changedWell.Id == 0 || changedWell.WellTypeId == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [well].[dbo].[Well] ");
            string setToUpdate = "";
            SqlCommand sqlCommand = new SqlCommand();
            if (changedWell.Location != null)
            {
                setToUpdate += "Latitude = @Latitude, ";
                sqlCommand.Parameters.AddWithValue("@Latitude", changedWell.Location.Latitude);
                setToUpdate += "Longitude = @Longitude, ";
                sqlCommand.Parameters.AddWithValue("@Longitude", changedWell.Location.Longitude);
            }
            if (changedWell.FundingInfo != null)
            {
                setToUpdate += "Organisation = @Organisation, ";
                sqlCommand.Parameters.AddWithValue("@Organisation", changedWell.FundingInfo.Organisation);
                if(changedWell.FundingInfo.OpeningDate != new DateTime())
                {
                    setToUpdate += "OpeningDate = @OpeningDate, ";
                    sqlCommand.Parameters.AddWithValue("@OpeningDate", changedWell.FundingInfo.OpeningDate);
                }
                setToUpdate += "Price = @Price, ";
                sqlCommand.Parameters.AddWithValue("@Price", changedWell.FundingInfo.Price);
            }
            setToUpdate += "WellTypeId = @WellTypeId, ";
            sqlCommand.Parameters.AddWithValue("@WellTypeId", changedWell.WellTypeId);
            sb.Append($"SET Name = @Name, {setToUpdate}Status = @Status ");
            sqlCommand.Parameters.AddWithValue("@Name", changedWell.Name);
            sqlCommand.Parameters.AddWithValue("@Status", changedWell.Status);
            sb.Append("WHERE Id = @Id;");
            sqlCommand.Parameters.AddWithValue("@Id", changedWell.Id);
            sqlCommand.CommandText = sb.ToString();
            return sqlCommand;
        }
        public static string UpdateMaintenanceLog(MaintenanceLog maintenanceLog, int wellId)
        {
            if (maintenanceLog == null || maintenanceLog.Id == 0 || wellId == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [well].[dbo].[StatusHistory] ");
            sb.Append($"SET Description = '{maintenanceLog.Description}', Works = {Convert.ToInt32(maintenanceLog.Works)}, Confirmed = {Convert.ToInt32(maintenanceLog.Confirmed)}, StatusChangedDate = '{maintenanceLog.StatusChangedDate.ToString("yyyyMMdd HH:mm:ss")}', WellId = {wellId} ");
            sb.Append($"WHERE Id = {maintenanceLog.Id};");
            return sb.ToString();
        }
        public static string UpdateIssue(UpdateIssue updateIssue)
        {
            if (updateIssue == null || updateIssue.Id == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [well].[dbo].[Issue] ");
            sb.Append($"SET WellId = {updateIssue.WellId}, Description = '{updateIssue.Description}', CreationDate = '{updateIssue.CreationDate.ToString("yyyyMMdd HH:mm:ss")}', Status = '{updateIssue.Status}', [Open] = {Convert.ToInt32(updateIssue.Open)}, ConfirmedBy = '{updateIssue.ConfirmedBy}', SolvedDate = '{updateIssue.SolvedDate.ToString("yyyyMMdd HH:mm:ss")}', RepairedBy = '{updateIssue.RepairedBy}', Works = {Convert.ToInt32(updateIssue.Works)} ");
            sb.Append($"WHERE Id = {updateIssue.Id};");
            return sb.ToString();
        }
        public static string UpdateWellStatus(int wellId, string status)
        {
            if (status == null || wellId == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [well].[dbo].[Well] ");
            sb.Append($"SET Status = '{status}' ");
            sb.Append($"WHERE Id = {wellId};");
            return sb.ToString();
        }

        // Delete
        public static string DeleteWell(int wellId)
        {
            if (wellId == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE ");
            sb.Append("FROM [well].[dbo].[Well] ");
            sb.Append($"WHERE Id = {wellId};");
            return sb.ToString();
        }
        public static string DeleteIssue(int issueId)
        {
            if (issueId == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE ");
            sb.Append("FROM [well].[dbo].[Issue] ");
            sb.Append($"WHERE Id = {issueId};");
            return sb.ToString();
        }
    }
}
