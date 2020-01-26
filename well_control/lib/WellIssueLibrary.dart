library well_control.issue_list;

import 'dart:convert';

import 'package:well_control/Functions.dart';
import 'package:well_control/WellIssue.dart';


/// Get all global issues. Returns a Future that resolves to an issue list.
Future<List<WellIssue>> getIssues() {
  return getAllIssues().then((response) {
    // fill list
    Iterable result = json.decode(response.body);
    List<WellIssue> issues = <WellIssue>[];
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
          resultList[i]["confirmedBy"],
          resultList[i]["brokenParts"]));
    }

    return issues;
  });
}

/// Get issues of specific well. Returns a Future that resolves to an issue list for one specific well.
/// [wellId] The well the issues should be returned for.
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
          issue["confirmedBy"],
          []);
      issueList.add(tempIssue);
    }
    return issueList;
  }).catchError((error) {
    print("An error happened while fetching issue data: " + error);
    return error;
  });
}


/// Get open issues of specific well. Returns a Future that resolves to an issue list
/// of only open issues of that specific well.
/// [wellId] The well's Id.
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
            issue["confirmedBy"],
            []);
        issueList.add(tempIssue);
      }
    }
    return issueList;
  }).catchError((error) {
    print("An error happened while fetching issue data: " + error);
    return error;
  });
}


/// Get one specific issue by Id. Returns a Future that resolves to one specific issue.
/// [id] The issue's id.
Future<WellIssue> getSpecificIssue(int id) {
  print("Getting specific issue");
  return getIssueById(id).then((response) {
    var decodedResult = json.decode(response.body);
    Part tempPart = new Part();
    List<Part> partList = [];
    print(decodedResult.toString());
    if (decodedResult["brokenParts"].length != 0) {
      print("Parts " + decodedResult["brokenParts"].toString());
      tempPart.name = decodedResult["brokenParts"][0]["name"];
      tempPart.description = decodedResult["brokenParts"][0]["description"];
      tempPart.id = decodedResult["brokenParts"][0]["id"];
      partList.add(tempPart);
    } else {
      print("Broken parts is null");

    }
    WellIssue tempIssue = new WellIssue(
        decodedResult["id"],
        decodedResult["wellId"],
        decodedResult["description"],
        decodedResult["creationDate"],
        decodedResult["status"],
        decodedResult["open"],
        decodedResult["works"],
        decodedResult["confirmedBy"],
        partList,
    );
    return tempIssue;
  }).catchError((error) {
    print("An error happend in specific issue: " + error.toString());
    return error;
  });
}
