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

    Iterable decodedResult = json.decode(response.body);
    print("\n Decoded: " + decodedResult.toString());
    // var decodedList = decodedResult as List;
    // print("\nAs list: " + decodedList.toString());
    // testing = decodedResult.map((i) =>
    //       WellIssue.fromJson(i)).toList();
    // print("Well Issue List: " + testing.toString());
    // List<WellIssue> issueList = decodedResult.map((model) => WellIssue.fromJson(model)).toList();
    List<WellIssue> issueList = [];
    for (var issue in decodedResult) {
      print("Looping through current issue: " + issue.toString());
      var tempIssue = new WellIssue(issue["id"], issue["wellId"], issue["description"], issue["creationDate"], issue["status"], issue["open"]);
      print("Created issue: " + tempIssue.toString());
      issueList.add(tempIssue);
    }
    print("\n List WellIssue: " + issueList.toString());
    return issueList;
  }).catchError((error) {
    print("An error happened while fetching issue data: " + error);
    return error;
  });
}

