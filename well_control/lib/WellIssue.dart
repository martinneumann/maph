
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

  WellIssue(int id, int wellId, String description, DateTime creationDate,
      String status, bool open, List<String> brokenParts, String confirmedBy, DateTime solvedDate,
      String repairedBy, bool works) {
    this.id = id;
    this.wellId = wellId;
    this.description = description;
    this.creationDate = creationDate;
    this.image = image;
    this.status = status;
    this.open = open;
    this.brokenParts = brokenParts;
    this.confirmedBy = confirmedBy;
    this.solvedDate = solvedDate;
    this.repairedBy = repairedBy;
    this.bill = bill;
    this.works = works;
  }


}