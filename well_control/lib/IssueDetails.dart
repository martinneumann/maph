import 'package:well_control/WellIssue.dart';

import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:well_control/RepairInformation.dart';
import 'package:well_control/WellIssueLibrary.dart';
import 'package:well_control/WellMarkerLibary.dart' as wellList;

import 'Functions.dart';
import 'WellIssue.dart';
import 'WellUpdate.dart';
import 'UserLibrary.dart' as users;

/// Class provides view of issue in details.
class IssueDetails extends StatefulWidget {
  IssueDetails({Key key, this.title, this.issue}) : super(key: key);

  /// Title of view.
  final String title;

  /// Gets clicked [WellIssue] form other view.
  final WellIssue issue;

  /// Stores menu item title for update well.
  static const wellUpdate = "Change Well info";

  /// Stores menu item title for settings.
  static const settings = "Settings";

  /// Stores menu item title for map.
  static const wellMap = "Map Overview";

  /// Stores menu item titles.
  static const List<String> menuChoices = <String>[
    settings,
    wellMap,
  ];

  @override
  _IssueDetailsState createState() => _IssueDetailsState(issue);
}

class _IssueDetailsState extends State<IssueDetails> {
  /// Stores current [WellIssue].
  WellIssue wellIssue;

  /// Stores downloaded issue.
  Future<WellIssue> issueFuture;

  _IssueDetailsState(this.wellIssue);

  /// Stores menu item title for map.
  static const wellMap = "Back to Map";

  /// Stores menu item titles.
  static const List<String> menuChoices = <String>[
    wellMap,
  ];

  @override
  void initState() {
    super.initState();
    issueFuture = getSpecificIssue(wellIssue.id);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: Text(wellList.wells
                  .firstWhere((o) => o.wellId == wellIssue.wellId)
                  .name +
              " Issue No. " +
              wellIssue.id.toString()),
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
          future: issueFuture,
          builder: (BuildContext context, AsyncSnapshot<WellIssue> snapshot) {
            if (snapshot.hasData) {
              return SingleChildScrollView(
                  child: Column(
                children: [
                  Icon(
                    Icons.error_outline,
                    color: Colors.red,
                    size: 90,
                  ),
                  Card(
                    child: Column(
                      mainAxisSize: MainAxisSize.min,
                      children: <Widget>[
                        ListTile(
                          title: Text('Description'),
                          subtitle: Text(wellIssue.description.toString()),
                        ),
                      ],
                    ),
                  ),
                  Card(
                    child: Column(
                      mainAxisSize: MainAxisSize.min,
                      children: <Widget>[
                        ListTile(
                          title: Text('Created on'),
                          subtitle: Text(DateFormat('dd/MM/yyyy').format(
                              DateTime.parse(
                                  wellIssue.creationDate.toString()))),
                        ),
                      ],
                    ),
                  ),
                  Card(
                    child: Column(
                      mainAxisSize: MainAxisSize.min,
                      children: <Widget>[
                        ListTile(
                          title: Text('Status'),
                          subtitle: Text(wellIssue.status.toString()),
                        ),
                      ],
                    ),
                  ),
                  Card(
                    child: Column(
                      mainAxisSize: MainAxisSize.min,
                      children: <Widget>[
                        ListTile(
                          title: Text('Operational'),
                          subtitle: Text(wellIssue.works.toString()),
                        ),
                      ],
                    ),
                  ),
                  Card(
                    child: Column(
                      mainAxisSize: MainAxisSize.min,
                      children: <Widget>[
                        if (wellIssue.confirmedBy != null)
                          ListTile(
                            title: Text('Confirmed by'),
                            subtitle: Text(wellIssue.confirmedBy),
                          ),
                        if (wellIssue.confirmedBy == null)
                          ListTile(
                            title: Text('Confirmed by'),
                            subtitle: Text("Nobody has confirmed this issue."),
                          ),
                      ],
                    ),
                  ),
                  Card(
                    child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: <Widget>[
                          if (snapshot.data.brokenParts != null)
                            ListTile(
                              title: Text('Broken part:'),
                              subtitle: Text(snapshot
                                      .data.brokenParts.first.name +
                                  "; condition: " +
                                  snapshot.data.brokenParts.first.description),
                            ),
                          if (snapshot.data.brokenParts == null)
                            ListTile(
                              title: Text('Broken part'),
                              subtitle: Text("No part was specified."),
                            ),
                        ]),
                  ),
                  //  infoSection,
                  // listSection,
                  Container(
                    margin: EdgeInsets.all(10.0),
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                      children: <Widget>[
                        IconButton(
                          tooltip: 'Confirm this issue',
                          color: Theme.of(context).primaryColor,
                          icon: Icon(Icons.thumb_up),
                          onPressed: () {
                            print("Confirmed Issue!");
                            wellIssue.confirmedBy =
                                users.getActiveUser().toString();
                            print(wellIssue.confirmedBy);
                            updateIssue(wellIssue).then((response) {
                              print(response.statusCode.toString());
                            });
                            //add function to call
                          },
                        ),
                        IconButton(
                          tooltip: 'Get reapir help',
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
              ));
            } else if (snapshot.hasError) {
              return Text("Error while loading data.");
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

  /// Methods defines action of clicked menu item.
  ///
  /// Opens certain view by comparing clicked [choice] with menu list names.
  void choiceAction(String choice) {
    if (choice == wellMap) {
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) =>
                  WellUpdate(title: "Back to Map", well: null)));
    } else {
      Navigator.pop(context);
    }
  }
}
