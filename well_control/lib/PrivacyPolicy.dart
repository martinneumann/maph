import 'dart:async' show Future;

import 'package:flutter/material.dart';
import 'package:well_control/Settings.dart';
import 'package:well_control/WellMap.dart';
import 'package:well_control/WellOverview.dart';
/// Class shows app definition of [PrivacyPolicy].
///
/// Class create an view with description of privacy policy.
class PrivacyPolicy extends StatefulWidget {
  PrivacyPolicy({Key key, this.title}) : super(key: key);

  final String title;

  @override
  _PrivacyPolicyState createState() => _PrivacyPolicyState();
}

/// State defines constants for menu.
class _PrivacyPolicyState extends State<PrivacyPolicy> {
  static const settings = "Settings";
  static const wellOverview = "List of Wells";
  static const wellMap = "Map Overview";

  static const List<String> menuChoices = <String>[
    settings,
    wellOverview,
    wellMap
  ];

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
          child: new FutureBuilder(
              future: loadAsset(context),
              builder: (BuildContext context, AsyncSnapshot<String> text) {
                return new SingleChildScrollView(
                  padding: new EdgeInsets.all(8.0),
                  child: new Text(text.data),
                );
              })),
    );
  }

  /// Loads privacy policy text from asset folder.
  Future<String> loadAsset(BuildContext context) async {
    return await DefaultAssetBundle.of(context)
        .loadString('assets/privacypolicy.txt');
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
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => WellMap(title: "Map Overview")));
    }
  }
}
