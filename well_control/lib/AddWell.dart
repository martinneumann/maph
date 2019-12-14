import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:well_control/WellMap.dart';
import 'package:well_control/WellMarker.dart';
import 'WellMarkerLibary.dart' as wellList;

import 'Settings.dart';
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

  static const List<String> menuChoices = <String>[wellOverview, wellMap , settings];

  final nameController = TextEditingController();
  final latitudeController = TextEditingController();
  final longitudeController = TextEditingController();
  String status;

  @override
  void dispose() {
    nameController.dispose();
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
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: <Widget>[
                  TextFormField(
                    controller: nameController,
                    decoration: InputDecoration(
                        labelText: "Name of well:"
                    ),
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
                        decoration: InputDecoration(
                            labelText: "Latitude:"
                        ),
                        validator: (value) {
                          if (value.isEmpty) {
                            return 'Please enter latitdue!';
                          }
                          return null;
                        },
                      ),
                      TextFormField(
                        controller: longitudeController,
                        decoration: InputDecoration(
                            labelText: "Longitude:"
                        ),
                        validator: (value) {
                          if (value.isEmpty) {
                            return 'Please enter longitude';
                          }
                          return null;
                        },
                      ),
                      DropdownButton<String>(
                        items: <String>['Working', 'maintenance', 'Not Working'].map((String value) {
                          return new DropdownMenuItem<String>(
                            value: value,
                            child: new Text(value),
                          );
                        }).toList(),
                        onChanged: (String value) {
                          setState(() {
                            status = value;
                          });
                        }
                      )
                    ],
                  ),
                  Padding(
                    padding: const EdgeInsets.symmetric(vertical: 16.0),
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
            )
        )
      )
    );
  }

  void choiceAction(String choice) {
    if (choice == settings) {
      Navigator.push(context,
          MaterialPageRoute(
              builder: (context) => Settings(title: "Settings")));
    } else if(choice == wellOverview) {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => WellOverview(title: "List of Wells")));
    }
    else {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => WellMap(title: "Well Map")));
    }
  }

  void addWell() {
    String color;
    if(status == "Working") {
      color = "green";
    }
    else if(status == "maintenance") {
      color = "yellow";
    }
    else {
      color = "red";
    }
    wellList.wells.add(new WellMarker(nameController.text , color ,
        double.parse(latitudeController.text) ,
        double.parse(longitudeController.text)));

    //print(wellList.wells.length);
  }
}