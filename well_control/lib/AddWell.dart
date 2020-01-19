import 'dart:convert';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

import 'Functions.dart';
import 'Settings.dart';
import 'WellMap.dart';
import 'WellOverview.dart';

class AddWell extends StatefulWidget {
  AddWell({Key key, this.title}) : super(key: key);

  final String title;

  @override
  _AddWellState createState() => _AddWellState();
}

class _AddWellState extends State<AddWell> {
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
  List<String> _wellTypes = ['Type A', 'Type B', 'Type C'];

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
                                return 'Please enter some text';
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
                                    return 'Please enter latitude!';
                                  }
                                  return null;
                                },
                              ),
                              TextFormField(
                                controller: longitudeController,
                                decoration:
                                InputDecoration(labelText: "Longitude:"),
                                validator: (value) {
                                  if (value.isEmpty) {
                                    return 'Please enter longitude';
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

  void addWell() async {
    String color;
    if (status == "Working") {
      color = "#00FF00";
    } else if (status == "maintenance") {
      color = "#FFFF00";
    } else {
      color = "#FF0000";
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
    data["name"] = nameController.text;
    data["status"] = color;
    data["location"] = location;
    data["fundingInfo"] = fundingInfo;
    data["wellType"] = wellType;

    await postNewWell(json.encode(data)).then(
            (response) => print("Response: " + response.statusCode.toString()));

    Navigator.pop(context);
  }
}
