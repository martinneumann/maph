bool technician = false;
bool admin = true;
bool basicUser = false;

/// Returns the currently active user as a string.
/// At startup, user is set to "Admin".
getActiveUser() {
  if (technician) return "Technician";
  if (admin) return "Admin";
  if (basicUser) return "Standard User";
}
