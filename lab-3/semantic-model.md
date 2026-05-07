# Semantic DB Model

## Models and main properties

| Model/Class/Table | Main properties |
| --- | --- |
| User | Id, Name, Surname, Email, PasswordHash, Role |
| FantasyTeam | Id, Name, Budget, UserId, ConstructorId, FantasyLeagueId |
| FantasyLeague | Id, Name, Description, StartDate, EndDate, LeagueType |
| Driver | Id, Name, Surname, Number, Price, ConstructorId |
| Constructor | Id, Name, Nationality, FoundedDate |
| Race | Id, Name, RaceDate, CircuitId |
| Circuit | Id, Name, Country, City, Length, NumberOfLaps |
| RaceResult | Id, FinishedPosition, ScoredPoints, DriverId, RaceId, DriverStatus |
| FantasyTeamDrivers (join) | FantasyTeamId, DriverId |

## Relationships

| Relationship | Type | Notes |
| --- | --- | --- |
| User -> FantasyTeam | 1-N | One user can have many fantasy teams. |
| FantasyLeague -> FantasyTeam | 1-N | One league can have many fantasy teams. |
| Constructor -> Driver | 1-N | One constructor can have many drivers. |
| Circuit -> Race | 1-N | One circuit can have many races. |
| Race -> RaceResult | 1-N | One race can have many race results. |
| Driver -> RaceResult | 1-N | One driver can have many race results. |
| FantasyTeam -> Driver | M-N | Many teams can have many drivers via FantasyTeamDrivers. |
| Constructor -> FantasyTeam | 1-N | One constructor can be selected by many fantasy teams. |
