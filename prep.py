import os
import sys
import logging
import shutil

# CONFIGURATION

# Guess.
SE_ROOT_DIR = os.path.abspath('F:\SteamLibrary\SteamApps\common\SpaceEngineers')

Configurations = ('Release', 'Debug')
Platforms = ('x86', 'x64')
Libraries = {
    'HavokWrapper_SE': {
        'x86': [
            'Bin/HavokWrapper.dll',
            'Bin/HavokWrapper.xml',
        ],
        'x64': [
            'Bin64/HavokWrapper.dll',
            'Bin64/HavokWrapper.xml',
        ],
    },
    'VRage.Native': {
        'x86': [
            'Bin/VRage.Native.dll'
        ],
        'x64': [
            'Bin64/VRage.Native.dll'
        ],
    },
    # 'RakNet': {
    #    'x86': ['Bin/RakNet.dll'],
    #    'x64': ['Bin64/RakNet.dll'],
    # },
    'SteamSDK': {
        'x86': [
            'Bin/SteamSDK.dll',
            'Bin/steam_api.dll',
        ],
        'x64': [
            'Bin64/SteamSDK.dll',
            'Bin64/steam_api.dll',
            'Bin64/steam_api64.dll',
        ],
    }
}


if __name__ == '__main__':
  logging.basicConfig(
      format='%(asctime)s [%(levelname)-8s]: %(message)s',
      datefmt='%m/%d/%Y %I:%M:%S %p',
      level=logging.DEBUG
  )

  for c in Configurations:
    for p in Platforms:
      logging.info('Setting up %s/%s...', c, p)
      for libname in Libraries.keys():
        for lib in Libraries[libname][p]:
          # print(repr(lib))
          frompath = os.path.join(SE_ROOT_DIR, lib)
          topath = os.path.join('3rd', libname, c.lower(), p)
          if not os.path.isdir(topath):
            logging.info('  mkdir -p %s', topath)
            os.makedirs(topath)
          tofile = os.path.join(topath, os.path.basename(lib))
          if os.path.isfile(tofile):
            # os.remove(tofile)
            continue
          logging.info('  copy %s -> %s', frompath, topath)
          shutil.copy2(frompath, topath)
