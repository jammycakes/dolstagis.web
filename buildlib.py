from functools import reduce
import os.path
import shutil
from subprocess import check_call


class Project:

    def _abspath(self, path):
        return os.path.abspath(os.path.join(self.root_dir, path))


    # ====== Constructor ====== #

    """
    Creates a new instance of the Project class.
    @param root
        The root directory of the repository
    @param build
        The subdirectory into which the build artefacts are to be placed
    """
    def __init__(self, root, build):
        if os.path.isfile(root):
            self.root_dir = os.path.abspath(os.path.dirname(root))
        else:
            self.root_dir = os.path.abspath(root)
        self.build_dir = self._abspath(build)

    """
    Post initialisation setup steps.
    """
    def start(self):
        self.file_version = self.version + '.' + str(self.build_number)
        if self.versioninfo:
            self.informational_version = self.version + '-' + self.versioninfo + '.' + str(self.build_number)
            self.nuget_version = self.version + '-' + self.versioninfo + \
                (str(self.build_number) if self.build_number > 0 else '')
        else:
            self.informational_version = self.file_version
            self.nuget_version = self.file_version
        
        print ('Building version: ' + self.informational_version)

    # ====== clean ====== #

    """
    Deletes the build output from previous builds.
    """
    def clean(self):
        if os.path.isdir(self.build_dir):
            shutil.rmtree(self.build_dir)
        os.makedirs(self.build_dir)


    # ====== write_version ====== #

    def write_version(self, path):
        versionfile = '''
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyVersion("%(version)s")]
[assembly: AssemblyFileVersion("%(version)s")]
[assembly: AssemblyInformationalVersion("%(versioninfo)s")]
''' % { 'version' : self.file_version, 'versioninfo' : self.informational_version }

        with open(self._abspath(path), 'w') as vf:
            vf.write(versionfile)


    # ====== run ====== #

    """
    Executes a process from the command line.
    """
    def run(self, path, args):
        check_call([path] + list(args))


    # ====== msbuild ====== #

    """
    Invokes MSBuild to build the solution or a .proj file
    """
    def msbuild(self, build_file, *targets, **properties):
        MSBUILD = 'C:\\Program Files (x86)\\MSBuild\\12.0\\Bin\\MSBuild.exe'
        if not properties.get('Configuration'):
            properties['Configuration'] = self.configuration
        args = [self._abspath(build_file)]
        if targets:
            args.append('/target:' + reduce(lambda a,b: a+','+b, targets))
        for prop in properties:
            args.append('/p:' + prop + '=' + properties[prop])
        self.run(MSBUILD, args)


    # ====== nunit ====== #

    """
    Runs the unit tests against a given NUnit project.
    """
    def nunit(self, nunit_project):
        NUNIT = self._abspath('src/packages/NUnit.Runners.2.6.3/tools/nunit-console.exe')
        args = [self._abspath(nunit_project), '/config=' + self.configuration]
        self.run(NUNIT, args)

    # ====== make_nuget ====== #

    """
    Prepares a NuGet package.
    """
    def make_nuget(self, project, **folders):
        nuspec = self._abspath('src/' + project + '/' + project + '.nuspec')
        if not os.path.isfile(nuspec):
            return
        nuget_base = os.path.join(self.build_dir, '.nuget')
        nuget_project = os.path.join(nuget_base, project)
        nuget_lib = os.path.join(nuget_project, 'lib')
        nuget_content = os.path.join(nuget_project, 'content')
        built_lib = self._abspath('src/' + project + '/bin/' + self.configuration)
        shutil.copytree(
            built_lib, nuget_lib,
            ignore = lambda d, x: [a for a in x if
                                   (not a.lower().startswith(project.lower() + '.'))
                                   or a.lower().endswith('.pdb')
                                   ]
        )
        shutil.copy2(nuspec, nuget_project)
        for folder in folders:
            destdir = os.path.join(nuget_project, folder)
            os.makedirs(destdir)
            for f in folders[folder]:
                src = self._abspath('src/' + project + '/' + f)
                dest = os.path.join(destdir, f)
                shutil.copy2(src, dest)
        NUGET = self._abspath('src/.nuget/NuGet.exe')
        self.run(NUGET, [
            'pack',
            os.path.join(nuget_project, project + '.nuspec'),
            '-OutputDirectory', self.build_dir,
            '-Version', self.nuget_version
        ])
