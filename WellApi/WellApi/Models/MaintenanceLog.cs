﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellApi.Models
{
    public class MaintenanceLog
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public bool? Works { get; set; }
        public bool? Confirmed { get; set; }
        public DateTime? StatusChangedDate { get; set; }
    }

    public class UpdateMaintenanceLog
    {
        public MaintenanceLog MaintenanceLog { get; set; }
        public int? WellId { get; set; }
    }

    public class InsertMaintenanceLog
    {
        public MaintenanceLog MaintenanceLog { get; set; }
        public int? WellId { get; set; }
    }
}
