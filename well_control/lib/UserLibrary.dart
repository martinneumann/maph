bool technician = false;
bool admin = false;
bool basicUser = false;

getActiveUser() {
  if (technician) return "Technician";
  if (admin) return "Admin";
  if (basicUser) return "Basic User";
}
