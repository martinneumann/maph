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
          resultList[i]["open"],
          resultList[i]["works"],
          resultList[i]["confirmedBy"]));
    }

    return issues;
  });
}

/// Get issues of specific well
Future<List<WellIssue>> getIssuesOfWell(String wellId) {
  return getWellIssues(wellId).then((response) {
    Iterable decodedResult = json.decode(response.body);
    List<WellIssue> issueList = [];
    for (var issue in decodedResult) {
      var tempIssue = new WellIssue(
          issue["id"],
          issue["wellId"],
          issue["description"],
          issue["creationDate"],
          issue["status"],
          issue["open"],
          issue["works"],
          issue["confirmedBy"]);
      issueList.add(tempIssue);
    }
    return issueList;
  }).catchError((error) {
    print("An error happened while fetching issue data: " + error);
    return error;
  });
}


/// Get issues of specific well
Future<List<WellIssue>> getOpenIssuesOfWell(String wellId) {
  return getWellIssues(wellId).then((response) {
    Iterable decodedResult = json.decode(response.body);
    List<WellIssue> issueList = [];
    for (var issue in decodedResult) {
      if (issue["open"] == true) {
        var tempIssue = new WellIssue(
            issue["id"],
            issue["wellId"],
            issue["description"],
            issue["creationDate"],
            issue["status"],
            issue["open"],
            issue["works"],
            issue["confirmedBy"]);
        issueList.add(tempIssue);
      }
    }
    return issueList;
  }).catchError((error) {
    print("An error happened while fetching issue data: " + error);
    return error;
  });
}


/// Get one specific issue by Id
Future<WellIssue> getSpecificIssue(int id) {
  return getIssueById(id).then((response) {
    WellIssue decodedResult = json.decode(response.body);
    WellIssue tempIssue = new WellIssue(
        decodedResult.id,
        decodedResult.wellId,
        decodedResult.description,
        decodedResult.creationDate,
        decodedResult.status,
        decodedResult.open,
        decodedResult.works,
        decodedResult.confirmedBy
    );
    return tempIssue;
  }).catchError((error) {
    return error;
  });
}
