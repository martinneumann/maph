import 'dart:convert';

import 'package:http/http.dart' as http;
import 'package:well_control/WellIssue.dart';

/// Defines the api url as const variable [wellApiUrl].
const wellApiUrl = "https://wellapi.azurewebsites.net/api/";

/// Post request for new well issue
///
/// Makes a POST request to save a new [issue] to the database.
Future<http.Response> postNewIssue(var body) {

  return http.post(wellApiUrl + 'Issue/CreateIssue',
      headers: {"Content-Type": "application/json"},
      body: body);
}

/// Response returns all well issues as [http.Response]
///
/// Sends get-request to get all created well issues.
Future<http.Response> getAllIssues() {
  return http.get(wellApiUrl + 'Issue/GetAll');
}

/// Get one specific issue by Id.
Future<http.Response> getIssueById(int id) {
  return http.get(wellApiUrl + 'Issue/GetIssue/$id');
}

/// Gets one well's issues
Future<http.Response> getWellIssues(String wellId) {
  print("Getting issue with: "  + wellId.toString());
  return http.get('https://wellapi.azurewebsites.net/api/Issue/GetIssuesFromWell/$wellId');
}

/// Set issue to closed
Future<http.Response> closeIssue(int id) {
  var body = ' { "id": $id, "open": false }';
  return http.post(wellApiUrl + 'Issue/UpdateIssue',
      headers: {"Content-Type": "application/json"},
      body: body);
}

/// Update issue data
Future<http.Response> updateIssue(WellIssue issue) {
  var data = {};
  data["id"] = issue.id;
  data["wellId"] = issue.wellId;
  data["name"] = issue.name;
  data["description"] = issue.description;
  data["status"] = issue.status;
  data["creationDate"] = issue.creationDate;
  data["image"] = issue.image;
  data["open"] = issue.open;
  data["works"] = issue.works;
  data["brokenParts"] = issue.brokenParts;
  data["wellType"] = issue.wellType;
  data["confirmedBy"] = issue.confirmedBy;
  data["fundingInfo"] = issue.fundingInfo;
  data["solvedDate"] = issue.solvedDate;
  data["repairedBy"] = issue.repairedBy;
  data["bill"] = issue.bill;
  var body = json.encode(data);
  return http.post(wellApiUrl + '/Issue/UpdateIssue',
      headers: {"Content-Type": "application/json"},
      body: body);
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
/// Filtering wells by location latitude, longitude and given radius
/// in meter.
Future<http.Response> getWellsByRadius(var body) {
  return http.post(wellApiUrl + 'Well/GetNearbyWells',
      headers: {"Content-Type": "application/json"}, body: body);
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

/// Response returns all wellTypes by [http.Response] message.
///
/// Sends get-request to get all well types.
Future<http.Response> getAllWellTypes() {
  return http.get(wellApiUrl + 'Well/GetAllWellTypes');
}
