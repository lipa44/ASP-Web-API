# Some points about lab-2 architecture

- Each class instance have it's **identical attribute**: `ExtraStudent`: **Id**, `MegaFaculty`: **Name**, `OgnpCourse`: **Name**, `ExtraGroup`: **Name** - none of them could be created twice with that attributes
- `ExtraGroup` doesn't contain any students in case to limit access to students list, instead of it all students contains in `MegaFaculty`'s `Dictionary<ExtraGroup, List<ExtraStudents>>`
- Student could be added into Group only with `MegaFaculty`'s public method
- Every Group have it's **personal Schedule**, which is given to all Students in Group while adding ones
- Every student have it's **personal Schedule** which evaluates like this: `GroupSchedule + OgnpCourseSchedule`
- `Schedule` is an instance which have an invariant: schedule can't have any lesson's crossings
- Methods to find entities in classes uses `ReferenceEquals` instead of basic `Equals` in order to limit entities initialization out of classes and class's interfaces

## Patterns

- Prototype pattern
- Singleton pattern

# Isu Extra structure:

<div align="center">
  <img height="500" src="https://user-images.githubusercontent.com/82240296/136666608-cc918297-be9d-41aa-96b4-508408068436.png" alt="Structure">
</div>

# Isu Extra structure in text:

- We have an Isu instance, Isu could contain MegaFaculties, which have an interface to add students, groups and OGNP courses 
- OGNP course linked to MegaFaculty and have it's studying streams with different lessons schedule
- Student can choose any OGNP courses except he's megafaculty's ones, if they aren't cross with his personal lessons schedule
- Student's schedule initializes while adding in group: group's lessons schedule copies into student's one
- Each student, group, OGNP course have it's unique field: Student - Id, Group - Name, OGNP - Name
- You can't have 2 students with same ids, 2 groups with same names and 2 courses with same names - in the opposite case you'll have an exception 
