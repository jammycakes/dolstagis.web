import os.path
from buildlib import *

project = Project(__file__, 'build')

project.version = '0.0.0'
project.versioninfo = 'alpha'
project.build_number = 0
project.configuration = 'Debug'

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
