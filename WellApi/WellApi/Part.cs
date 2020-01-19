using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellApi
{
    public class Part
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class SqlQuerry
    {
        public static string InsertParts(Part[] parts)
        {
            if (parts == null || parts.Length == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[Part] (Name, Description)");
            sb.Append("OUTPUT inserted.Id");
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
            sb.Append("INSERT INTO [well].[dbo].[WellType] (Name, Particularity,Depth)");
            sb.Append("OUTPUT INSERTED.Id");
            sb.Append($"VALUES ('{wellType.Name}','{wellType.Particularity}',{wellType.Depth});");
            return sb.ToString();
        }
        public static string InsertWellParts(int wellTypeId, int[] partId)
        {
            if (partId == null)
                return null;
            if (partId.Length == 0 || wellTypeId == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[WellParts] (WellTypeId, PartId)");
            for (int i = 0; i < partId.Length - 1; i++)
            {
                sb.Append($"VALUES ({wellTypeId},{partId[i]}),");
            }
            sb.Append($"VALUES ({wellTypeId},{partId[partId.Length - 1]});");
            return sb.ToString();
        }
        public static string InsertFundingInfo(FundingInfo fundingInfo)
        {
            if(fundingInfo == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[FundingInfo] (Organisation, OpeningDate, Price)");
            sb.Append("OUTPUT INSERTED.Id");
            if (fundingInfo.OpeningDate == null)
                sb.Append($"VALUES ('{fundingInfo.Organisation}',CURRENT_TIMESTAMP,{fundingInfo.Price});");
            else
                sb.Append($"VALUES ('{fundingInfo.Organisation}','{fundingInfo.OpeningDate.Date}',{fundingInfo.Price});");
            return sb.ToString();
        }
        public static string InsertLocation(Location location)
        {
            if (location == null)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[Location] (Longitude, Latitude)");
            sb.Append("OUTPUT INSERTED.Id");
            sb.Append($"VALUES ({location.Longitude},{location.Latitude});");
            return sb.ToString();
        }
        public static string InsertWell(Well well)
        {
            if (well == null || well.WellType.Id == 0 || well.FundingInfo.Id == 0 || well.Location.Id == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            // no Image!!!
            sb.Append("INSERT INTO [well].[dbo].[Well] (Name, Status, LocationId, FundingInfoId, WellTypeId)");
            sb.Append("OUTPUT INSERTED.Id");
            sb.Append($"VALUES ('{well.Name}', '{well.Status}', {well.Location.Id}, {well.FundingInfo.Id}, {well.WellType.Id});");
            return sb.ToString();
        }
        public static string InsertStatusHistory(WellStatus[] statusHistory, int wellId)
        {
            if (statusHistory == null || wellId == 0 || statusHistory.Length == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            // no Image!!!
            sb.Append("INSERT INTO [well].[dbo].[StatusHistory] (Description, Works, Confirmed, StatusChangedDate, WellId)");
            List<string> values = new List<string>();
            foreach (WellStatus status in statusHistory)
            {
                if (status.StatusChangedDate == null)
                    sb.Append($"VALUES ('{status.Description}',{status.Works},{status.Confirmed},CURRENT_TIMESTAMP, {wellId}),");
                else
                    sb.Append($"VALUES ('{status.Description}',{status.Works},{status.Confirmed},'{status.StatusChangedDate.Date}', {wellId}),");
            }
            if (values.Count == 0)
                return null;
            values[0] = "VALUES " + values[0];
            values[values.Count - 1].TrimEnd(',');
            values[values.Count - 1].Append(';');
            return sb.ToString();
        }
        public static string DeleteWell(int wellId)
        {
            if (wellId == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE");
            sb.Append("FROM [well].[dbo].[Well] w");
            sb.Append("JOIN [well].[dbo].[StatusHistory] h");
            sb.Append("On w.Id = h.WellId");
            sb.Append("JOIN [well].[dbo].[FundingInfo] f");
            sb.Append("On w.FundingId = f.Id");
            sb.Append("JOIN [well].[dbo].[Issue] i");
            sb.Append("On w.Id = i.WellId");
            sb.Append($"WHERE w.Id = {wellId};");
            return sb.ToString();
        }
        public static string UpdateParts(Part[] parts)
        {
            if (parts == null)
                return null;
            if (parts.Length == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[Part] (Id, Name, Description)");
            List<string> values = new List<string>();
            foreach (Part part in parts)
            {
                if (part.Id != 0)
                    values.Add($"({part.Id}, '{part.Name}', '{part.Description}'),");
            }
            if (values.Count == 0)
                return null;
            values[0] = "VALUES " + values[0];
            values[values.Count - 1].TrimEnd(',');
            foreach (string value in values)
            {
                sb.Append(value);
            }
            sb.Append($"ON DUPLICATE KEY UPDATE Id=VALUES(Id),");
            sb.Append($"Name=VALUES(Name),");
            sb.Append($"Description=VALUES(Description);");
            return sb.ToString();
        }
        public static string UpdateWellType(WellType wellType)
        {
            if (wellType == null || wellType.Id == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [well].[dbo].[WellType]");           
            sb.Append($"SET Name = '{wellType.Name}', Particularity = '{wellType.Particularity}', Depth = {wellType.Depth}");
            sb.Append($"WHERE Id = {wellType.Id};");
            return sb.ToString();
        }
        public static string UpdateFundingInfo(FundingInfo fundingInfo)
        {
            if (fundingInfo == null || fundingInfo.Id == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [well].[dbo].[FundingInfo]");
            sb.Append($"SET Organisation = '{fundingInfo.Organisation}', OpeningDate = '{fundingInfo.OpeningDate.Date}', Price = {fundingInfo.Price}");
            sb.Append($"WHERE Id = {fundingInfo.Id};");
            return sb.ToString();
        }
        public static string UpdateLocation(Location location)
        {
            if (location == null || location.Id == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [well].[dbo].[Location]");
            sb.Append($"SET Longitude = {location.Longitude}, Latitude = {location.Latitude}");
            sb.Append($"WHERE Id = {location.Id};");
            return sb.ToString();
        }
        public static string UpdateWell(Well well)
        {
            if (well == null || well.Id == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [well].[dbo].[Well]");
            sb.Append($"SET Name = '{well.Name}', Status = '{well.Status}', LocationId = {well.Location.Id}, FundingInfoId = {well.FundingInfo.Id}, WellTypeId = {well.WellType.Id}" );
            sb.Append($"WHERE Id = {well.Id};");
            return sb.ToString();
        }
        public static string UpdateStatusHistory(WellStatus[] statusHistory, int wellId)
        {
            if (statusHistory == null || statusHistory.Length == 0 || wellId == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[StatusHistory] (Id, Description, Works, Confirmed, StatusChangedDate, WellId)");
            List<string> values = new List<string>();
            foreach (WellStatus status in statusHistory)
            {
                if (status.Id != 0)
                    values.Add($"({status.Id}, '{status.Description}', {status.Works}, {status.Confirmed}, {status.StatusChangedDate.Date}, {wellId}),");
            }
            if (values.Count == 0)
                return null;
            values[0] = "VALUES " + values[0];
            values[values.Count - 1].TrimEnd(',');
            foreach (string value in values)
            {
                sb.Append(value);
            }
            sb.Append($"ON DUPLICATE KEY UPDATE Id=VALUES(Id),");
            sb.Append($"Description=VALUES(Description),");
            sb.Append($"Works=VALUES(Works),");
            sb.Append($"Confirmed=VALUES(Confirmed),");
            sb.Append($"StatusChangedDate=VALUES(StatusChangedDate),");
            sb.Append($"WellId=VALUES(WellId);");
            return sb.ToString();
        }
    }
}
