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
            if (parts.Length == 0)
            {
                return null;
            }
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
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [well].[dbo].[WellType] (Name, Particularity,Depth)");
            sb.Append("OUTPUT INSERTED.Id");
            sb.Append($"VALUES ('{wellType.Name}','{wellType.Particularity}',{wellType.Depth});");
            return sb.ToString();
        }
    }
}
