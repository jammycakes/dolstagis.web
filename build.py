import os.path
from buildlib import *
import argparse
import sys

VERSION = '0.4.0'
VERSIONINFO = 'alpha'
BUILD = 1
CONFIGURATION = 'Debug'

argparser = argparse.ArgumentParser(description = 'Build Dolstagis.Web')
argparser.add_argument('configuration', default = CONFIGURATION, nargs = '?')
argparser.add_argument('-v', '--version', default = VERSION)
argparser.add_argument('-i', '--versioninfo', default = VERSIONINFO)
argparser.add_argument('-n', '--build', default=BUILD, type=int)
args = argparser.parse_args(sys.argv[1:])

project = Project(__file__, 'build')

project.version = args.version
project.versioninfo = args.versioninfo
project.build_number = args.build
project.configuration = args.configuration

project.start()
project.clean()
project.restore_packages()
project.write_version('src/.version/VersionInfo.cs')
project.msbuild('src/Dolstagis.Web.sln', 'Clean', 'Build', Platform='Any CPU')
project.nunit('src/Dolstagis.Tests/Dolstagis.Tests.nunit')

project.make_nuget('Dolstagis.Web')
project.make_nuget('Dolstagis.Web.Aspnet',
    content = [
        'Index.cs.pp',
        'MainFeature.cs.pp',
        'Web.config.transform'
    ]
)
project.make_nuget('Dolstagis.Web.Views.Nustache')