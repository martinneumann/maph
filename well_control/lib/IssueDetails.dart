import 'package:well_control/WellIssue.dart';

class IssueDetails extends StatefulWidget {
  IssueDetails({Key key, this.title, this.issue}) : super(key: key);

  final String title;

  @override
  _WellIssueState createState() => _WellIssueState(issue);
}

class  _IssueDetails extends State<WellIssue> {
  Future<WellIssue> issue;
}


  _WellIssueState(this.issue);

  static const wellUpdate = "Change Well";
  static const wellDelete = "Delete Well";
  static const settings = "Settings";
  static const wellMap = "Map Overview";
  static const addWell = "Add Well";

  static const List<String> menuChoices = <String>[
    wellUpdate,
    wellDelete,
    settings,
    wellMap,
    addWell
  ];

  printSomething(String message) {
    print(message);
  }


  @override
  void initState() {