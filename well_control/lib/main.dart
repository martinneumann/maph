import 'package:flutter/material.dart';
import 'package:well_control/AddWell.dart';
import 'package:well_control/RepairInformation.dart';
import 'package:well_control/ReportWell.dart';
import 'package:well_control/WellInfo.dart';
import 'package:well_control/WellMap.dart';
import 'package:well_control/WellUpdate.dart';

void main() => runApp(MyApp());
/// App manages wells in certain country.
///
/// App includes map to show current wells and their status by different colors.
/// There is an overview of all existing wells available by menu item.
///
/// Users can add [AddWell], update [WellUpdate] or delete [WellInfo] wells.
/// Also they can write an issue report [ReportWell] if wells status is changed
/// from working to Maintenance.
/// Well issues are visible for every well at view [WellInfo].
///
/// App provides view for repair information at [RepairInformation].
class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Well Control',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: WellMap(title: 'Well Map'),
    );
  }
}
