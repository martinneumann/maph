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
        resultList[i]["description"],
        resultList[i]["creationDate"],
        resultList[i]["image"],
        resultList[i]["status"],
        resultList[i]["open"],
        resultList[i]["brokenParts"],
        resultList[i]["confirmedBy"],
        resultList[i]["solvedDate"],
        resultList[i]["repairedBy"],
        resultList[i]["bill"],
        resultList[i]["works"]));
  }

  return issues;
});
}

