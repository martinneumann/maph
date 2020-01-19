using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text;
using System.IO;

namespace WellApi
{
    public class DB
    {
        // Connection

        static SqlConnection sqlConnection = null;
        public static void ConnectToDb()
        {
            string connectionString = "Server=tcp:wellhtw.database.windows.net,1433;Initial Catalog=well;Persist Security Info=False;User ID=htw;Password=maph2019!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
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
        public static bool AddCompleteWell(Well well)
        {
            if (well == null)
                return false;
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
                return false;
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
            ExecuteStatusHistory(well.StatusHistory, well.Id);
            return true;
        }
        public static void UpdateCompleteWell(Well well)
        {
            if (well == null)
                return;
            if (well.WellType != null)
            {
                ExecuteUpdateParts(well.WellType.Parts);
            }
            ExecuteUpdateWellType(well.WellType);
            ExecuteUpdateFundingInfo(well.FundingInfo);
            ExecuteUpdateLocation(well.Location);
            ExecuteUpdateWell(well);
            ExecuteUpdateStatusHistory(well.StatusHistory, well.Id);

        }

        // Single SQL Querry

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
                            Status = reader.GetString(2)
                        };
                        smallWell.Location.Longitude = reader.GetDouble(3);
                        smallWell.Location.Latitude = reader.GetDouble(4);
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
            Well well = new Well();
            using (SqlCommand command = new SqlCommand(sqlSelectWell, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        well.Id = reader.GetInt32(0);
                        well.Name = reader.GetString(1);
                        well.Image = null;
                        Stream real_Image = reader.GetStream(3);
                        well.Status = reader.GetString(4);
                        well.Location.Id = reader.GetInt32(5);
                        well.Location.Longitude = reader.GetDouble(6);
                        well.Location.Latitude = reader.GetDouble(7);
                        well.FundingInfo.Id = reader.GetInt32(8);
                        well.FundingInfo.Organisation = reader.GetString(9);
                        well.FundingInfo.OpeningDate = reader.GetDateTime(10);
                        well.FundingInfo.Price = reader.GetDouble(11);
                        well.WellType.Id = reader.GetInt32(12);
                        well.WellType.Name = reader.GetString(13);
                        well.WellType.Particularity = reader.GetString(14);
                        well.WellType.Depth = reader.GetDouble(15);
                    }
                }
            }
            return well;
        }     
        public static WellStatus[] ExecuteSelectStatusHistory(int wellId)
        {
            string sqlSelectStatusHistory = SqlQuerry.SelectStatusHistory(wellId);
            if (sqlSelectStatusHistory == null)
                return null;
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
        public static void ExecuteStatusHistory(WellStatus[] statusHistory, int wellId)
        {
            string sqlStatusHistory = SqlQuerry.InsertStatusHistory(statusHistory, wellId);
            if (sqlStatusHistory == null)
                return;
            using (SqlCommand command = new SqlCommand(sqlStatusHistory, sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static void ExecuteDeleteWell(int wellId)
        {
            string sqlDelete = SqlQuerry.DeleteWell(wellId);
            if (sqlDelete == null)
                return;
            using (SqlCommand command = new SqlCommand(sqlDelete, sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static void ExecuteUpdateParts(Part[] parts)
        {
            string sqlParts = SqlQuerry.UpdateParts(parts);
            if (sqlParts == null)
                return;
            using (SqlCommand command = new SqlCommand(sqlParts, sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static void ExecuteUpdateWellType(WellType wellType)
        {
            string sqlWellType = SqlQuerry.UpdateWellType(wellType);
            if (sqlWellType == null)
                return;
            using (SqlCommand command = new SqlCommand(sqlWellType, sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }   
        public static void ExecuteUpdateFundingInfo(FundingInfo fundingInfo)
        {
            string sqlFundingInfo = SqlQuerry.UpdateFundingInfo(fundingInfo);
            if (sqlFundingInfo == null)
                return;
            using (SqlCommand command = new SqlCommand(sqlFundingInfo, sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static void ExecuteUpdateLocation(Location location)
        {
            string sqlLocation = SqlQuerry.UpdateLocation(location);
            if (sqlLocation == null)
                return;
            using (SqlCommand command = new SqlCommand(sqlLocation, sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static void ExecuteUpdateWell(Well well)
        {
            string sqlWell = SqlQuerry.UpdateWell(well);
            if (sqlWell == null)
                return;
            using (SqlCommand command = new SqlCommand(sqlWell, sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static void ExecuteUpdateStatusHistory(WellStatus[] statusHistory, int wellId)
        {
            string sqlStatusHistory = SqlQuerry.UpdateStatusHistory(statusHistory, wellId);
            if (sqlStatusHistory == null)
                return;
            using (SqlCommand command = new SqlCommand(sqlStatusHistory, sqlConnection))
            {
                command.ExecuteNonQuery();
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
        public static void AddCompleteNewIssue(Issue issue)
        {
            int IssueId = ExecuteInsertIssue(issue);
            if (IssueId == 0)
                return;
            List<int> partIds = new List<int>();
            foreach (Part part in issue.BrokenParts)
            {
                partIds.Add(part.Id);
            }
            ExecuteInsertBrokenParts(partIds.ToArray(), issue.Id);
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
                            Id = reader.GetInt32(0),
                            CreationDate = reader.GetDateTime(1),
                            WellId = reader.GetInt32(2)
                        };
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
                        issue.WellId = reader.GetInt32(1);
                        issue.Description = reader.GetString(2);
                        issue.CreationDate = reader.GetDateTime(3);
                        issue.Image = null;
                        issue.Status = reader.GetString(5);
                        issue.Open = reader.GetBoolean(6);
                        issue.ConfirmedBy = reader.GetString(7);
                        issue.SolvedDate = reader.GetDateTime(8);
                        issue.RepairedBy = reader.GetString(9);
                        issue.Bill = null;
                        issue.Works = reader.GetBoolean(11);
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
                return (int)command.ExecuteScalar();
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
    }
}
