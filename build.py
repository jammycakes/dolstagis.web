#!/usr/bin/env python

import os.path
from buildlib import *

CONFIGURATION = 'Debug'

project = Project(__file__, 'build')
project.version = '0.1.0.0'
project.versioninfo = False

project.clean()
project.write_version('src/.version/VersionInfo.cs')
project.msbuild('src/Dolstagis.Web.sln', 'Clean', 'Build', Configuration=CONFIGURATION, Platform='Any CPU')
project.nunit('src/Dolstagis.Tests/Dolstagis.Tests.nunit', CONFIGURATION)