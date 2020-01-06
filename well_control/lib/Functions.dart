import 'package:http/http.dart' as http;


/// Makes a POST request to save a new issue to the database.
/// @param description The issue's description
Future<http.Response> postNewIssue(String description) {
  return http.post('https://wellapi.azurewebsites.net/api/Issue/PostNewIssue/' + description);
}

/// Gets all issues
Future<http.Response> getAllIssues() {
  return http.get('https://wellapi.azurewebsites.net/api/Issue/GetAll');
}