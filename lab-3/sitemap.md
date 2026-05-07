# Semantic Routing Model

## Custom attribute routes

| URL                   | Controller               | Action  | View                                |
| --------------------- | ------------------------ | ------- | ----------------------------------- |
| /                     | HomeController           | Index   | Views/Home/Index.cshtml             |
| /privacy              | HomeController           | Privacy | Views/Home/Privacy.cshtml           |
| /error                | HomeController           | Error   | Views/Shared/Error.cshtml           |
| /build-team           | BuildTeamController      | Index   | Views/BuildTeam/Index.cshtml        |
| /circuits             | CircuitsController       | Index   | Views/Circuits/Index.cshtml         |
| /circuits/{id}        | CircuitsController       | Details | Views/Circuits/Details.cshtml       |
| /constructors         | ConstructorsController   | Index   | Views/Constructors/Index.cshtml     |
| /constructors/{id}    | ConstructorsController   | Details | Views/Constructors/Details.cshtml   |
| /drivers              | DriversController        | Index   | Views/Drivers/Index.cshtml          |
| /drivers/{id}         | DriversController        | Details | Views/Drivers/Details.cshtml        |
| /fantasy-leagues      | FantasyLeaguesController | Index   | Views/FantasyLeagues/Index.cshtml   |
| /fantasy-leagues/{id} | FantasyLeaguesController | Details | Views/FantasyLeagues/Details.cshtml |
| /fantasy-teams        | FantasyTeamsController   | Index   | Views/FantasyTeams/Index.cshtml     |
| /fantasy-teams/{id}   | FantasyTeamsController   | Details | Views/FantasyTeams/Details.cshtml   |
| /race-results         | RaceResultsController    | Index   | Views/RaceResults/Index.cshtml      |
| /race-results/{id}    | RaceResultsController    | Details | Views/RaceResults/Details.cshtml    |
| /races                | RacesController          | Index   | Views/Races/Index.cshtml            |
| /races/{id}           | RacesController          | Details | Views/Races/Details.cshtml          |
| /users                | UsersController          | Index   | Views/Users/Index.cshtml            |
| /users/{id}           | UsersController          | Details | Views/Users/Details.cshtml          |

## Default conventional routes

| URL pattern                  | Controller               | Action  | View                                |
| ---------------------------- | ------------------------ | ------- | ----------------------------------- |
| /Home/Index                  | HomeController           | Index   | Views/Home/Index.cshtml             |
| /Home/Privacy                | HomeController           | Privacy | Views/Home/Privacy.cshtml           |
| /Home/Error                  | HomeController           | Error   | Views/Shared/Error.cshtml           |
| /BuildTeam/Index             | BuildTeamController      | Index   | Views/BuildTeam/Index.cshtml        |
| /Circuits/Index              | CircuitsController       | Index   | Views/Circuits/Index.cshtml         |
| /Circuits/Details/{id}       | CircuitsController       | Details | Views/Circuits/Details.cshtml       |
| /Constructors/Index          | ConstructorsController   | Index   | Views/Constructors/Index.cshtml     |
| /Constructors/Details/{id}   | ConstructorsController   | Details | Views/Constructors/Details.cshtml   |
| /Drivers/Index               | DriversController        | Index   | Views/Drivers/Index.cshtml          |
| /Drivers/Details/{id}        | DriversController        | Details | Views/Drivers/Details.cshtml        |
| /FantasyLeagues/Index        | FantasyLeaguesController | Index   | Views/FantasyLeagues/Index.cshtml   |
| /FantasyLeagues/Details/{id} | FantasyLeaguesController | Details | Views/FantasyLeagues/Details.cshtml |
| /FantasyTeams/Index          | FantasyTeamsController   | Index   | Views/FantasyTeams/Index.cshtml     |
| /FantasyTeams/Details/{id}   | FantasyTeamsController   | Details | Views/FantasyTeams/Details.cshtml   |
| /RaceResults/Index           | RaceResultsController    | Index   | Views/RaceResults/Index.cshtml      |
| /RaceResults/Details/{id}    | RaceResultsController    | Details | Views/RaceResults/Details.cshtml    |
| /Races/Index                 | RacesController          | Index   | Views/Races/Index.cshtml            |
| /Races/Details/{id}          | RacesController          | Details | Views/Races/Details.cshtml          |
| /Users/Index                 | UsersController          | Index   | Views/Users/Index.cshtml            |
| /Users/Details/{id}          | UsersController          | Details | Views/Users/Details.cshtml          |
