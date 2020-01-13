library well_control.well_list;

import 'dart:convert';

import 'package:flutter_map/flutter_map.dart';
import 'package:well_control/WellMarker.dart';
import 'Functions.dart';

List<WellMarker> wells = <WellMarker>[
];


/// Returns all markers requested from DB
Future<List<Marker>> getMarkers() {
  // request wells
  return getAllWells().then((response)
  {
    print(response.statusCode);
    Iterable result = json.decode(response.body);
    var resultList = result.toList();
    print('Result ' + resultList.toString());
    for (var i = 0; i < resultList.length; i++) {

      wells.add(WellMarker(
          resultList[i]["name"],
          "green",
          resultList[i]["location"]["latitude"],
          resultList[i]["location"]["longitude"]));
    }
    List<Marker> markers = new List(wells.length);
    for (var i = 0; i < wells.length; i++) {
      markers[i] = wells[i].marker;
    }

    return markers;
  });

}
