#!/usr/bin/env python3

import datetime
import json
import re
import shutil
from pathlib import Path
from typing import assert_never

SRC_FILE = Path(
    "C:/Program Files (x86)/Steam/steamapps/common/Wild Skies/LostSkies_Data/"
    "LostSkiesDataDump/data.json"
)
DST_DIR = Path("output")
GITIGNORE_FILENAME = ".gitignore"
GITIGNORE_TEXT = """*
"""
GAME_PREFIX = "LS"
PLUGIN_PREFIX = "LSDD"
MISSING_VERSION = "UNKNOWN"
RE_COMMENT = re.compile(
    r"""
    \s*  # Possible indent.
    /\*  # Start of comment.
    .*  # Content of comment.
    \*/  # End of comment.
    """,
    re.VERBOSE,
)


def main(src_file: Path, dst_dir: Path) -> None:
    dst_dir.mkdir(parents=True, exist_ok=True)
    make_gitignore(parent_dir=dst_dir)
    copy_file(src_path=src_file, dst_dir=dst_dir)


def make_gitignore(parent_dir: Path) -> None:
    gitignore = parent_dir / GITIGNORE_FILENAME
    if not gitignore.exists():
        with gitignore.open("xt") as fp:
            fp.write(GITIGNORE_TEXT)
    elif not gitignore.is_file():
        raise FileNotFoundError(f"{gitignore} exists, but is not a file")


def copy_file(src_path: Path, dst_dir: Path) -> None:
    dst_path = dst_dir / make_dest_name(src_path=src_path)
    shutil.copy(src=src_path, dst=dst_path)


def make_dest_name(src_path: Path) -> str:
    timestamp = get_timestamp(src_path=src_path)
    data = load_data_dump(fpath=src_path)
    game_version = get_game_version(data)
    plugin_version = get_plugin_version(data)
    suffix = src_path.suffix
    return f"""{timestamp}_{GAME_PREFIX}-{game_version}_{PLUGIN_PREFIX}-{
        plugin_version
    }{suffix}"""


def get_timestamp(src_path: Path) -> str:
    file_stat = src_path.stat()
    dt = datetime.datetime.fromtimestamp(
        timestamp=file_stat.st_mtime, tz=datetime.UTC
    )
    return dt.strftime("%Y-%m-%d_%H-%M-%S")


def load_data_dump(fpath: Path) -> dict:
    with fpath.open("rt", encoding="utf-8") as fp:
        return json.loads(RE_COMMENT.sub("", fp.read()))


def get_game_version(data: dict) -> str:
    game_version_info = data.get("GameVersionInfo")
    if not isinstance(game_version_info, dict):
        return MISSING_VERSION
    version = game_version_info.get("version")
    if not isinstance(version, str):
        return MISSING_VERSION
    return version


def get_plugin_version(data: dict) -> str:
    plugin_version_info = data.get("PluginVersionInfo")
    if not isinstance(plugin_version_info, dict):
        return MISSING_VERSION
    version = plugin_version_info.get("Version")
    if version is not None and not isinstance(version, str):
        version = None
    git_hash = plugin_version_info.get("GitHash")
    if git_hash is not None and not isinstance(git_hash, str):
        git_hash = None
    parts = (version, git_hash)
    match parts:
        case (str(version), str(git_hash)):
            return f"{version}-{git_hash}"
        case (str(version), None):
            return version
        case (None, str(git_hash)):
            return git_hash
        case (None, None):
            return MISSING_VERSION
        case _:
            assert_never(parts)


if __name__ == "__main__":
    main(src_file=SRC_FILE, dst_dir=DST_DIR)
