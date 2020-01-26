import 'dart:convert';
import 'dart:io';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:image_picker/image_picker.dart';

import 'Functions.dart';
import 'RepairInformation.dart';
import 'Settings.dart';
import 'WellMarker.dart';

class ReportWell extends StatefulWidget {
  ReportWell({Key key, this.title, this.well}) : super(key: key);

  final WellMarker well;
  final String title;

  @override
  _ReportWellState createState() => _ReportWellState();
}

class _ReportWellState extends State<ReportWell> {
  final _formKey = GlobalKey<FormState>();

  static const settings = "Settings";
  static const repairInformation = "Repair Help";

  static const List<String> menuChoices = <String>[
    repairInformation,
    settings,
  ];

  String _selectedPart;
  List<String> parts = new List<String>();

  Future<File> _imageFile;

  @override
  void initState() {
    super.initState();
    widget.well.wellParts.forEach((key, value) => parts.add(key));
  }

  final textController = TextEditingController();

  @override
  void dispose() {
    // Clean up the controller when the widget is disposed.
    textController.dispose();
    super.dispose();
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
          body: new Container(
              margin: EdgeInsets.all(18.0),
              child: Form(
                key: _formKey,
                child: SingleChildScrollView(
                  child: Column(children: <Widget>[
                    Row(children: <Widget>[
                      Container(
                        padding: const EdgeInsets.only(left: 10.0, right: 15.0),
                        child: Icon(
                          Icons.title,
                          color: Colors.grey,
                        ),
                      ),
                      Container(child: Text(widget.well.name))
                    ]),
                    Divider(color: Colors.black87),
                    Row(children: <Widget>[
                      Container(
                        padding: const EdgeInsets.only(left: 10.0, right: 15.0),
                        child: Icon(
                          Icons.build,
                          color: Colors.grey,
                        ),
                      ),
                      Container(
                          child: Expanded(
                            child: DropdownButtonHideUnderline(
                              child: DropdownButton(
                                isExpanded: true,
                                hint: Text('Please choose a well part.'),
                                value: _selectedPart,
                                onChanged: (newValue) {
                                  setState(() {
                                    _selectedPart = newValue;
                                  });
                                },
                                items: parts.map((part) {
                                  return DropdownMenuItem(
                                    child: new Text(part),
                                    value: part,
                                  );
                                }).toList(),
                              ),
                            ),
                          ))
                    ]),
                    Divider(color: Colors.black87),
                    TextFormField(
                      keyboardType: TextInputType.multiline,
                      maxLines: 10,
                      controller: textController,
                      decoration: InputDecoration(
                          alignLabelWithHint: true,
                          labelText: "Describe the Problem",
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
                    FlatButton.icon(
                      onPressed: () {
                        _displayOptionsDialog();
                      },
                      icon: Icon(Icons.add_a_photo),
                      label: Text('Add a Photo'),
                      color: Colors.blue,
                    ),
                  ]),
                ),
              )),
          floatingActionButton: FloatingActionButton(
            onPressed: () {
              createIssue();
              Navigator.pop(context);
              setState(() {});
            },
            child: Icon(Icons.send),
            backgroundColor: Colors.blue,
          ),
        ));
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
          return Text('');
        }
      },
    );
  }


  void choiceAction(String choice) {
    if (choice == settings) {
      Navigator.push(context,
          MaterialPageRoute(builder: (context) => Settings(title: "Settings")));
    } else {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => RepairInformation(title: "Repair Help")));
    }
  }

  void createIssue() async {
    var data = {};

    data["description"] = textController.text;
    data["works"] = false;
    List<int> brokenParts = new List<int>();
    brokenParts.add(widget.well.wellParts[_selectedPart]);
    data["brokenPartIds"] = brokenParts;
    data["wellId"] = widget.well.wellId;

    await postNewIssue(json.encode(data)).then((response) {
      print("Response: " + response.statusCode.toString());
    });
  }
}


