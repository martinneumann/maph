import 'dart:convert';
import 'package:intl/intl.dart';

import 'package:flutter/material.dart';
import 'package:well_control/AddWell.dart';
import 'package:well_control/RepairInformation.dart';
import 'package:well_control/ReportWell.dart';
import 'package:well_control/Settings.dart';
import 'package:well_control/WellIssueLibrary.dart';
import 'package:well_control/WellMap.dart';
import 'package:well_control/WellMarkerLibary.dart' as wellList;

import 'Functions.dart';
import 'WellIssue.dart';
import 'WellMarker.dart';
import 'WellUpdate.dart';

class WellInfo extends StatefulWidget {
  WellInfo({Key key, this.title, this.well}) : super(key: key);

  final WellMarker well;
  final String title;

  @override
  _WellInfoState createState() => _WellInfoState(well);
}

class _WellInfoState extends State<WellInfo> {
  Future<String> wellInfos;
  Future<List<WellIssue>> wellIssues;

  WellMarker wellMarker;

  _WellInfoState(this.wellMarker);

  static const wellUpdate = "Change Well";
  static const wellDelete = "Delete Well";
  static const settings = "Settings";
  static const wellMap = "Map Overview";
  static const addWell = "Add Well";

  static const List<String> menuChoices = <String>[
    wellUpdate,
    wellDelete,
    settings,
    wellMap,
    addWell
  ];

  @override
  void initState() {
    wellInfos = getWellInfos(widget.well);
    wellIssues = getIssuesOfWell(widget.well.wellId.toString());
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
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
            child: FutureBuilder(
          future: wellInfos,
          builder: (BuildContext context, AsyncSnapshot<String> snapshot) {
            if (snapshot.hasData) {
              return SingleChildScrollView(
                  child: Center(
                child: Column(
                  children: [
                    image,
                    Card(
                      child: Column(
                        mainAxisSize: MainAxisSize.min,
                        children: <Widget>[
                          ListTile(
                            title: Text('Name:'),
                            subtitle: Text(wellMarker.getWellName()),
                          ),
                        ],
                      ),
                    ),
                    Card(
                      child: Column(
                        mainAxisSize: MainAxisSize.min,
                        children: <Widget>[
                          ListTile(
                            title: Text('Type:'),
                            subtitle: Text(wellMarker.type),
                          ),
                        ],
                      ),
                    ),
                    Card(
                      child: Column(
                        mainAxisSize: MainAxisSize.min,
                        children: <Widget>[
                          ListTile(
                            title: Text('Geolaction:'),
                            subtitle: Text("Longitude: " +
                                wellMarker.location.longitude.toString() +
                                "\n" +
                                "Latitude: " +
                                wellMarker.location.latitude.toString()),
                          ),
                        ],
                      ),
                    ),
                    Card(
                      child: Column(
                        mainAxisSize: MainAxisSize.min,
                        children: <Widget>[
                          ListTile(
                            title: Text('Funding Info:'),
                            subtitle: Text(wellMarker.fundingOrganisation),
                          ),
                        ],
                      ),
                    ),
                    Card(
                      child: Column(
                        mainAxisSize: MainAxisSize.min,
                        children: <Widget>[
                          ListTile(
                            title: Text('Price:'),
                            subtitle: Text(wellMarker.costs + "\$"),
                          ),
                        ],
                      ),
                    ),
                    Card(
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: <Widget>[
                          FutureBuilder<List<WellIssue>>(
                              future: wellIssues,
                              builder: (BuildContext context,
                                  AsyncSnapshot<List<WellIssue>> snapshot) {
                                List<Widget> children;
                                if (snapshot.hasData) {
                                  if (snapshot.data.length == 0) {
                                    children = <Widget>[
                                      Icon(
                                        Icons.check_circle_outline,
                                        color: Colors.green,
                                        size: 60,
                                      ),
                                    ];
                                  }
                                  children = <Widget>[
                                    Icon(
                                      Icons.check_circle_outline,
                                      color: Colors.green,
                                      size: 60,
                                    ),
                                    Padding(
                                      padding: const EdgeInsets.only(top: 16),
                                child: Card(
                                child: Column(
                                mainAxisAlignment: MainAxisAlignment.center,
                                children: <Widget>[
                                            for (var item in snapshot.data) Card(
                                                child: Text((DateFormat("dd/MM/yyyy").format(DateTime.parse(item.creationDate)) + ": " + item.description.toString() + ". Open: " + item.open.toString()))
                                            ),
                                            ],
                                ),
                                ),
                                        ),
                                  ];
                                } else if (snapshot.hasError) {
                                  print("Snapshot for issues (error): " +
                                      snapshot.toString());
                                  children = <Widget>[
                                    Icon(
                                      Icons.error_outline,
                                      color: Colors.red,
                                      size: 60,
                                    ),
                                    Padding(
                                      padding: const EdgeInsets.only(top: 16),
                                      child: Text('Error: ${snapshot.error}'),
                                    )
                                  ];
                                } else {
                                  children = <Widget>[
                                    SizedBox(
                                      child: CircularProgressIndicator(),
                                      width: 60,
                                      height: 60,
                                    ),
                                    const Padding(
                                      padding: EdgeInsets.only(top: 16),
                                      child: Text('Awaiting result...'),
                                    )
                                  ];
                                }
                                return Center(
                                  child: Column(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    crossAxisAlignment:
                                        CrossAxisAlignment.center,
                                    children: children,
                                  ),
                                );
                              })
                        ],
                      ),
                    ),

                    //  infoSection,
                    // listSection,
                    Container(
                      margin: EdgeInsets.all(10.0),
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                        children: <Widget>[
                          IconButton(
                            tooltip: 'call',
                            color: Theme.of(context).primaryColor,
                            icon: Icon(Icons.call),
                            onPressed: () {
                              //add function to call
                            },
                          ),
                          IconButton(
                            icon: Icon(Icons.near_me),
                            color: Theme.of(context).primaryColor,
                            onPressed: () {},
                          ),
                          IconButton(
                            icon: Icon(Icons.report),
                            color: Theme.of(context).primaryColor,
                            onPressed: () {
                              Navigator.push(
                                  context,
                                  MaterialPageRoute(
                                      builder: (context) => ReportWell(
                                          title: "Report malfunction")));
                            },
                          ),
                          IconButton(
                            icon: Icon(Icons.build),
                            color: Theme.of(context).primaryColor,
                            onPressed: () {
                              Navigator.push(
                                  context,
                                  MaterialPageRoute(
                                      builder: (context) => RepairInformation(
                                          title: "Repair Help")));
                            },
                          ),
                        ],
                      ),
                    ),
                  ],
                ),
              ));
            } else if (snapshot.hasError) {
              print("Error when getting data!");
              return Center(
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  crossAxisAlignment: CrossAxisAlignment.center,
                  children: <Widget>[
                    SizedBox(height: 50),
                    Text("Error: " + snapshot.error.toString()),
                  ],
                ),
              );
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
        )));
  }

  void choiceAction(String choice) {
    if (choice == wellUpdate) {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => WellUpdate(
                  title: "Change well information", well: widget.well)));
    } else if (choice == wellDelete) {
      print("wellId: " + widget.well.wellId.toString());
      requestDelete(widget.well.wellId).then((result) {
        Navigator.pop(context);
      });

    } else if (choice == wellMap) {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => WellMap(title: "Map Overview")));
    } else if (choice == settings) {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => Settings(title: "Settings")));
    } else {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => AddWell(title: "Add new well")));
    }
  }

  ///
  /// Future for well issue list.
  ///

  /// Future for well infos.
  Future<String> getWellInfos(WellMarker well) async {
    var result;

    return getWell(well.wellId).then((response) {
      print("GetWell response:" + response.statusCode.toString());
      print("GetWell response:" + response.body.toString());

      result = json.decode(response.body);
      well.setFundingOrganisation(result["fundingInfo"]["organisation"]);
      well.setType(result["wellType"]["name"]);

      String price = result["fundingInfo"]["price"].toString();
      if (!price.contains('.')) {
        price += ".00";
      }
      well.setWellCosts(price);

      return 'OK';
    });
  }

  Future<String> requestDelete(int wellId) async {
    await deleteWell(wellId).then((response) {
      print("Delete Resposne: " + response.statusCode.toString());
    });

    await wellList.getMarkersMap();

    return 'Deleted';
  }

  Widget image = Card(
    child: Column(
      mainAxisSize: MainAxisSize.min,
      children: <Widget>[
        Container(
            width: 190.0,
            height: 190.0,
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              image: DecorationImage(
                fit: BoxFit.fill,
                image: AssetImage('assets/well_picture/well_1.jpg'),
              ),
            )),
      ],
    ),
  );
}
