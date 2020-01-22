import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:flutter_map_marker_cluster/flutter_map_marker_cluster.dart';
import 'package:latlong/latlong.dart';
import 'package:location/location.dart';
import 'package:well_control/AddWell.dart';
import 'package:well_control/ReportWell.dart';
import 'package:well_control/Settings.dart';
import 'package:well_control/WellOverview.dart';

import 'WellMarkerLibary.dart' as wellList;

class WellMap extends StatefulWidget {
  WellMap({Key key, this.title}) : super(key: key);

  final String title;

  @override
  _WellMapState createState() => _WellMapState();
}

class _WellMapState extends State<WellMap> {
  MapController mapController = new MapController();
  Location userLocation = new Location();

  static const mapMarkers = null;
  static const listofWells = "List of wells";
  static const addWell = "Add Well";
  static const settings = "Settings";
  static const report = "Report Malfunction";
  static const List<String> menuChoices = <String>[
    listofWells,
    addWell,
    report,
    settings
  ];

  @override
  void initState() {
    super.initState();
  }

  Future<List<Marker>> _markerList = wellList.getMarkers();
  List<Widget> children;

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
          child: FutureBuilder<List<Marker>>(
            future: _markerList,
            builder:
                (BuildContext context, AsyncSnapshot<List<Marker>> snapshot) {
              print("Snapshot data: " + snapshot.data.toString());
              if (snapshot.hasData) {
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
                            markers: snapshot.data,
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

  void choiceAction(String choice) {
    if (choice == settings) {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => Settings(title: "Settings")));
    } else if (choice == addWell) {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => AddWell(title: "Add Well")));
    } else if (choice == listofWells) {
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

  Future<Map<String, double>> _getLocation() async {
    var currentLocation = <String, double>{};
    try {
      currentLocation = await userLocation.getLocation();
    } catch (e) {
      currentLocation = null;
    }
    return currentLocation;
  }

  void setUserLocation(Map<String, double> userLocation) {
    if (userLocation != null) {
      LatLng location =
      LatLng(userLocation['latitude'], userLocation['longitude']);
      wellList.setUserPositionMarker(location);
      mapController.move(location, 14);
    }
  }
}
