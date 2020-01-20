
/// Class that represents an issue. Each well can have multiple issues.
class WellIssue {
  int id;
  int wellId;
  String description;
  DateTime creationDate;
  String image;
  String status;
  bool open;
  bool works;
  List<String> brokenParts;
  String confirmedBy;
  DateTime solvedDate;
  String repairedBy;
  String bill;

  WellIssue(int id, int wellId, String description, DateTime creationDate, String image,
      String status, bool open, List<String> brokenParts, String confirmedBy, DateTime solvedDate,
      String repairedBy, String bill, bool works) {
    id = id;
    wellId = wellId;
    description = description;
    creationDate = creationDate;
    image = image;
    status = status;
    open = open;
    brokenParts = brokenParts;
    confirmedBy = confirmedBy;
    solvedDate = solvedDate;
    repairedBy = repairedBy;
    bill = bill;
    works = works;
  }


}