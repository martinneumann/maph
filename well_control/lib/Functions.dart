import 'package:http/http.dart' as http;

import 'WellIssue.dart';


/// Makes a POST request to save a new issue to the database.
/// @param issue The issue to be posted.
Future<http.Response> postNewIssue(WellIssue issue) {
  print("Param issue description: " + issue.description.toString());
  var query = "{ id: ${issue.id.toString()}, description: ${issue.description.toString()}, image: ${issue.image.toString()}, "
      "creationDate: ${issue.creationDate.toIso8601String()}, status: ${issue.status.toString()}, open: ${issue.open.toString()}, "
      "confirmedBy: ${issue.confirmedBy.toString()}, solvedDate: ${issue.solvedDate.toIso8601String()}, repairedBy: ${issue.repairedBy.toString()}, "
      "bill: ${issue.bill.toString()}, works: ${issue.works.toString()}, brokenParts: ${issue.brokenParts.toString()}, wellId: ${issue.wellId.toString()}";
  print(query);
  return http.post(new Uri.http('https://wellapi.azurewebsites.net/api/Issue/PostNewIssue', query));
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
Future<http.Response> postNewWell(var body) {
  return http.post(
      'https://wellapi.azurewebsites.net/api/Well/PostNewWell/',
      headers: {"Content-Type": "application/json"},
      body: body);
}

/// Updates the information of a specific well
/// @param well A JSON String including the updated well information.
Future<http.Response> postUpdateWell(int id, var body) {
  return http.post(
      'https://wellapi.azurewebsites.net/api/Well/PostUpdateWell/$id',
      headers: {"Content-Type": "application/json"},
      body: body);
}

/// Deletes a specific well
/// @param id The specific well ID
Future<http.Response> deleteWell(int id) {
  return http.get('https://wellapi.azurewebsites.net/api/Well/DeleteWell/$id');
}
