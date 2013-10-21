#!/usr/bin/env python

import os.path
from buildlib import *

CONFIGURATION = 'Debug'

project = Project(__file__, 'build')

project.version = '0.1.0'
project.versioninfo = 'alpha'
project.build_number = 0
project.configuration = 'Release'

project.start()
project.clean()
project.write_version('src/.version/VersionInfo.cs')
project.msbuild('src/Dolstagis.Web.sln', 'Clean', 'Build', Platform='Any CPU')
project.nunit('src/Dolstagis.Tests/Dolstagis.Tests.nunit')

project.make_nugets(
    'Dolstagis.Web',
    'Dolstagis.Web.Aspnet',
    # 'Dolstagis.Web.Owin',
    'Dolstagis.Web.Views.Nustache'
)
