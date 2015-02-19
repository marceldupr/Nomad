# Nomad
## Overview

**Nomad**, a simple database migration tool for *developers*

## Installation

PM> Install-Package Nomad

### Usage

**Execute from command-line**

  `Nomad "pathtomigrations\migration.dll" ["connectionstring" | server dbname [user password]]`

**Examples**

*Integrated Security (Will create database if not exist. Current user has to have admin rights)*

  `Nomad "C:\Deploy\Database\Persistence.Migrations.dll" . NomadTest`

*Sql User (Will create database if not exist. Specified user has to have admin rights)*

  `Nomad "C:\Deploy\Database\Persistence.Migrations.dll" . NomadTest admin p@$$20rd`

*Existing Database*

  `Nomad "C:\Deploy\Database\Persistence.Migrations.dll" "Data Source=.; Initial Catalog=NomadTest;User Id=admin;Password=p@$$20rd"`

**Execute from Nant**

    <exec program="${path.to.nomad}\Nomad.exe">
        <arg value="${path.to.migrations}\Persistence.Migrations.dll"/>
        <arg value="${database.server}" />
        <arg value="${database.name}" />
    </exec>
    
**Execute from Powershell**

		& "Migrations\Nomad.exe" Migrations\Migrations.dll ${database.server} ${database.name} ${database.useradmin} ${database.passwordadmin}

### Packaging
At the command line, navigate to src/Nomad and run:

    nuget spec Nomad.csproj
Check the generated .nuspec file looks OK, then run this to create the package:

    nuget pack Nomad.csproj �Prop Configuration=Release �IncludeReferencedProjects

If not done already, set NuGet API key:

    nuget setApiKey xxxx-xxxx-xxxx-xxxx

Then push to nuget.org as follows:

    nuget push