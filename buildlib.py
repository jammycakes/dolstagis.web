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
        informational_version = self.version + ('-' + self.versioninfo if self.versioninfo else '')
        versionfile = '''
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyVersion("%(version)s")]
[assembly: AssemblyFileVersion("%(version)s")]
[assembly: AssemblyInformationalVersion("%(versioninfo)s")]
''' % { 'version' : self.version, 'versioninfo' : informational_version }

        with open(self._abspath(path), 'w') as vf:
            vf.write(versionfile)

