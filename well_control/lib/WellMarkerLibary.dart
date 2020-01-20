library well_control.well_list;

import 'dart:convert';

import 'package:flutter_map/flutter_map.dart';
import 'package:well_control/WellMarker.dart';

import 'Functions.dart';

/// Well list
List<WellMarker> wells = <WellMarker>[
];

/// Returns all markers requested from DB and saves them in the global
/// well list.
Future<List<Marker>> getMarkers() {
  // request wells
  return getAllWells().then((response)
  {
    print(response.statusCode);
    Iterable result = json.decode(response.body);
    var resultList = result.toList();
    print('Result ' + resultList.toString());
    double test = 0.0;
    for (var i = 0; i < resultList.length; i++) {
      print("ResultList: " + resultList[i].toString());
      test = resultList[i]["location"]["latitude"];
      print("test was: " + test.toString());
      wells.add(WellMarker(
          resultList[i]["name"].toString(),
          resultList[i]["wellId"],
          resultList[i]["status"].toString(),
          resultList[i]["location"]["latitude"],
          resultList[i]["location"]["longitude"]));
    }
    List<Marker> markers = new List(wells.length);
    print("Markers " + markers[0].toString());
    for (var i = 0; i < wells.length; i++) {
      markers[i] = wells[i].marker;
    }
    print("Markers " + markers[0].toString());

    return markers;
  }).catchError((error) {
    print(error.toString());
  });
}
