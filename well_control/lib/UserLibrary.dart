bool technician = false;
bool admin = false;
bool basicUser = false;

/// Returns the currently active user as a string.
/// At startup on the map scree, user is set to "BasicUser".
getActiveUser() {
  if (technician) return "Technician";
  if (admin) return "Admin";
  if (basicUser) return "Standard User";
}
