import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:font_awesome_flutter/font_awesome_flutter.dart';
import 'package:latlong/latlong.dart';

class WellMap extends StatefulWidget {
  WellMap({Key key, this.title}) : super(key: key);

  final String title;

  @override
  _WellMapState createState() => _WellMapState();
}

class _WellMapState extends State<WellMap> {

  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        body: Center(
          child: Container(
            //width: 200,
            //height: 200,
            child: FlutterMap(
              options: MapOptions(
                center: LatLng(45.5231, -122.6765),
                zoom: 13.0,
              ),
              layers: [
                TileLayerOptions(
                  urlTemplate:
                  'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
                  subdomains: ['a', 'b', 'c'],
                  additionalOptions: {
                    'access_token': '',
                    'id': ''
                  },
                ),
                MarkerLayerOptions (
                    markers: [
                      Marker(
                          //width: 80.0,
                          //height: 80.0,
                          point: LatLng(45.5231, -122.6765),
                          builder: (ctx) =>
                              Container(
                                  child: IconButton(
                                    icon: Icon(FontAwesomeIcons.mapMarkerAlt),
                                    color: Color.fromARGB(255, 250, 0, 0),
                                    iconSize: 45.0,
                                    onPressed: (){},
                                  )
                              )
                      )
                    ]
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
