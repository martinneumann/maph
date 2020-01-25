import 'package:well_control/WellIssue.dart';
import 'dart:convert';

import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
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

class IssueDetails extends StatefulWidget {
  IssueDetails({Key key, this.title, this.issue}) : super(key: key);

  final String title;
  final WellIssue issue;

  static const wellUpdate = "Change Well info";

  /// Stores menu item title for reporting malfunction.
  static const report = "Report Malfunction";
  static const settings = "Settings";
  static const wellMap = "Map Overview";
  static const wellDelete = "Delete Well";



  static const List<String> menuChoices = <String>[
    wellUpdate,
    report,
    settings,
    wellMap,
    wellDelete
  ];

  @override
  _IssueDetailsState createState() => _IssueDetailsState(issue);
}

class _IssueDetailsState extends State<IssueDetails> {

  WellIssue wellIssue;

  _IssueDetailsState(this.wellIssue);

  static const wellUpdate = "Change Well info";

  /// Stores menu item title for reporting malfunction.
  static const report = "Report Malfunction";
  static const settings = "Settings";
  static const wellMap = "Map Overview";
  static const wellDelete = "Delete Well";


  static const List<String> menuChoices = <String>[
    wellUpdate,
    report,
    settings,
    wellMap,
    wellDelete
  ];

  printSomething(String message) {
    print(message);
  }

  ScrollController _controller = new ScrollController();

  @override
  void initState() {
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
              builder: (BuildContext context,
                  AsyncSnapshot<WellIssue> snapshot) {
                  return SingleChildScrollView(
                      child: Center(
                        child: Column(
                          children: [
                            Card(
                              child: Column(
                                mainAxisSize: MainAxisSize.min,
                                children: <Widget>[
                                  ListTile(
                                    title: Text('Description:'),
                                    subtitle: Text(
                                        wellIssue.description.toString()),
                                  ),
                                ],
                              ),
                            ),
                            Card(
                              child: Column(
                                mainAxisSize: MainAxisSize.min,
                                children: <Widget>[
                                  ListTile(
                                    title: Text('Created on:'),
                                    subtitle: Text(
                                        wellIssue.creationDate.toString()),
                                  ),
                                ],
                              ),
                            ),
                            Card(
                              child: Column(
                                mainAxisSize: MainAxisSize.min,
                                children: <Widget>[
                                  ListTile(
                                    title: Text('Open:'),
                                    subtitle: Text(
                                        wellIssue.status.toString()),
                                  ),
                                ],
                              ),
                            ),
                            Card(
                              child: Column(
                                mainAxisSize: MainAxisSize.min,
                                children: <Widget>[
                                  ListTile(
                                    title: Text('Confirmed by:'),
                                    subtitle: Text(
                                        wellIssue.confirmedBy.toString()),
                                  ),
                                ],
                              ),
                            ),
                            Card(
                              child: Column(
                                mainAxisSize: MainAxisSize.min,
                                children: <Widget>[
                                  ListTile(
                                    title: Text('Funded by:'),
                                    subtitle: Text(
                                        wellIssue.fundingInfo.toString()),
                                  ),
                                ],
                              ),
                            ),
                            Card(
                              child: Column(
                                mainAxisAlignment: MainAxisAlignment.center,
                                children: <Widget>[
                                ],
                              ),
                            ),

                            //  infoSection,
                            // listSection,
                            Container(
                              margin: EdgeInsets.all(10.0),
                              child: Row(
                                mainAxisAlignment: MainAxisAlignment
                                    .spaceEvenly,
                                children: <Widget>[
                                  IconButton(
                                    tooltip: 'call',
                                    color: Theme
                                        .of(context)
                                        .primaryColor,
                                    icon: Icon(Icons.call),
                                    onPressed: () {
                                      //add function to call
                                    },
                                  ),
                                  IconButton(
                                    icon: Icon(Icons.near_me),
                                    color: Theme
                                        .of(context)
                                        .primaryColor,
                                    onPressed: () {},
                                  ),
                                  IconButton(
                                    icon: Icon(Icons.report),
                                    color: Theme
                                        .of(context)
                                        .primaryColor,
                                    onPressed: () {
                                      Navigator.push(
                                          context,
                                          MaterialPageRoute(
                                              builder: (context) =>
                                                  ReportWell(
                                                      title: "Report malfunction",
                                                      well: null)));
                                    },
                                  ),
                                  IconButton(
                                    icon: Icon(Icons.build),
                                    color: Theme
                                        .of(context)
                                        .primaryColor,
                                    onPressed: () {
                                      Navigator.push(
                                          context,
                                          MaterialPageRoute(
                                              builder: (context) =>
                                                  RepairInformation(
                                                      title: "Repair Help")));
                                    },
                                  ),
                                ],
                              ),
                            ),
                          ],
                        ),
                      ));
              },
            )));

  }


  void choiceAction(String choice) {
    if (choice == wellUpdate) {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) =>
                  WellUpdate(
                      title: "Change well information", well: null)));
    } else if (choice == wellDelete) {
        Navigator.pop(context);
      }
    }
  }

