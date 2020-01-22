import 'dart:convert';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

import 'Functions.dart';
import 'Settings.dart';
import 'WellMap.dart';
import 'WellMarker.dart';
import 'WellOverview.dart';

class WellUpdate extends StatefulWidget {
  WellUpdate({Key key, this.title, this.well}) : super(key: key);

  final WellMarker well;
  final String title;

  @override
  _WellUpdateState createState() => _WellUpdateState();
}

class _WellUpdateState extends State<WellUpdate> {
  final _formKey = GlobalKey<FormState>();
  static const wellOverview = "List of Wells";
  static const wellMap = "Map Overview";
  static const settings = "Settings";

  static const List<String> menuChoices = <String>[
    wellOverview,
    wellMap,
    settings
  ];

  final nameController = TextEditingController();
  final fundingController = TextEditingController();
  final costsController = TextEditingController();
  final latitudeController = TextEditingController();
  final longitudeController = TextEditingController();
  String status;

  List<String> _wellStatus = ['Working', 'Maintenance', 'Not Working'];

  String type;
  List<String> _wellTypes = [
    'Type A',
    'Type B',
    'Type C',
    'Type X',
    'Special Well',
    'Power Well'
  ];

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
  void initState() {
    super.initState();

    nameController.text = widget.well.getWellName();
    latitudeController.text = widget.well.location.latitude.toString();
    longitudeController.text = widget.well.location.longitude.toString();
    fundingController.text = widget.well.fundingOrganisation;
    costsController.text = widget.well.costs.toString();
    status = widget.well.status;
    type = widget.well.type;
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
                child: Form(
                    key: _formKey,
                    child: SingleChildScrollView(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: <Widget>[
                          TextFormField(
                            controller: nameController,
                            decoration:
                                InputDecoration(labelText: "Name of well:"),
                            validator: (value) {
                              if (value.isEmpty) {
                                return "Enter well name";
                              }
                              return null;
                            },
                          ),
                          Column(
                            children: <Widget>[
                              TextFormField(
                                controller: latitudeController,
                                decoration:
                                    InputDecoration(labelText: "Latitude:"),
                                validator: (value) {
                                  if (value.isEmpty) {
                                    return "Enter latitude";
                                  }
                                  return null;
                                },
                                onFieldSubmitted: (value) {
                                  latitudeController..text = value.toString();
                                },
                              ),
                              TextFormField(
                                controller: longitudeController,
                                decoration:
                                    InputDecoration(labelText: "Longitude:"),
                                validator: (value) {
                                  if (value.isEmpty) {
                                    return "Enter longitude";
                                  }
                                  return null;
                                },
                              ),
                              DropdownButton<String>(
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
                              DropdownButton<String>(
                                value: type,
                                hint: Text("Select the well type"),
                                onChanged: (String value) {
                                  setState(() {
                                    type = value;
                                  });
                                },
                                items: _wellTypes.map((type) {
                                  return DropdownMenuItem(
                                    child: new Text(type),
                                    value: type,
                                  );
                                }).toList(),
                              ),
                              TextFormField(
                                controller: fundingController,
                                decoration:
                                    InputDecoration(labelText: "Funded by:"),
                                validator: (value) {
                                  if (value.isEmpty) {
                                    return 'Please enter funding organisation.';
                                  }
                                  return null;
                                },
                              ),
                              TextFormField(
                                controller: costsController,
                                decoration:
                                    InputDecoration(labelText: "Costs:"),
                                validator: (value) {
                                  if (value.isEmpty) {
                                    return 'Please enter the building costs of the well.';
                                  }
                                  return null;
                                },
                              ),
                              Padding(
                                padding:
                                    const EdgeInsets.symmetric(vertical: 16.0),
                                child: RaisedButton(
                                  onPressed: () {
                                    // Validate returns true if the form is valid, or false
                                    // otherwise.
                                    if (_formKey.currentState.validate()) {
                                      // If the form is valid, display a Snackbar.
                                      updateWell();
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

  void updateWell() async {
    String color;
    if (status == "Working") {
      color = "green";
    } else if (status == "Maintenance") {
      color = "yellow";
    } else {
      color = "red";
    }

    var location = {};
    location["longitude"] = double.parse(longitudeController.text);
    location["latitude"] = double.parse(latitudeController.text);

    var fundingInfo = {};
    fundingInfo["organisation"] = fundingController.text;
    fundingInfo["price"] = double.parse(costsController.text);

    var wellType = {};
    wellType["name"] = type;

    var data = {};
    data["id"] = widget.well.wellId;
    data["name"] = nameController.text;
    data["status"] = color;
    data["location"] = location;
    data["fundingInfo"] = fundingInfo;
    data["wellType"] = wellType;

    widget.well.name = nameController.text;
    widget.well.location.longitude = double.parse(longitudeController.text);
    widget.well.location.latitude = double.parse(latitudeController.text);
    widget.well.setColor(color);
    widget.well.setWellCosts(costsController.text);
    widget.well.setFundingOrganisation(fundingController.text);
    widget.well.setType(type);

    await postUpdateWell(json.encode(data)).then(
        (response) => print("Response: " + response.statusCode.toString()));

    Navigator.pop(context);
  }
}
