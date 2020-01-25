import 'dart:convert';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:well_control/WellMarkerLibary.dart' as wellList;

import 'Functions.dart';
import 'Settings.dart';
import 'WellMap.dart';
import 'WellOverview.dart';

/// Class provides view to create new well by user.
///
/// View consists of several input field for well description.
class AddWell extends StatefulWidget {
  AddWell({Key key, this.title}) : super(key: key);

  /// Title of view.
  final String title;

  @override
  _AddWellState createState() => _AddWellState();
}

/// State provides view of [AddWell].
class _AddWellState extends State<AddWell> {

  /// Key identifies form for validation.
  final _formKey = GlobalKey<FormState>();

  /// Stores menu item title for well list.
  static const wellOverview = "List of Wells";

  /// Stores menu item title for map.
  static const wellMap = "Map Overview";

  /// Stores menu item title for settings.
  static const settings = "Settings";

  /// Stores menu item titles.
  static const List<String> menuChoices = <String>[
    wellOverview,
    wellMap,
    settings
  ];

  /// Stores user input for name of well.
  final nameController = TextEditingController();

  /// Stores user input of funding organisation.
  final fundingController = TextEditingController();

  /// Stores user input for costs of creating well.
  final costsController = TextEditingController();

  /// Stores latitude of well.
  final latitudeController = TextEditingController();

  /// Stores longitude of well.
  final longitudeController = TextEditingController();

  /// Stores user input of current status from well.
  String status;

  /// Stores available status options.
  List<String> _wellStatus = ['Working', 'Maintenance', 'Not Working'];

  /// Stores user input for type of well.
  String type;

  @override
  void dispose() {
    nameController.dispose();
    latitudeController.dispose();
    longitudeController.dispose();
    fundingController.dispose();
    costsController.dispose();
    super.dispose();
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
            body: new Container(
                margin: const EdgeInsets.all(18.0),
                child: Form(
                    key: _formKey,
                    child: SingleChildScrollView(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: <Widget>[
                          TextFormField(
                            controller: nameController,
                            decoration: InputDecoration(
                              labelText: "Name of well:",
                              border: InputBorder.none,
                              prefixIcon: Padding(
                                padding: EdgeInsets.only(right: 10.0),
                                child: Icon(Icons.title),
                              ),
                            ),
                            validator: (value) {
                              if (value.isEmpty) {
                                return 'Please enter some text';
                              }
                              return null;
                            },
                          ),
                          Divider(color: Colors.black87),
                          TextFormField(
                            controller: longitudeController,
                            keyboardType: TextInputType.number,
                            decoration: InputDecoration(
                              labelText: "Longitude:",
                              border: InputBorder.none,
                              prefixIcon: Padding(
                                padding: EdgeInsets.only(right: 10.0),
                                child: Icon(Icons.gps_fixed),
                              ),
                            ),
                            validator: (value) {
                              if (value.isEmpty) {
                                return 'Please enter longitude';
                              } else if (double.parse(value) < -180 ||
                                  double.parse(value) > 180) {
                                return 'Longitude has to be between -180 and 180 degrees!';
                              }
                              return null;
                            },
                          ),
                          Divider(color: Colors.black87),
                          Column(
                            children: <Widget>[
                              TextFormField(
                                controller: latitudeController,
                                keyboardType: TextInputType.number,
                                decoration: InputDecoration(
                                  labelText: "Latitude:",
                                  border: InputBorder.none,
                                  prefixIcon: Padding(
                                    padding: EdgeInsets.only(right: 10.0),
                                    child: Icon(Icons.gps_fixed),
                                  ),
                                ),
                                validator: (value) {
                                  if (value.isEmpty) {
                                    return 'Please enter latitude!';
                                  } else if (double.parse(value) < -90 ||
                                      double.parse(value) > 90) {
                                    return 'Latitude has to be between -90 and 90 degrees!';
                                  }
                                  return null;
                                },
                              ),
                              Divider(color: Colors.black87),
                              Row(children: <Widget>[
                                Container(
                                  padding: const EdgeInsets.only(
                                      left: 10.0, right: 15.0),
                                  child: Icon(
                                    Icons.build,
                                    color: Colors.grey,
                                  ),
                                ),
                                Container(
                                  child: Expanded(
                                    child: DropdownButtonHideUnderline(
                                      child: DropdownButton<String>(
                                        isExpanded: true,
                                        value: status,
                                        hint: Text("Select a well status"),
                                        onChanged: (String value) {
                                          setState(() {
                                            status = value;
                                          });
                                        },
                                        items: _wellStatus.map((stat) {
                                          return DropdownMenuItem(
                                            child: new Text(stat),
                                            value: stat,
                                          );
                                        }).toList(),
                                      ),
                                    ),
                                  ),
                                ),
                              ]),
                              Divider(color: Colors.black87),
                              Row(
                                children: <Widget>[
                                  Container(
                                    margin: const EdgeInsets.only(
                                        left: 10.0, right: 15.0),
                                    child: Icon(Icons.settings,
                                        color: Colors.grey),
                                  ),
                                  Container(
                                    child: Expanded(
                                      child: DropdownButtonHideUnderline(
                                        child: DropdownButton<String>(
                                          isExpanded: true,
                                          value: type,
                                          hint: Text("Select the well type"),
                                          onChanged: (String value) {
                                            setState(() {
                                              type = value;
                                            });
                                          },
                                          items: wellList.wellTypeNames.map((
                                              type) {
                                            return DropdownMenuItem(
                                              child: new Text(type),
                                              value: type,
                                            );
                                          }).toList(),
                                        ),
                                      ),
                                    ),
                                  ),
                                ],
                              ),
                              Divider(color: Colors.black87),
                              TextFormField(
                                controller: fundingController,
                                decoration: InputDecoration(
                                  labelText: "Funded by:",
                                  border: InputBorder.none,
                                  prefixIcon: Padding(
                                    padding: EdgeInsets.only(right: 10.0),
                                    child: Icon(Icons.account_balance),
                                  ),
                                ),
                                validator: (value) {
                                  if (value.isEmpty) {
                                    return 'Please enter funding organisation.';
                                  }
                                  return null;
                                },
                              ),
                              Divider(color: Colors.black87),
                              TextFormField(
                                controller: costsController,
                                keyboardType: TextInputType.number,
                                decoration: InputDecoration(
                                  border: InputBorder.none,
                                  labelText: "Costs:",
                                  prefixIcon: Padding(
                                    padding: EdgeInsets.only(right: 10.0),
                                    child: Icon(Icons.monetization_on),
                                  ),
                                ),
                                validator: (value) {
                                  if (value.isEmpty) {
                                    return 'Please enter the building costs of the well.';
                                  }
                                  return null;
                                },
                              ),
                              Divider(color: Colors.black87),
                              new Container(
                                padding: const EdgeInsets.all(18.0),
                                child: RaisedButton(
                                  onPressed: () {
                                    if (_formKey.currentState.validate()) {
                                      addWell();
                                    }
                                  },
                                  child: Text('Submit'),
                                ),
                              ),
                            ],
                          ),
                        ],
                      ),
                    )))));
  }

  /// Methods defines action of clicked menu item.
  ///
  /// Opens certain view by comparing clicked [choice] with menu list names.
  void choiceAction(String choice) {
    if (choice == settings) {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => Settings(title: "Settings")));
    } else if (choice == wellOverview) {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => WellOverview(title: "List of Wells")));
    } else {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => WellMap(title: "Well Map")));
    }
  }

  /// Save new well.
  ///
  /// Adds new well by sending user input data to database.
  void addWell() async {
    String color;
    if (status == "Working") {
      color = "green";
    } else if (status == "Maintenance") {
      color = "yellow";
    } else {
      color = "red";
    }

    var location = {};
    location["latitude"] = double.parse(latitudeController.text);
    location["longitude"] = double.parse(longitudeController.text);

    var fundingInfo = {};
    fundingInfo["organisation"] = fundingController.text;
    fundingInfo["price"] = double.parse(costsController.text);

    var data = {};
    data["name"] = nameController.text;
    data["status"] = color;
    data["location"] = location;
    data["fundingInfo"] = fundingInfo;
    data["wellTypeId"] =
    wellList.wellTypeIds[wellList.wellTypeNames.indexOf(type)];

    await postNewWell(json.encode(data)).then(
            (response) => print("Response: " + response.statusCode.toString()));
    await wellList.getMarkersMap();
    Navigator.pop(context);
  }
}
