#!/usr/bin/env python

import os.path
from buildlib import *

project = Project(__file__, 'build')
project.version = '0.1.0.0'
project.versioninfo = False

project.clean()
project.write_version('src/.version/VersionInfo.cs')
project.msbuild('src/Dolstagis.Web.sln', 'Clean', 'Build', Configuration='Debug', Platform='Any CPU')
