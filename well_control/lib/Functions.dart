import 'dart:convert';

import 'package:http/http.dart' as http;

import 'WellIssue.dart';

/// Defines the api url as const variable [wellApiUrl].
const wellApiUrl = "https://wellapi.azurewebsites.net/api/";

/// Post request for new well issue
///
/// Makes a POST request to save a new [issue] to the database.
Future<http.Response> postNewIssue(WellIssue issue) {
  print("Param issue description: " + issue.description.toString());
  var query = json.encode(
      "{ id: ${issue.id.toString()}, description: ${issue.description
          .toString()},"
      "creationDate: ${issue.creationDate.toString()}, status: ${issue.status.toString()}, open: ${issue.open.toString()}, "
      ")");
  print(query);
  return http.post(wellApiUrl + 'Issue/CreateIssue',
      body: query);
}

/// Response returns all well issues as [http.Response]
///
/// Sends get-request to get all created well issues.
Future<http.Response> getAllIssues() {
  return http.get(wellApiUrl + 'Issue/GetAll');
}


/// Gets one well's issues
Future<http.Response> getWellIssues(String wellId) {
  print("Getting issue with: "  + wellId.toString());
  return http.get('https://wellapi.azurewebsites.net/api/Issue/GetIssuesFromWell/$wellId');
}

/// Gets all wells
/// Response returns all wells as [http.Response]
///
/// Sends get-request to get all wells.
/// Wells consists name, id, status and location.
Future<http.Response> getAllWells() {
  return http.get(wellApiUrl + 'Well/GetAll');
}

/// Response returns wells filtered by radius in meter.
///
/// Sends post-request to get wells in radius.
/// Filtering wells by location [latitude], [longitude] and given radius
/// in meter.
Future<http.Response> getWellsByRadius(double latitude , double longitude , 
                                       int radius) {
  var query = jsonEncode("{" +
    "searchRadius:" + radius.toString() + "," +
    "location:{" +
      "latitude:" + latitude.toString() + "," +
      "longitude:" + longitude.toString() +
    "}}");

  return http.post(wellApiUrl + 'Well/GetNearbyWells',
      body: query);
}

/// Response returns specific well by given [id].
///
/// Sends get-request to get well once by given [id].
Future<http.Response> getWell(int id) {
  return http.get(wellApiUrl + 'Well/GetWell/$id');
}

/// Posts new well to database
///
/// The argument [body] includes wells data as JSON-string.
Future<http.Response> postNewWell(var body) {
  return http.post(wellApiUrl + 'Well/CreateWell/',
      headers: {"Content-Type": "application/json"}, body: body);
}

/// Posts data to update well data.
///
/// The argument [body] includes new wells data as JSON-string.
Future<http.Response> postUpdateWell(var body) {
  return http.post(
      wellApiUrl + 'Well/UpdateWell/',
      headers: {"Content-Type": "application/json"},
      body: body);
}

/// Deletes a specific well
///
/// Deletes well on database by given [id].
Future<http.Response> deleteWell(int id) {
  return http.delete(
      wellApiUrl + 'Well/DeleteWell/$id');
}

/// Response returns all wellTypes [http.Response]
///
/// Sends get-request to get all wellTypes.
Future<http.Response> getAllWellTypes() {
  return http.get(wellApiUrl + 'Well/GetAllWellTypes');
}
