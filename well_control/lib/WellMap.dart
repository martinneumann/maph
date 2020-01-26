import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:flutter_map_marker_cluster/flutter_map_marker_cluster.dart';
import 'package:latlong/latlong.dart';
import 'package:location/location.dart';
import 'package:map_controller/map_controller.dart';
import 'package:well_control/AddWell.dart';
import 'package:well_control/Settings.dart';
import 'package:well_control/WellOverview.dart';

import 'UserLibrary.dart' as users;
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

  /// Variable is necessary to create several [FloatingActionButton]
  /// with unique hero tag name.
  int floatingButtonNumber = 0;

  /// Stores status of located user location by gps.
  bool userLocated = false;

  /// Stores [defaultLocation] for map at beginning.
  LatLng defaultLocation = LatLng(6.071891, 38.785878);

  /// Stores [defaultZoom] for map at beginning.
  double defaultZoom = 2.0;

  /// Stores name of user location marker.
  String locationMarkerName = wellList.userLocationMarkerName;

  /// Stores [searchRadius] of nearby wells referred to user's position.
  int searchRadius = 400000;

  /// Stores color of gps icon at beginning.
  Color userLocationMarkerColor = Color.fromARGB(255, 200, 0, 0);

  /// Stores well markers and user location marker to show these on map.
  Future<Map<String,Marker>> markerMap = wellList.getMarkersMap();

  /// Stores menu item title for well list.
  static const listWells = "List of wells";

  /// Stores menu item title for adding well.
  static const addWell = "Add Well";

  /// Stores menu item title for settings.
  static const settings = "Settings";

  @override
  void initState() {
    users.basicUser = true;
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
          actions:
          setActions(),
        ),

        body: Center(
          child: FutureBuilder<Map<String,Marker>>(
            future: markerMap,
            builder: (BuildContext context, AsyncSnapshot<Map<String,Marker>> snapshot) {
              if (snapshot.hasData) {
                 updateMarkers(statefulMapController, snapshot.data);

                return Center(
                    child: Container(
                      child: FlutterMap(
                        options: MapOptions(
                            center: defaultLocation,
                            zoom: defaultZoom,
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
                              floatingButtonNumber++;
                              return FloatingActionButton(
                                child: Text(markers.length.toString()),
                                onPressed: null,
                                heroTag: "clusterButton" + floatingButtonNumber.toString(),
                              );
                            },
                          ),
                        ],
                        mapController: mapController,
                      ),
                    ));
              } else {
                return Center(
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: <Widget>[
                      CircularProgressIndicator(),
                      SizedBox(height: 50),
                      Text("Loading..."),
                    ],
                  ),
                );
              }
            },
          ),
        ),
        floatingActionButton: FloatingActionButton (
          onPressed: () {
            _getLocation().then((value) {
              setState(() {
                setUserLocation(value);
              });
            });
          },
          child: Icon(Icons.gps_fixed),
          backgroundColor: userLocationMarkerColor,
        ),
      ),
    );
  }

  ///Method returns PopupMenu.
  ///
  /// Items inside menu depend on current ative user.
  List<Widget> setActions() {
    if (users.admin) {
      return [PopupMenuButton(
        onSelected: (value) {
          choiceAction(value);
        },
        itemBuilder: (context) =>
        [
          PopupMenuItem(
            child: Text(listWells),
            value: listWells,
          ),
          PopupMenuItem(
            child: Text(addWell),
            value: addWell,
          ),
          PopupMenuItem(
            child: Text(settings),
            value: settings,
          ),
        ],
      )
      ];
    } else if (users.technician) {
      return [PopupMenuButton(
        onSelected: (value) {
          choiceAction(value);
        },
        itemBuilder: (context) =>
        [
          PopupMenuItem(
            child: Text(listWells),
            value: listWells,
          ),
          PopupMenuItem(
            child: Text(settings),
            value: settings,
          ),
        ],
      )
      ];
    } else {
      return [PopupMenuButton(
        onSelected: (value) {
          choiceAction(value);
        },
        itemBuilder: (context) =>
        [
          PopupMenuItem(
            child: Text(settings),
            value: settings,
          ),
        ],
      )
      ];
    }
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
    } else {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => WellOverview(title: "List of Wells")));
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
  /// Changes color of gps-button referred to use the user location.
  /// If user deactivates gps location and zoom is changed to default.
  void setUserLocation(Map<String, double> userLocation) {
    if (userLocation != null) {
      if(!userLocated) {
        userLocated = true;
        LatLng location =
        LatLng(userLocation['latitude'], userLocation['longitude']);
        userLocationMarkerColor = Color.fromARGB(255, 0, 175, 0);

        markerMap = wellList.getWellMarkersByRadius(userLocation['latitude'],
            userLocation['longitude'], searchRadius);

        wellList.setUserPositionMarker(location);
        mapController.move(location, 5);
      }
      else {
        userLocated = false;
        markerMap = wellList.getMarkersMap();
        userLocation = null;
        userLocationMarkerColor = Color.fromARGB(255, 200, 0, 0);
        mapController.move(defaultLocation, defaultZoom);
      }

    }
  }

  /// Updates visible markers on map.
  ///
  /// Adds new well markers or updates position of existing well on map.
  /// Deletes well marker if well was deleted.
  void updateMarkers(StatefulMapController markerController ,
                     Map<String, Marker> markerMap) async {

    if(markerController.markers.length < markerMap.length) {
      markerController.addMarkers(markers: markerMap);
    }
    else if(markerController.markers.length > markerMap.length) {
      List<String> removeMarkerList = List<String>();

      if(userLocated) {
        markerMap[locationMarkerName] = markerController.namedMarkers[locationMarkerName];
      }

      markerController.namedMarkers.forEach((name , _) {
        if(!markerMap.containsKey(name)) {
          removeMarkerList.add(name);
        }
      });

      markerController.removeMarkers(names: removeMarkerList);
    }
    else {
      markerController.addMarkers(markers: markerMap);
    }
  }
}