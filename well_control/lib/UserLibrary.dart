/// Simulates account of technician.
bool technician = false;

/// Simulates account of admin.
bool admin = true;

/// Simulates account of user.
bool basicUser = false;

/// Returns the currently active user as a string.
///
/// At startup, user is set to "Admin".
getActiveUser() {
  if (technician) return "Technician";
  if (admin) return "Admin";
  if (basicUser) return "Standard User";
}
