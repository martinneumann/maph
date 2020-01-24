library well_control.issue_list;

import 'dart:convert';

import 'package:well_control/Functions.dart';
import 'package:well_control/WellIssue.dart';


/// Issue list
List<WellIssue> issues = <WellIssue>[];


/// Get all global issues
Future<List<WellIssue>> getIssues() {
  return getAllIssues().then((response) {
  // fill list
  Iterable result = json.decode(response.body);
  var resultList = result.toList();
  print('Result ' + resultList.toString());
  for (var i = 0; i < resultList.length; i++) {

    issues.add(WellIssue(
        resultList[i]["id"],
        resultList[i]["wellId"],
        resultList[i]["description"],
        resultList[i]["creationDate"],
        resultList[i]["status"],
        resultList[i]["open"]));
  }

  return issues;
});
}

/// Get issues of specific well
Future <List<WellIssue>> getIssuesOfWell(String wellId) {
  return getWellIssues(wellId).then((response) {
    print('Response with code ' + response.statusCode.toString() + ' in getIssuesOfWell ' + response.body);
    List<WellIssue> testing;

    testing = (json.decode(response.body) as List).map((i) =>
        WellIssue.fromJson(i)).toList();

    print("List: " + testing.toString());
    return testing;
  }).catchError((error) {
    print("An error happened while fetching issue data: " + error);
    return error;
  });
}

