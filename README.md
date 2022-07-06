# Cint Robot Cleaner Task - C#

## Design Decisions

* All the classes and functions are written in compliance with SOLID principles. Primarily the `Single Responsibility and `Open Closed` principles.
* Validations are implemented to restrict invalid operations (e.g., not providing starting position).
* For the `IRobotService` implementation, no boundary values are checked as a cleaning plain can be very large.
* In the case of the execution of the cleaning with zero commands, the starting position is cleaned.
* I have tried to cover all the use cases with the unit tests. Unit test inputs are chosen randomly.
* Inputs are not validated to comply with the problem statement.

## Notes
* It took 4-5 hours to complete the task, including the unit tests.
