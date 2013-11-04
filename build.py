import os.path
from buildlib import *
import argparse
import sys

argparser = argparse.ArgumentParser(description = 'Build Dolstagis.Web')
argparser.add_argument('configuration', default = 'Debug', nargs = '?')
argparser.add_argument('-v', '--version', default = '0.1.0')
argparser.add_argument('-i', '--versioninfo', default = 'alpha')
argparser.add_argument('-n', '--build', default=0, type=int)
args = argparser.parse_args(sys.argv[1:])

project = Project(__file__, 'build')

project.version = args.version
project.versioninfo = args.versioninfo
project.build_number = args.build
project.configuration = args.configuration

project.start()
project.clean()
project.write_version('src/.version/VersionInfo.cs')
project.msbuild('src/Dolstagis.Web.sln', 'Clean', 'Build', Platform='Any CPU')
project.nunit('src/Dolstagis.Tests/Dolstagis.Tests.nunit')

project.make_nuget('Dolstagis.Web')
project.make_nuget('Dolstagis.Web.Aspnet',
    content = ['web.config.transform']
)
project.make_nuget('Dolstagis.Web.Views.Nustache')
