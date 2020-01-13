import 'package:http/http.dart' as http;


/// Makes a POST request to save a new issue to the database.
/// @param description The issue's description
Future<http.Response> postNewIssue(String description) {
  return http.post(new Uri.http('https://wellapi.azurewebsites.net/api/Issue/PostNewIssue', description));
}

/// Gets all issues
Future<http.Response> getAllIssues() {
  return http.get('https://wellapi.azurewebsites.net/api/Issue/GetAll');
}

/// Gets all wells
Future<http.Response> getAllWells() {
  return http.get('https://wellapi.azurewebsites.net/api/Well/GetAll');
}

/// Gets a specific well
/// @param id The specific well ID
Future<http.Response> getWell(int id) {
  return http.get('https://wellapi.azurewebsites.net/api/Well/GetWell/$id');
}

/// Gets nearby wells
/// @param searchRadius A JSON String including searchRadius and current Position.
Future<http.Response> getNearbyWells(String searchRadius) {
  return http.post(
      'https://wellapi.azurewebsites.net/api/Well/GetNearbyWells/$searchRadius');
}

/// Saves a new well
/// @param well A JSON String with all current available well information.
Future<http.Response> postNewWell(String well) {
  return http.post(
      'https://wellapi.azurewebsites.net/api/Well/PostNewWell/$well');
}

/// Updates the information of a specific well
/// @param well A JSON String including the updated well information.
Future<http.Response> postUpdateWell(String well) {
  return http.post(
      'https://wellapi.azurewebsites.net/api/Well/PostUpdateWell/$well');
}

/// Deletes a specific well
/// @param id The specific well ID
Future<http.Response> deleteWell(int id) {
  return http.get('https://wellapi.azurewebsites.net/api/Well/DeleteWell/$id');
}