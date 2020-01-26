/// Simulates account of technician.
bool technician = false;

/// Simulates account of admin.
bool admin = false;

/// Simulates account of user.
bool basicUser = false;

getActiveUser() {
  if (technician) return "Technician";
  if (admin) return "Admin";
  if (basicUser) return "Basic User";
}
