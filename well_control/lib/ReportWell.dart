import 'dart:io';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:image_picker/image_picker.dart';
import 'package:well_control/WellMap.dart';
import 'package:http/http.dart' as http;

import 'RepairInformation.dart';
import 'Settings.dart';
import 'WellMarkerLibary.dart' as wellList;
import 'WellOverview.dart';

/// Makes a POST request to save a new issue to the database.
/// @param description The issue's description
Future<http.Response> postNewIssue(String description) {
  return http.post('https://wellapi.azurewebsites.net/api/Issue/PostNewIssue/' + description);
}

class ReportWell extends StatefulWidget {
  ReportWell({Key key, this.title}) : super(key: key);

  final String title;

  @override
  _ReportWellState createState() => _ReportWellState();
}

class _ReportWellState extends State<ReportWell> {
  final _formKey = GlobalKey<FormState>();
  static const wellOverview = "List of Wells";
  static const wellMap = "Map Overview";
  static const settings = "Settings";
  static const repairInformation = "Repair Help";

  static const List<String> menuChoices = <String>[
    wellOverview,
    wellMap,
    settings,
    repairInformation,
  ];

  String _selectedWell;
  List<String> wellNames = new List<String>();

  Future<File> _imageFile;

  @override
  void initState() {
    super.initState();

    for (int i = 0; i < wellList.wells.length; i++) {
      wellNames.add(wellList.wells[i].getMarkerName());
    }
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
        home: Scaffold(
            resizeToAvoidBottomPadding: true,
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
                child: ListView(children: <Widget>[
                  DropdownButton(
                    isExpanded: true,
                    hint: Text('Please choose a well'),
                    value: _selectedWell,
                    onChanged: (newValue) {
                      setState(() {
                        _selectedWell = newValue;
                      });
                    },
                    items: wellList.wells.map((well) {
                      return DropdownMenuItem(
                        child: new Text(well.getMarkerName()),
                        value: well.getMarkerName(),
                      );
                    }).toList(),
                  ),
                  TextFormField(
                    keyboardType: TextInputType.multiline,
                    maxLines: 10,
                    decoration: InputDecoration(
                        alignLabelWithHint: true,
                        labelText: "Describe the problem:",
                        border: new OutlineInputBorder(
                            borderRadius: const BorderRadius.all(
                              const Radius.circular(0.0),
                            ),
                            borderSide: new BorderSide(
                                color: Colors.black87, width: 1.0))),
                    validator: (value) {
                      if (value.isEmpty) {
                        return 'Please enter some text';
                      }
                      return null;
                    },
                  ),
                  showImage(),
                  FlatButton.icon(
                    onPressed: () {
                      _displayOptionsDialog();
                    },
                    icon: Icon(Icons.add_a_photo),
                    label: Text('Add a Photo'),
                    color: Colors.blue,
                  ),
                  submitButton(),
                ]),
              ),
            )));
  }

  void _displayOptionsDialog() async {
    await _optionsDialogBox();
  }

  Future<void> _optionsDialogBox() {
    return showDialog(
        context: context,
        barrierDismissible: true,
        builder: (BuildContext context) {
          return AlertDialog(
            content: new SingleChildScrollView(
              child: new ListBody(
                children: <Widget>[
                  GestureDetector(
                    child: new Text('Take Photo'),
                    onTap: imageSelectorCamera,
                  ),
                  Padding(
                    padding: EdgeInsets.all(8.0),
                  ),
                  GestureDetector(
                    child: new Text('Select Image From Gallery'),
                    onTap: imageSelectorGallery,
                  ),
                ],
              ),
            ),
          );
        });
  }

  void imageSelectorCamera() async {
    Navigator.pop(context);

    setState(() {
//      _imageFile = ImagePicker.pickImage(source: ImageSource.camera);
    });
  }

  void imageSelectorGallery() async {
    Navigator.pop(context);

    setState(() {
      _imageFile = ImagePicker.pickImage(source: ImageSource.gallery);
    });
  }

  Widget showImage() {
    return FutureBuilder<File>(
      future: _imageFile,
      builder: (BuildContext context, AsyncSnapshot<File> snapshot) {
        if (snapshot.connectionState == ConnectionState.done &&
            snapshot.data != null) {
          return Image.file(
            snapshot.data,
            width: 120,
            height: 120,
          );
        } else if (snapshot.error != null) {
          return const Text(
            'Error Picking Image',
            textAlign: TextAlign.center,
          );
        } else {
          return const Text(
            'No Image Selected',
            textAlign: TextAlign.center,
          );
        }
      },
    );
  }

  Widget submitButton() {
    return Align(
      alignment: Alignment.bottomRight,
      child: FlatButton.icon(
        color: Colors.blue,
        textColor: Colors.white,
        icon: Icon(Icons.send),
        label: Text('Submit'),
        onPressed: () {
          postNewIssue('This is a test issue to test the POST request.').then((response) =>
            print(response)
          );

          wellList.wells[wellNames.indexOf(_selectedWell)].setColor("yellow");
          Navigator.pop(context);
        },
      ),
    );
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
    } else if (choice == wellMap) {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => WellMap(title: "Well Map")));
    } else {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => RepairInformation(title: "Repair Help")));
    }
  }
}