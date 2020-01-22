using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Device.Location;

namespace WellApi
{
    public class DB
    {
        // Connection

        static SqlConnection sqlConnection = null;
        public static void ConnectToDb(string connectionString)
        {
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }      
        public static void DisconnectFromDb()
        {
            if( sqlConnection != null)
            {
                sqlConnection.Close();
                sqlConnection = null;
            }
        }

        // Well

        // Mutiple SQL Querries
        public static Well GetCompleteWell(int wellId)
        {
            Well well = ExecuteSelectWell(wellId);
            if (well == null || well.Id == 0)
                return null;
            well.StatusHistory = ExecuteSelectStatusHistory(wellId);
            if (well.WellType.Id == 0)
                return well;
            well.WellType.Parts = ExecuteSelectWellParts(well.WellType.Id);
            return well;
        }
        public static int AddCompleteWell(Well well)
        {
            if (well == null)
                return 0;
            well.WellType.Id = ExecuteInsertWellType(well.WellType);
            if (well.WellType.Id != 0)
            {
                int[] partIds = ExecuteInsertParts(well.WellType.Parts);
                ExecuteInsertWellParts(well.WellType.Id, partIds);
            }
            well.FundingInfo.Id = ExecuteInsertFundingInfo(well.FundingInfo);
            well.Location.Id = ExecuteInsertLocation(well.Location);
            well.Id = ExecuteInsertWell(well);
            if (well.Id == 0)
                return 0;
            if (well.StatusHistory == null || well.StatusHistory.Length == 0)
            {
                List<WellStatus> statusHistory = new List<WellStatus>();
                WellStatus status = new WellStatus
                {
                    Works = true,
                    Confirmed = true,
                    Description = "new Well created"
                };
                statusHistory.Add(status);
                well.StatusHistory = statusHistory.ToArray();
            }
            ExecuteInsertStatusHistory(well.StatusHistory, well.Id);
            return well.Id;
        }
        public static int UpdateCompleteWell(Well well)
        {
            if (well == null || well.Id == 0)
                return 0;
            
            int affected = 0;
            Well oldWell = ExecuteSelectWell(well.Id);
            if (well.WellType != null && well.WellType.Parts != null)
            {
                foreach (Part part in well.WellType.Parts)
                {
                    affected += ExecuteUpdatePart(part);
                }
            }
            if (well.WellType != null && well.WellType.Id == 0)
                well.WellType.Id = oldWell.WellType.Id;
            affected += ExecuteUpdateWellType(well.WellType);
            if (well.FundingInfo != null && well.FundingInfo.Id == 0)
                well.FundingInfo.Id = oldWell.FundingInfo.Id;
            affected += ExecuteUpdateFundingInfo(well.FundingInfo);
            if (well.Location != null && well.Location.Id == 0)
                well.Location.Id = oldWell.Location.Id;
            affected += ExecuteUpdateLocation(well.Location);
            affected += ExecuteUpdateWell(well);
            if (well.StatusHistory == null)
                return affected;
            foreach (WellStatus status in well.StatusHistory)
            {
                affected += ExecuteUpdateStatusHistory(status, well.Id);
            }
            return affected;
        }

        // Single SQL Querry

        public static SmallWell[] ExecuteSelectNearbySmallWells(SearchNearbyWells searchNearbyWells)
        {
            if (searchNearbyWells == null && searchNearbyWells.SearchRadius == 0 && searchNearbyWells.Location == null)
                return null;
            GeoCoordinate position = new GeoCoordinate(searchNearbyWells.Location.Latitude, searchNearbyWells.Location.Longitude);
            SmallWell[] smallWells = ExecuteSelectSmallWells();
            if (smallWells == null)
                return null;
            List<SmallWell> nearbySmallWells = new List<SmallWell>();
            foreach (SmallWell well in smallWells)
            {
                GeoCoordinate coordinate = new GeoCoordinate(well.Location.Latitude, well.Location.Longitude);
                if (position.GetDistanceTo(coordinate) <= searchNearbyWells.SearchRadius)
                {
                    nearbySmallWells.Add(well);
                }
            }
            return nearbySmallWells.ToArray();
        }
        public static SmallWell[] ExecuteSelectSmallWells()
        {
            String sqlSelectSmallWells = SqlQuerry.SelectSmallWells();
            if (sqlSelectSmallWells == null)
                return null;
            List<SmallWell> smallWells = new List<SmallWell>();
            using (SqlCommand command = new SqlCommand(sqlSelectSmallWells, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SmallWell smallWell = new SmallWell
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Status = reader.GetString(2),
                            Location = new Location
                            {
                                Id = reader.GetInt32(3),
                                Longitude = reader.GetDouble(4),
                                Latitude = reader.GetDouble(5)
                            }
                        };
                        smallWells.Add(smallWell);
                    }
                }
            }
            return smallWells.ToArray();
        }
        public static Well ExecuteSelectWell(int wellId)
        {
            string sqlSelectWell = SqlQuerry.SelectWell(wellId);
            if (sqlSelectWell == null)
                return null;

            using (SqlCommand command = new SqlCommand(sqlSelectWell, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Well well = new Well();
                        if (!reader.IsDBNull(0))
                            well.Id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            well.Name = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            well.Status = reader.GetString(2);
                        well.Location = new Location();
                        if (!reader.IsDBNull(3))
                            well.Location.Id = reader.GetInt32(3);
                        if (!reader.IsDBNull(4))
                            well.Location.Longitude = reader.GetDouble(4);
                        if (!reader.IsDBNull(5))
                            well.Location.Latitude = reader.GetDouble(5);
                        well.FundingInfo = new FundingInfo();
                        if (!reader.IsDBNull(6))
                            well.FundingInfo.Id = reader.GetInt32(6);
                        if (!reader.IsDBNull(7))
                            well.FundingInfo.Organisation = reader.GetString(7);
                        if (!reader.IsDBNull(8))
                            well.FundingInfo.OpeningDate = reader.GetDateTime(8);
                        if (!reader.IsDBNull(9))
                            well.FundingInfo.Price = reader.GetDouble(9);
                        well.WellType = new WellType();
                        if (!reader.IsDBNull(10))
                            well.WellType.Id = reader.GetInt32(10);
                        if (!reader.IsDBNull(11))
                            well.WellType.Name = reader.GetString(11);
                        if (!reader.IsDBNull(12))
                            well.WellType.Particularity = reader.GetString(12);
                        if (!reader.IsDBNull(13))
                            well.WellType.Depth = reader.GetDouble(13);
                        return well;
                    }
                }
            }
            return null;
        }     
        public static WellStatus[] ExecuteSelectStatusHistory(int wellId)
        {
            string sqlSelectStatusHistory = SqlQuerry.SelectStatusHistory(wellId);
            if (sqlSelectStatusHistory == null)
                return new WellStatus[0];
            List<WellStatus> wellStatuses = new List<WellStatus>();
            using (SqlCommand command = new SqlCommand(sqlSelectStatusHistory, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        WellStatus wellStatus = new WellStatus
                        {
                            Id = reader.GetInt32(0),
                            Description = reader.GetString(1),
                            Works = reader.GetBoolean(2),
                            Confirmed = reader.GetBoolean(3),
                            StatusChangedDate = reader.GetDateTime(4)
                        };
                        wellStatuses.Add(wellStatus);
                    }
                }
            }
            return wellStatuses.ToArray();
        }
        public static Part[] ExecuteSelectWellParts(int wellTypeId)
        {
            string sqlSelectWellParts = SqlQuerry.SelectWellParts(wellTypeId);
            if (sqlSelectWellParts == null)
                return null;
            List<Part> parts = new List<Part>();
            using (SqlCommand command = new SqlCommand(sqlSelectWellParts, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Part part = new Part
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2)
                        };
                        parts.Add(part);
                    }
                }
            }
            return parts.ToArray();
        }
        public static int[] ExecuteInsertParts(Part[] parts)
        {
            string sqlParts = SqlQuerry.InsertParts(parts);
            if (sqlParts == null)
                return null;
            List<int> partsId = new List<int>();
            using (SqlCommand command = new SqlCommand(sqlParts, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        partsId.Add(reader.GetInt32(0));
                    }
                }
            }
            return partsId.ToArray();
        }
        public static int ExecuteInsertWellType(WellType wellType)
        {
            string sqlWellType = SqlQuerry.InsertWellType(wellType);
            if (sqlWellType == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlWellType, sqlConnection))
            {
                return (int)command.ExecuteScalar();
            }
        }
        public static void ExecuteInsertWellParts(int wellTypeId, int[] partId)
        {
            string sqlWellParts = SqlQuerry.InsertWellParts(wellTypeId, partId);
            if (sqlWellParts == null)
                return;
            using (SqlCommand command = new SqlCommand(sqlWellParts, sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static int ExecuteInsertFundingInfo(FundingInfo fundingInfo)
        {
            string sqlFundingInfo = SqlQuerry.InsertFundingInfo(fundingInfo);
            if (sqlFundingInfo == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlFundingInfo, sqlConnection))
            {
                return (int)command.ExecuteScalar();
            }
        }
        public static int ExecuteInsertLocation(Location location)
        {
            string sqlLocation = SqlQuerry.InsertLocation(location);
            if (sqlLocation == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlLocation, sqlConnection))
            {
                return (int)command.ExecuteScalar();
            }
        }
        public static int ExecuteInsertWell(Well well)
        {
            string sqlWell = SqlQuerry.InsertWell(well);
            if (sqlWell == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlWell, sqlConnection))
            {
                return (int)command.ExecuteScalar();
            }
        }
        public static void ExecuteInsertStatusHistory(WellStatus[] statusHistory, int wellId)
        {
            string sqlStatusHistory = SqlQuerry.InsertStatusHistory(statusHistory, wellId);
            if (sqlStatusHistory == null)
                return;
            using (SqlCommand command = new SqlCommand(sqlStatusHistory, sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static bool ExecuteDeleteWell(int wellId)
        {
            string sqlDelete = SqlQuerry.DeleteWell(wellId);
            if (sqlDelete == null)
                return false;
            using (SqlCommand command = new SqlCommand(sqlDelete, sqlConnection))
            {
                int affected = command.ExecuteNonQuery();
                if (affected > 0)
                    return true;
            }
            return false;
        }
        public static int ExecuteUpdatePart(Part part)
        {
            string sqlPart = SqlQuerry.UpdatePart(part);
            if (sqlPart == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlPart, sqlConnection))
            {
                return command.ExecuteNonQuery();
            }
        }
        public static int ExecuteUpdateWellType(WellType wellType)
        {
            string sqlWellType = SqlQuerry.UpdateWellType(wellType);
            if (sqlWellType == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlWellType, sqlConnection))
            {
                return command.ExecuteNonQuery();
            }
        }   
        public static int ExecuteUpdateFundingInfo(FundingInfo fundingInfo)
        {
            string sqlFundingInfo = SqlQuerry.UpdateFundingInfo(fundingInfo);
            if (sqlFundingInfo == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlFundingInfo, sqlConnection))
            {
                return command.ExecuteNonQuery();
            }
        }
        public static int ExecuteUpdateLocation(Location location)
        {
            string sqlLocation = SqlQuerry.UpdateLocation(location);
            if (sqlLocation == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlLocation, sqlConnection))
            {
                return command.ExecuteNonQuery();
            }
        }
        public static int ExecuteUpdateWell(Well well)
        {
            string sqlWell = SqlQuerry.UpdateWell(well);
            if (sqlWell == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlWell, sqlConnection))
            {
                return command.ExecuteNonQuery();
            }
        }
        public static int ExecuteUpdateStatusHistory(WellStatus statusHistory, int wellId)
        {
            string sqlStatusHistory = SqlQuerry.UpdateStatusHistory(statusHistory, wellId);
            if (sqlStatusHistory == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlStatusHistory, sqlConnection))
            {
                return command.ExecuteNonQuery();
            }
        }
        
        
        // Issue

        // Mutiple SQL Querries

        public static Issue GetCompleteIssue(int issueId)
        {
            Issue issue = ExecuteSelectIssue(issueId);
            if (issue != null || issue.Id != 0)
                issue.BrokenParts = ExecuteSelectBrokenParts(issue.Id);
            return issue;
        }
        public static bool AddCompleteNewIssue(Issue issue)
        {
            int IssueId = ExecuteInsertIssue(issue);
            if (IssueId == 0)
                return false;
            if (issue.BrokenParts != null)
            {
                List<int> partIds = new List<int>();
                foreach (Part part in issue.BrokenParts)
                {
                    partIds.Add(part.Id);
                }
                ExecuteInsertBrokenParts(partIds.ToArray(), issue.Id);
            }

            string status = ExecuteSelectWellStatus(issue.WellId);
            if (status != null && status == "green")
            {
                if (issue.ConfirmedBy == null || issue.ConfirmedBy == "")
                    ExecuteUpdateWellStatus(issue.WellId, "yellow");
                else if (issue.Works == false && status != "red")
                {
                    ExecuteUpdateWellStatus(issue.WellId, "red");
                }
            }
            WellStatus wellStatus = new WellStatus
            {
                Description = $"Issue #{issue.Id} Create",
                Works = issue.Works,
                Confirmed = false
            };
            if (issue.ConfirmedBy != null && issue.ConfirmedBy.Length > 0)
                wellStatus.Confirmed = true;
            WellStatus[] statusHistory = new WellStatus[1];
            statusHistory[0] = wellStatus;
            ExecuteInsertStatusHistory(statusHistory, issue.Id);
            return true;
        }
        public static void UpdateCompleteIssue(Issue issue)
        {
            if (issue.Id == 0)
                return;
            ExecuteUpdateIssue(issue);
            //Update well
            if (!issue.Open)
            {
                SmallIssue[] smallIssues = ExecuteSelectSmallIssues();
                bool allClosed = true;
                foreach (SmallIssue smallIssue in smallIssues)
                {
                    Issue otherIssue = ExecuteSelectIssue(smallIssue.Id);
                    if (otherIssue.Open)
                    {
                        allClosed = false;
                        break;
                    }
                }
                if (allClosed)
                    ExecuteUpdateWellStatus(issue.WellId, "green");
            }
            else if (issue.ConfirmedBy == null || issue.ConfirmedBy == "")
            {
                SmallIssue[] smallIssues = ExecuteSelectSmallIssues();
                bool NoConfirmedIssues = true;
                foreach (SmallIssue smallIssue in smallIssues)
                {
                    Issue otherIssue = ExecuteSelectIssue(smallIssue.Id);
                    if (otherIssue.Works == false && otherIssue.ConfirmedBy != null && otherIssue.ConfirmedBy.Length > 0)
                    { 
                        NoConfirmedIssues = false;
                        break;
                    }
                }
                if (NoConfirmedIssues)
                    ExecuteUpdateWellStatus(issue.WellId, "yellow");
            }
            else if (issue.Works == false)
            {
                ExecuteUpdateWellStatus(issue.WellId, "red");
            }

            WellStatus wellStatus = new WellStatus
            {
                Description = $"Issue #{issue.Id} Updated",
                Works = issue.Works,
                Confirmed = false
            };
            if (issue.ConfirmedBy != null && issue.ConfirmedBy.Length > 0)
                wellStatus.Confirmed = true;
            WellStatus[] statusHistory = new WellStatus[1];
            statusHistory[0] = wellStatus;
            ExecuteInsertStatusHistory(statusHistory, issue.Id);
        }

        // Single SQL Querry

        public static SmallIssue[] ExecuteSelectSmallIssues()
        {
            string sqlSmallIssue = SqlQuerry.SelectSmallIssue();
            if (sqlSmallIssue == null)
                return null;
            List<SmallIssue> smallIssues = new List<SmallIssue>();
            using (SqlCommand command = new SqlCommand(sqlSmallIssue, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SmallIssue smallIssue = new SmallIssue
                        {
                            Id = reader.GetInt32(0)
                        };
                        if (!reader.IsDBNull(1))
                            smallIssue.CreationDate = reader.GetDateTime(1);
                        if (!reader.IsDBNull(2))
                            smallIssue.WellId = reader.GetInt32(2);
                        smallIssues.Add(smallIssue);
                    }
                }
            }
            return smallIssues.ToArray();
        }
        public static Issue ExecuteSelectIssue(int issueId)
        {
            Issue issue = new Issue();
            string sqlIssue = SqlQuerry.SelectIssue(issueId);
            if (sqlIssue == null)
                return null;
            using (SqlCommand command = new SqlCommand(sqlIssue, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        issue.Id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            issue.WellId = reader.GetInt32(1);
                        if(!reader.IsDBNull(2))
                            issue.Description = reader.GetString(2);
                        if (!reader.IsDBNull(3))
                            issue.CreationDate = reader.GetDateTime(3);
                        if (!reader.IsDBNull(4))
                            issue.Status = reader.GetString(4);
                        if (!reader.IsDBNull(5))
                            issue.Open = reader.GetBoolean(5);
                        if (!reader.IsDBNull(6))
                            issue.ConfirmedBy = reader.GetString(6);
                        if (!reader.IsDBNull(7))
                            issue.SolvedDate = reader.GetDateTime(7);
                        if (!reader.IsDBNull(8))
                            issue.RepairedBy = reader.GetString(8);
                        if (!reader.IsDBNull(9))
                            issue.Works = reader.GetBoolean(9);
                    }
                }
            }
            return issue;
        }
        public static Part[] ExecuteSelectBrokenParts(int issueId)
        {
            string sqlBrokenParts = SqlQuerry.SelectBrokenParts(issueId);
            if (sqlBrokenParts == null)
                return null;
            List<Part> parts = new List<Part>();
            using (SqlCommand command = new SqlCommand(sqlBrokenParts, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Part part = new Part
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2)
                        };
                        parts.Add(part);
                    }
                }
            }
            return parts.ToArray();
        }      
        public static int ExecuteInsertIssue(Issue issue)
        {
            string sqlInsertIssue = SqlQuerry.InsertIssue(issue);
            if (sqlInsertIssue == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlInsertIssue, sqlConnection))
            {
                try
                {
                    return (int)command.ExecuteScalar();
                }
                catch
                {
                    return 0;
                }
            }
        }
        public static void ExecuteInsertBrokenParts(int[] partIds, int issueId)
        {
            string sqlBrokenParts = SqlQuerry.InsertBrokenParts(partIds, issueId);
            if (sqlBrokenParts == null)
                return;
            using (SqlCommand command = new SqlCommand(sqlBrokenParts, sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static void ExecuteUpdateIssue(Issue issue)
        {
            string sqlUpdateIssue = SqlQuerry.UpdateIssue(issue);
            if (sqlUpdateIssue == null)
                return;
            using (SqlCommand command = new SqlCommand(sqlUpdateIssue, sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static void ExecuteDeleteIssue(int issueId)
        {
            string sqlDeleteIssue = SqlQuerry.DeleteIssue(issueId);
            if (sqlDeleteIssue == null)
                return;
            using (SqlCommand command = new SqlCommand(sqlDeleteIssue, sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static string ExecuteSelectWellStatus(int wellId)
        {
            string sqlSelectWellStatus = SqlQuerry.SelectWellStatus(wellId);
            if (sqlSelectWellStatus == null)
                return null;
            using (SqlCommand command = new SqlCommand(sqlSelectWellStatus, sqlConnection))
            {
                return (string)command.ExecuteScalar();
            }
        }
        public static int ExecuteUpdateWellStatus(int wellId, string status)
        {
            string sqlUpdateWellStatus = SqlQuerry.UpdateWellStatus(wellId, status);
            if (sqlUpdateWellStatus == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlUpdateWellStatus, sqlConnection))
            {
                return command.ExecuteNonQuery();
            }
        }
    }
}
