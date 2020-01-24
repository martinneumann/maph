import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:flutter_map_marker_cluster/flutter_map_marker_cluster.dart';
import 'package:map_controller/map_controller.dart';
import 'package:latlong/latlong.dart';
import 'package:location/location.dart';
import 'package:well_control/AddWell.dart';
import 'package:well_control/ReportWell.dart';
import 'package:well_control/Settings.dart';
import 'package:well_control/WellOverview.dart';
import 'dart:async';

import 'WellMarkerLibary.dart' as wellList;
/// Class create view of map
///
/// Map shows wells as markers with different colors.
/// Also user can set current position on map.
/// Map is provided by mapbox.
class WellMap extends StatefulWidget {
  /// Constructor initializes content of view.
  WellMap({Key key, this.title}) : super(key: key);
  /// Title of view.
  final String title;

  @override
  _WellMapState createState() => _WellMapState();
}
/// Initializes state of [WellMap] class.
///
/// Includes all variables and view to show map.
class _WellMapState extends State<WellMap> {
  /// Controller moves map position by given user location.
  MapController mapController;
  /// Gets user location from GPS.
  Location userLocation;
  /// Controller updates view of markers.
  StatefulMapController statefulMapController;
  /// Updates marker data.
  StreamSubscription<StatefulMapControllerStateChange> sub;
  /// Checks [statefulMapController] is ready for update.
  bool ready = false;
  /// Stores well markers and user location marker to show these on map.
  Future<Map<String,Marker>> markerMap = wellList.getMarkersMap();
  /// Stores menu item title for well list.
  static const listWells = "List of wells";
  /// Stores menu item title for adding well.
  static const addWell = "Add Well";
  /// Stores menu item title for settings.
  static const settings = "Settings";
  /// Stores menu item title for reporting malfunction.
  static const report = "Report Malfunction";
  /// Stores menu item titles.
  static const List<String> menuChoices = <String>[
    listWells,
    addWell,
    report,
    settings
  ];

  @override
  void initState() {
    mapController = MapController();
    userLocation = Location();
    statefulMapController = StatefulMapController(mapController: mapController);
    statefulMapController.onReady.then((_) => setState(() => ready = true));
    sub = statefulMapController.changeFeed.listen((change) => setState(() {}));
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        appBar: AppBar(
          title: Text(widget.title),
          actions: <Widget>[
            PopupMenuButton<String>(
              onSelected: choiceAction,
              itemBuilder: (BuildContext context) {
                return menuChoices.map((String choice) {
                  return PopupMenuItem<String>(
                    value: choice,
                    child: Text(choice),
                  );
                }).toList();
              },
            )
          ],
        ),
        body: Center(
          child: FutureBuilder<Map<String,Marker>>(
            future: markerMap,
            builder:
                (BuildContext context, AsyncSnapshot<Map<String,Marker>> snapshot) {
              //print("WellMap data: " + snapshot.data.toString());
              if (snapshot.hasData) {
                statefulMapController.addMarkers(markers: snapshot.data);
                return Center(
                    child: Container(
                      child: FlutterMap(
                        options: MapOptions(
                        center: LatLng(6.071891, 38.785878),
                        zoom: 12.0,
                            plugins: [
                              MarkerClusterPlugin(),
                            ]),
                        layers: [
                          TileLayerOptions(
                            urlTemplate:
                            'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
                            subdomains: ['a', 'b', 'c'],
                            additionalOptions: {'access_token': '', 'id': ''},
                          ),
                          MarkerClusterLayerOptions(
                            maxClusterRadius: 120,
                            size: Size(40, 40),
                            fitBoundsOptions: FitBoundsOptions(
                              padding: EdgeInsets.all(50),
                            ),
                            markers: statefulMapController.markers,
                            polygonOptions: PolygonOptions(
                                borderColor: Colors.blueAccent,
                                color: Colors.black12,
                                borderStrokeWidth: 3),
                            builder: (context, markers) {
                              return FloatingActionButton(
                                child: Text(markers.length.toString()),
                                onPressed: null,
                                heroTag: "clusterButton",
                              );
                            },
                          ),
                        ],
                        mapController: mapController,
                      ),
                    ));
              } else {
                return Text("Loading...");
              }
            },
          ),
        ),
        floatingActionButton: FloatingActionButton(
          onPressed: () {
            _getLocation().then((value) {
              setState(() {
                setUserLocation(value);
              });
            });
          },
          child: Icon(Icons.gps_fixed),
          backgroundColor: Colors.blue,
        ),
      ),
    );
  }

  /// Methods defines action of clicked menu item.
  ///
  /// Opens certain view by comparing clicked [choice] with menu list names.
  void choiceAction(String choice) {
    if (choice == settings) {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => Settings(title: "Settings")));
    } else if (choice == addWell) {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => AddWell(title: "Add Well")));
    } else if (choice == listWells) {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => WellOverview(title: "List of Wells")));
    } else {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => ReportWell(title: "Report Malfunction")));
    }
  }

  @override
  void dispose() {
    sub.cancel();
    super.dispose();
  }

  /// Method loads user location data.
  ///
  /// Returns the location data as map.
  Future<Map<String, double>> _getLocation() async {
    var currentLocation = <String, double>{};
    try {
      currentLocation = await userLocation.getLocation();
    } catch (e) {
      currentLocation = null;
    }
    return currentLocation;
  }

  /// Sets user location on map.
  ///
  /// Sets location data and add marker for user position on map.
  void setUserLocation(Map<String, double> userLocation) {
    if (userLocation != null) {
      LatLng location =
        LatLng(userLocation['latitude'], userLocation['longitude']);

      markerMap = wellList.getWellMarkersByRadius(userLocation['latitude'],
          userLocation['longitude'], 400);

      wellList.setUserPositionMarker(location);
      mapController.move(location, 14);
    }
  }
}