import 'dart:convert';

import 'package:http/http.dart' as http;

import 'WellIssue.dart';

/// Makes a POST request to save a new issue to the database.
/// @param issue The issue to be posted.
Future<http.Response> postNewIssue(WellIssue issue) {
  print("Param issue description: " + issue.description.toString());
  var query = json.encode(
      "{ id: ${issue.id.toString()}, description: ${issue.description
          .toString()},"
      "creationDate: ${issue.creationDate}, status: ${issue.status.toString()}, open: ${issue.open.toString()}, "
      ")");
  return http.post('http://wellapi.azurewebsites.net/api/Issue/PostNewIssue',
      body: query);
}

/// Gets all issues
Future<http.Response> getAllIssues() {
  return http.get('https://wellapi.azurewebsites.net/api/Issue/GetAll');
}


/// Gets one well's issues
Future<http.Response> getWellIssues(String wellId) {
  print("Getting issue with: "  + wellId.toString());
  return http.get('https://wellapi.azurewebsites.net/api/Issue/GetIssue/$wellId');
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
  return http.post('https://wellapi.azurewebsites.net/api/Well/PostNewWell/',
      headers: {"Content-Type": "application/json"}, body: body);
}

/// Updates the information of a specific well
/// @param well A JSON String including the updated well information.
Future<http.Response> postUpdateWell(var body) {
  return http.post(
      'https://wellapi.azurewebsites.net/api/Well/PostUpdateWell/',
      headers: {"Content-Type": "application/json"},
      body: body);
}

/// Deletes a specific well
/// @param id The specific well ID
Future<http.Response> deleteWell(int id) {
  return http.delete(
      'https://wellapi.azurewebsites.net/api/Well/DeleteWell/$id');
}
