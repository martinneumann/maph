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
    print(json.decode(response.body));
    Iterable result = json.decode(response.body);
    var resultList = result.toList();
    print('Result ' + resultList.toString());
    for (var i = 0; i < resultList.length; i++) {

      wells.add(WellMarker(
          resultList[i]["id"],
          resultList[i]["name"],
          resultList[i]["status"],
          resultList[i]["location"]["latitude"],
          resultList[i]["location"]["longitude"]));

      wells[i].setId(resultList[i]["id"]);
    }
    List<Marker> markers = new List(wells.length);
    for (var i = 0; i < wells.length; i++) {
      markers[i] = wells[i].marker;
    }

    if (markers.length == 0) {
      print("No markers retrieved, posting dummy.");
      var dummyMarker = new List(1);
      dummyMarker.add(WellMarker(1, "TestMarker", "green", 10, 10));
      List<Marker> dummyMarkers = new List(1);
      return dummyMarkers;
    } else {
      print("Received valid markers.");
      return markers;

    }
  });

}
